using System;
using System.Collections.Generic;
using System.Net;
using Common.Api;
using Common.Models;
using Common.Utils;
using NetworkCallController.Model;
using NLog;
using static Common.Models.ResponsePacket;

namespace NetworkCallController
{
    public class ConnectionRequest
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Random _rnd = new Random();
        private readonly Dictionary<string, string> _clientPortAliases;
        private readonly Dictionary<string, string> _portDomains;
        private readonly string _domain;

        private IApiClient<RequestPacket, ResponsePacket> _ccConnectionRequestClient;
        
        //TODO Make those two accessible from other On...Received methods/classes
        private readonly List<Connection> _connections;
        private int _connectionCounter;

        public ConnectionRequest(Dictionary<string, string> clientPortAliases, 
                                 Dictionary<string, string> portDomains,
                                 string domain,
                                 IPAddress serverAddress, 
                                 int ccConnectionRequestRemotePort)
        {
            _clientPortAliases = clientPortAliases;
            _portDomains = portDomains;
            _domain = domain;
            _ccConnectionRequestClient = 
                new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccConnectionRequestRemotePort);
            _connections = new List<Connection>();
            _connectionCounter = 0;
        }
        
        public ResponsePacket OnConnectionRequestReceived(RequestPacket requestPacket)
        {
            // Get ConnectionRequest_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            string srcName = requestPacket.SrcName;
            string dstName = requestPacket.DstName;
            int slotsNumber = requestPacket.SlotsNumber;

            LOG.Info($"Received NCC::ConnectionRequest_{GenericPacket.PacketTypeToString(type)}" +
                     $"(srcName = {srcName}, dstName = {dstName},slotsNumber = {slotsNumber}");
            // < C A L L   A D M I S S I O N   C O N T R O L >
            LOG.Info("Call Admission Control");
            // P O L I C Y
            // Randomize chance of rejecting ConnectionRequest_req by Policy component
            int chanceToRejectRequestInPolicy = _rnd.Next(0, 100);
            if (chanceToRejectRequestInPolicy > 5)
                LOG.Info($"ConnectionRequest meets conditions of Policy component");
            else
                return new Builder()
                    .SetRes(ResponseType.AuthProblem)
                    .Build();
            // D I R E C T O R Y 
            // Find srcName and dstName ports in clientPortAliases dictionary
            string srcPort = null;
            string dstPort = null;
            foreach (string clientPortName in _clientPortAliases.Keys)
            {
                if (clientPortName == srcName)
                    srcPort = _clientPortAliases[clientPortName];
                if (clientPortName == dstName)
                    dstPort = _clientPortAliases[clientPortName];
            }
            
            // If Directory couldn't find dstPort send NCC::ConnectionRequest_res(res=No client);
            if (dstPort == null)
            {
                LOG.Info($"Directory could not find port for user {srcName}");
                LOG.Info($"NCC::ConnectionRequest_res(res = {ResponseTypeToString(ResponseType.NoClient)})");
                return new Builder()
                    .SetRes(ResponseType.NoClient)
                    .Build();
            }
            // </ C A L L   A D M I S S I O N   C O N T R O L >
            
            //TODO [ASON] Ask dstClient if he wants to connect with srcClient
            
            // If CAC is passed, create a connection
            int connectionId = int.Parse($"{_domain[1]}{_connectionCounter++}{srcPort}{dstPort}");
            LOG.Trace($"connectionId: {connectionId}");
            Connection newConnection = new Connection(connectionId, srcName, srcPort, dstName, dstPort, slotsNumber);
            _connections.Add(newConnection);
            // Output active connections
            LOG.Info("Active Connections: ");
            foreach (var con in _connections)
            {
                LOG.Info(con.ToString);
            }

            // Check if dstPort is from NCC's domain or outside the domain
            string dstDomain = GetDomainFromPort(dstPort);
            bool outsideDomain = (dstDomain != _domain);

            if (outsideDomain)
            {
                // Inter domain connection
                //TODO [ASON] Implement this. NCC just needs to ask its peer NCC if he wants a call
                return null;
            }
            else
            {
                // Intra domain connection
                // Send CC:ConnectionRequest(id, src, dst, sl)
                ResponsePacket connectionRequestResponse = _ccConnectionRequestClient.Get(new RequestPacket.Builder()
                    .SetId(newConnection.Id) 
                    .SetSrcPort(newConnection.SrcPortAlias)
                    .SetDstPort(newConnection.DstPortAlias)
                    .SetSlotsNumber(newConnection.SlotsNumber)
                    .Build());
                // GET CC::ConnectionRequest_res(res, nextZonePort, slots)
                // In inter domain connection we are ignoring nextZonePort and slots
                ResponseType res = connectionRequestResponse.Res;
                switch (res)
                {
                        case ResponseType.Ok:
                        {
                            // Send NCC::ConnectionRequest_res(res=OK, id = newConnection.Id)
                            LOG.Info($"Send NCC::ConnectionRequest_res(res=OK, id = {newConnection.Id})");
                            return new Builder()
                                .SetRes(ResponseType.Ok)
                                .SetId(newConnection.Id)
                                .Build();
                        }
                        default:
                        {
                            // Send NCC::ConnectionRequest_res(res=Network problem)
                            return new Builder()
                                .SetRes(ResponseType.Refused)
                                .Build();
                        }
                }
            }
        }

        private string GetDomainFromPort(string portAlias)
        {
            string domain="";
            // TODO Change to lambda and handle NotFoundException
            foreach (var portDomain in _portDomains)
            {
                if (Checkers.PortMatches(portDomain.Key, portAlias))
                {
                    domain = portDomain.Value;
                    break;
                }
            }
            return domain;
        }
    }
}
