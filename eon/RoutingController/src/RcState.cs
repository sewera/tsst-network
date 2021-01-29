using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Common.Utils;
using NLog;
using RoutingController.Config;
using RoutingController.Model;

namespace RoutingController
{
    public class RcState : IRcState
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly List<Connection> _connections;
        private readonly List<Configuration.RouteTableRow> _routeTable;
        private readonly List<Link> _links;

        public RcState(List<Configuration.RouteTableRow> routeTable)
        {
            _connections = new List<Connection>();
            _links = new List<Link>();
            _routeTable = routeTable;
        }

        public ResponsePacket OnRouteTableQuery(RequestPacket requestPacket)
        {
            // Get RouteTableQuery_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            int connectionId = requestPacket.Id;
            string srcPort = requestPacket.SrcPort;
            string dstPort = requestPacket.DstPort;
            int slotsNumber = requestPacket.SlotsNumber;
            
            LOG.Info($"Received RC::RouteTableQuery_req" + $"(connectionId = {connectionId}, srcPort = {srcPort}," +
                     $" dstPort = {dstPort}, slotsNumber = {slotsNumber}");

            // Set dstZone depending on destination domain
            string secondDomainPortPattern = "3xx";
            string dstZone;
            if (Checkers.PortMatches(secondDomainPortPattern, dstPort))
            {
                dstZone = "012,021";
                LOG.Trace($"Destination port belongs to other zone. Possible leaving ports: {dstZone}");
            }
            else
            {
                dstZone = dstPort;
                LOG.Trace($"Destination port belongs to the same zone. Leaving port: {dstZone}");
            }

            string gateway = "0";
            (int, int) slots = (0, 0);
            // Check whether we don't already have a registered connection with given connectionId
            foreach (Connection connection in _connections.Where(connection => connection.Id == connectionId))
            {
                slots = connection.Slots;
            }
            
            // If there is no connection with given id we create new one with new pair of slots
            if (!(_connections.Any(connection => connection.Id == connectionId)))
            {
                // We search through the RIB until we find pattern matching our src and dst port
                foreach (Configuration.RouteTableRow row in _routeTable)
                {
                    // If we find it we set gateway to the one from that row
                    if (Checkers.PortMatches(row.Src, srcPort) && Checkers.PortMatches(row.Dst, dstPort))
                    {
                        if (Checkers.MultipleGatewaysInRibRow(row.Gateway))
                        {
                            string[] possibleGateways = row.Gateway.Split(",");
                            gateway = possibleGateways.First();
                            // TODO: losowanko
                        }
                        else gateway = row.Gateway;
                        LOG.Trace($"Found matching gateway: {gateway} for given srcPort: {srcPort} and dstPort: {dstPort}");

                        slots = CreateSlots(gateway, slotsNumber);
                    }
                    else
                    {
                        LOG.Trace($"Could not find matching gateway for given srcPort: {srcPort} and dstPort: {dstPort}");
                    }
                }
                
                _connections.Add(new Connection(connectionId, slots));
            }

            LOG.Info($"Sending RC::RouteTableQuery_res" + $"(connectionId = {connectionId}, gateway = {gateway}," +
                     $" slots = {slots.ToString()}, dstZone = {dstZone}");
            
            return new ResponsePacket.Builder()
                .SetId(connectionId)
                .SetGateway(gateway)
                .SetSlots(slots)
                .SetDstZone(dstZone)
                .Build();
        }

        public ResponsePacket OnLocalTopology(RequestPacket requestPacket)
        {
            // Get OnLocalTopology_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            string port1 = requestPacket.Port1;
            string port2 = requestPacket.Port2;
            List<(int, int)> slotsArray = requestPacket.SlotsArray;

            LOG.Info($"Received RC::OnLocalTopology_req" + $"(port1 = {port1}, port2 = {port2}," +
                     $" slotsArray = {SlotsArrayToString(slotsArray)}");
            
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
            
            for (int i = 1; i < 100 - slotsNumber; i++)
            {
                slots = (i, i + slotsNumber);

                foreach (Link link in _links.Where(link => gateway == link.PortAlias1 || gateway == link.PortAlias2))
                {
                    if (link.SlotsArray.Any(slotsTuple => !Checkers.SlotsOverlap(slots, slotsTuple)))
                    {
                        LOG.Trace($"Assigned slots: {slots.ToString()} for gateway: {gateway}");
                        return slots;
                    }
                }
            }

            return (0, 0);
        }

        private string SlotsArrayToString(List<(int, int)> slotsArray)
        {
            string result = "";
            
            foreach ((int, int) slotsTuple in slotsArray)
            {
                result += slotsTuple.ToString() + " ";
            }

            return result;
        }
    }
}
