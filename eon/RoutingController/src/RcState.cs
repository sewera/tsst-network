using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Common.Utils;
using NLog;
using RoutingController.Config;
using RoutingController.Models;

namespace RoutingController
{
    public class RcState : IRcState
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly List<Connection> _connections;
        private readonly List<Configuration.RouteTableRow> _routeTable;
        private readonly List<Link> _links;
        private readonly Dictionary<string, Queue<ResponsePacket>> _responsePackets = new Dictionary<string, Queue<ResponsePacket>>();

        public RcState(List<Configuration.RouteTableRow> routeTable)
        {
            _connections = new List<Connection>();
            _links = new List<Link>();
            _routeTable = routeTable;
        }

        public ResponsePacket OnRouteTableQuery(RequestPacket requestPacket)
        {
            // Get RouteTableQuery_req packet params
            int connectionId = requestPacket.Id;
            string srcPort = requestPacket.SrcPort;
            string dstPort = requestPacket.DstPort;
            int slotsNumber = requestPacket.SlotsNumber;
            RequestPacket.Est est = requestPacket.Establish;

            if (est == RequestPacket.Est.Teardown)
            {
                LOG.Info($"Received RC::RouteTableQuery_req(Teardown, connectionId = {connectionId}, srcPort = {srcPort}," +
                         $" dstPort = {dstPort}, slotsNumber = {slotsNumber})");

                // The best idea will be to store the actual responses in a table / dict and when the Teardown RouteTableQuery
                // arrives, we just match connectionId, srcPort, dstPort, slotsNumber and if they are the same, just return
                // the same ResponsePacket.
                // e.g. instead of just returning ResponsePacket, do this:
                // ResponsePacket responsePacket = new ResponsePacket.Builder().<blablabla>.Build();
                // _responsePackets[requestPacket] = responsePacket;
                // return responsePacket;
                //
                // and then when the Teardown comes, just do:
                LOG.Info($"Sending RC::RouteTableQuery_res(Teardown, connectionId = {connectionId})");
                var teardownResponsePacket = _responsePackets[GetUniqueRcId(connectionId, srcPort, dstPort, slotsNumber)].Dequeue();
                if (teardownResponsePacket == null)
                {
                    LOG.Error("Could not find such connection");
                    return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Refused).Build();
                }
                return teardownResponsePacket;
            }
            
            LOG.Info($"Received RC::RouteTableQuery_req(connectionId = {connectionId}, srcPort = {srcPort}," +
                     $" dstPort = {dstPort}, slotsNumber = {slotsNumber})");

            // Set dstZone depending on destination domain
            string secondDomainPortPattern = "3xx";
            string dstZone;
            if (Checkers.PortMatches(secondDomainPortPattern, dstPort) > -1) // TODO: Check for matches value
            {
                dstZone = "012,021";
                LOG.Trace($"Destination port belongs to other zone. Possible leaving ports: {dstZone}");
            }
            else
            {
                dstZone = dstPort;
                LOG.Trace($"Destination port belongs to the same zone. Leaving port: {dstZone}");
            }

            string gateway = GetBestGateway(srcPort, dstPort) ?? "0";
            (int, int) slots = (0, 0);
            // Check whether we don't already have a registered connection with given connectionId
            if (!_connections.Exists(connection => connection.Id == connectionId))
            {
                try
                {
                    slots = CreateSlots(gateway, slotsNumber);
                }
                catch (Exception e)
                {
                    LOG.Info(e.Message);
                    return new ResponsePacket.Builder()
                        .SetRes(ResponsePacket.ResponseType.ResourcesProblem)
                        .Build();
                }

                _connections.Add(new Connection(connectionId, slots));
                LOG.Trace($"Allocated slots {slots} for new connection with id {connectionId}");
            }
            else
            {
                slots = _connections.Find(connection => connection.Id == connectionId).Slots;
                LOG.Trace($"There is already registered connection with id {connectionId}. Allocated slots: {slots}");
            }

            ResponsePacket responsePacket = new ResponsePacket.Builder()
                .SetRes(ResponsePacket.ResponseType.Ok)
                .SetId(connectionId)
                .SetGateway(gateway)
                .SetSlots(slots)
                .SetDstZone(dstZone)
                .Build();

            RequestPacket requestPacketWhenTeardown = requestPacket;
            requestPacketWhenTeardown.Establish = RequestPacket.Est.Teardown;
            string uniqueRcId = GetUniqueRcId(connectionId, srcPort, dstPort, slotsNumber);
            if (_responsePackets.ContainsKey(uniqueRcId))
            {
                LOG.Trace($"_responsePacketsTable contains key: {uniqueRcId}");
            }
            else
            {
                _responsePackets[uniqueRcId] = new Queue<ResponsePacket>();
            }
            _responsePackets[uniqueRcId].Enqueue(responsePacket);

            LOG.Info($"Sending RC::RouteTableQuery_res" + $"(connectionId = {connectionId}, gateway = {gateway}," +
                     $" slots = {slots.ToString()}, dstZone = {dstZone})");
            
            return responsePacket;
        }

        public ResponsePacket OnLocalTopology(RequestPacket requestPacket)
        {
            // Get OnLocalTopology_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            string port1 = requestPacket.Port1;
            string port2 = requestPacket.Port2;
            List<(int, int)> slotsArray = requestPacket.SlotsArray;

            LOG.Info($"Received RC::OnLocalTopology_req" + $"(port1 = {port1}, port2 = {port2}," +
                     $" slotsArray = {SlotsArrayToString(slotsArray)})");
            
            // If it's first OnLocalTopology_req for this link just add it to the _links List
            if (!(_links.Any((link => link.PortAlias1 == port1 && link.PortAlias2 == port2))))
            {
                _links.Add(new Link(port1, port2, slotsArray));
            }
            else
            {
                // If link between given 2 ports already exists in _links update its SlotsArray
                foreach (Link link in _links.Where(link => link.PortAlias1 == port1 && link.PortAlias2 == port2))
                {
                    link.SlotsArray = slotsArray;
                }
            }
            
            LOG.Info($"Sending RC::OnLocalTopology_res" + $"(res = Ok)");
            
            return new ResponsePacket.Builder()
                .SetRes(ResponsePacket.ResponseType.Ok)
                .Build();
        }

        public ResponsePacket OnNetworkTopology(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
        }

        private (int, int) CreateSlots(string gateway, int slotsNumber)
        {
            (int, int) slots;

            // Naive approach, slots are assigned regardless of a route.
            for (int i = 1; i < 100 - slotsNumber; i++)
            {
                slots = (i, i + slotsNumber);

                if (_connections.Exists(connection => Checkers.SlotsOverlap(connection.Slots, slots)))
                    continue;

                return slots;
            }

            throw new Exception("Slots could not be allocated");
        }

        private string SlotsArrayToString(List<(int, int)> slotsArray)
        {
            string result = "";
            
            foreach ((int, int) slotsTuple in slotsArray)
            {
                result += slotsTuple + " ";
            }

            return result;
        }

        public string GetBestGateway(string srcPort, string dstPort)
        {
            int maxPrioritySrc = -1;
            int maxPriorityDst = -1;
            string bestGateway = null;
            foreach (Configuration.RouteTableRow ribRow in _routeTable)
            {
                if (Checkers.PortMatches(ribRow.Src, srcPort) == -1 || Checkers.PortMatches(ribRow.Dst, dstPort) == -1)
                    continue;

                int priorityDst = Checkers.PortMatches(ribRow.Dst, dstPort);
                if (priorityDst > maxPriorityDst)
                {
                    maxPriorityDst = priorityDst;
                    maxPrioritySrc = Checkers.PortMatches(ribRow.Src, srcPort);
                    bestGateway = Choosers.GetGatewayFromRibRow(ribRow.Gateway);
                    continue;
                }

                int prioritySrc = Checkers.PortMatches(ribRow.Src, srcPort);
                if (prioritySrc > maxPrioritySrc)
                {
                    maxPrioritySrc = prioritySrc;
                    bestGateway = Choosers.GetGatewayFromRibRow(ribRow.Gateway);
                }
            }

            if (bestGateway == null)
                LOG.Error($"Could not find route for srcPort = {srcPort} and dstPort = {dstPort}");
            else
                LOG.Trace($"Found matching gateway: {bestGateway} for srcPort = {srcPort} and dstPort = {dstPort}");

            return bestGateway;
        }

        private static string GetUniqueRcId(int connectionId, string srcPort, string dstPort, int slotsNumber)
        {
            return $"{connectionId}{srcPort}{dstPort}{slotsNumber}";
        }
    }
}
