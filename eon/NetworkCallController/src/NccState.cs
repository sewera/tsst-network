using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Common.Api;
using Common.Models;
using Common.Utils;
using NetworkCallController.Model;
using NLog;
using static Common.Models.ResponsePacket;

namespace NetworkCallController
{
    public class NccState
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Random _rnd = new Random();
        private readonly Dictionary<string, string> _clientPortAliases;
        private readonly Dictionary<string, string> _portDomains;
        private readonly string _domain;
        private readonly IPAddress _serverAddress;

        private readonly IApiClient<RequestPacket, ResponsePacket> _ccConnectionRequestClient;
        private readonly IApiClient<RequestPacket, ResponsePacket> _nccCallCoordinationClient;

        private readonly List<Connection> _connections;
        private int _connectionCounter;

        private readonly Dictionary<int, RequestPacket> _ccConnectionRequests = new Dictionary<int, RequestPacket>();

        public NccState(Dictionary<string, string> clientPortAliases,
                        Dictionary<string, string> portDomains,
                        string domain,
                        IPAddress serverAddress,
                        int ccConnectionRequestRemotePort,
                        int nccCallCoordinationRemotePort)
        {
            _clientPortAliases = clientPortAliases;
            _portDomains = portDomains;
            _domain = domain;
            _serverAddress = serverAddress;
            _ccConnectionRequestClient =
                new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccConnectionRequestRemotePort);
            _nccCallCoordinationClient =
                new ApiClient<RequestPacket, ResponsePacket>(serverAddress, nccCallCoordinationRemotePort);
            _connections = new List<Connection>();
            _connectionCounter = 0;
        }

        public ResponsePacket OnCallRequestReceived(RequestPacket requestPacket)
        {
            // Get ConnectionRequest_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            string srcName = requestPacket.SrcName;
            string dstName = requestPacket.DstName;
            int slotsNumber = requestPacket.SlotsNumber;

            LOG.Info($"Received NPCC::CallRequest_req" + $"(srcName = {srcName}, dstName = {dstName}, slotsNumber = {slotsNumber})");
            // < C A L L   A D M I S S I O N   C O N T R O L >
            LOG.Info("Call Admission Control");
            // P O L I C Y
            // Randomize chance of rejecting ConnectionRequest_req by Policy component
            int chanceToRejectRequestInPolicy = _rnd.Next(0, 100);
            if (chanceToRejectRequestInPolicy > 5)
                LOG.Info("Call Request meets conditions of Policy component");
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

            // If Directory couldn't find dstPort send NCC::CallRequest_res(res=No client);
            if (dstPort == null)
            {
                LOG.Info($"Directory could not find port for user {dstName}");
                LOG.Info($"NPCC::CallRequest_res(res = {ResponseTypeToString(ResponseType.NoClient)})");
                return new Builder()
                    .SetRes(ResponseType.NoClient)
                    .Build();
            }
            LOG.Info($"Directory found port: {dstPort} for client: {dstName}");
            // Check if dstPort is from NCC's domain or outside the domain
            string dstDomain = GetDomainFromPort(dstPort);
            bool outsideDomain = dstDomain != _domain;

            ResponseType res; //aux var for storing OneShotClients responses
            if (outsideDomain)
            {
                // Ask second domain NCC 
                // Send NCC::CallCoordination(srcName, dstName, sl)
                LOG.Info("Send NPCC::CallCoordination_req" +
                         $"(srcName = {srcName}, dstName = {dstName}, sl = {slotsNumber})");
                ResponsePacket nccCallCoordinationResponse = _nccCallCoordinationClient.Get(new RequestPacket.Builder()
                    .SetSrcName(srcName)
                    .SetDstName(dstName)
                    .SetSlotsNumber(slotsNumber)
                    .Build());
                res = nccCallCoordinationResponse.Res;
                LOG.Info($"Received NPCC::CallCoordination_res(res = {res})");
                // If second domain NCC refused the call we should also refuse it
                if (res != ResponseType.Ok)
                {
                    LOG.Info($"Second domain refused the call");
                    LOG.Info($"Send NPCC::CallRequest_res(res = {ResponseTypeToString(ResponseType.AuthProblem)})");
                    return new Builder()
                        .SetRes(ResponseType.Refused)
                        .Build();
                }
            }
            
            // C A L L   A C C E P T
            if (!outsideDomain)
            {
                IApiClient<RequestPacket, ResponsePacket> callAcceptClient = new ApiClient<RequestPacket, ResponsePacket>(_serverAddress, int.Parse(dstPort.ToString() + "9"));

                LOG.Info("Send CPCC::CallAccept_req" + $"(srcName = {srcName})");
                ResponsePacket callAcceptRes = callAcceptClient.Get(new RequestPacket.Builder()
                    .SetSrcName(srcName)
                    .Build()
                );
                LOG.Info($"Received CPCC:CallAccept_res(res = {callAcceptRes.Res})");
                if (callAcceptRes.Res == ResponseType.Refused) 
                    return new Builder()
                        .SetRes(ResponseType.RefusedByCalledParty)
                        .Build();
            }
            LOG.Info($"Call Admission Control ended succesfully");
            
            // </ C A L L   A D M I S S I O N   C O N T R O L >
            
            

            // If CAC is passed, create a connection
            int connectionId = int.Parse($"{_domain[1]}{_connectionCounter++}{srcPort}{dstPort}");
            LOG.Trace($"connectionId: {connectionId}");
            Connection newConnection = new Connection(connectionId, srcName, srcPort, dstName, dstPort, slotsNumber);
            _connections.Add(newConnection);
            // Output active connections
            LOG.Info("Active Connections: ");
            foreach (Connection con in _connections) LOG.Info(con.ToString);

            
            

            // Order domain CC to set a Connection
            // Send CC:ConnectionRequest(id, src, dst, sl)
            LOG.Info("Send CC:ConnectionRequest_req" +
                     $"(id = {connectionId}, src = {newConnection.SrcPortAlias}, dst = {newConnection.DstPortAlias}, sl = {newConnection.SlotsNumber})");
            RequestPacket ccConnectionRequestPacket = new RequestPacket.Builder()
                .SetId(newConnection.Id)
                .SetSrcPort(newConnection.SrcPortAlias)
                .SetDstPort(newConnection.DstPortAlias)
                .SetSlotsNumber(newConnection.SlotsNumber)
                .Build();
            ResponsePacket connectionRequestResponse = _ccConnectionRequestClient.Get(ccConnectionRequestPacket);
            LOG.Info($"Received CC:ConnectionRequest_res(res = {connectionRequestResponse.Res})");
            res = connectionRequestResponse.Res;
            (int, int) slots = connectionRequestResponse.Slots;
            // Check domain CC response
            switch (res)
            {
                case ResponseType.Ok:
                {
                    _ccConnectionRequests[newConnection.Id] = ccConnectionRequestPacket;
                    LOG.Info($"Send NPCC::CallRequest_res(res = OK, id = {newConnection.Id}, slots = {slots})");
                    return new Builder()
                        .SetRes(ResponseType.Ok)
                        .SetId(newConnection.Id)
                        .SetSlots(slots)
                        .Build();
                }
                case ResponseType.ResourcesProblem:
                {
                    LOG.Info("Send NPCC:CallRequest_res(res = ResourcesProblem)");
                    return new Builder()
                        .SetRes(ResponseType.ResourcesProblem)
                        .Build();
                }
                default:
                {
                    LOG.Info("Send NPCC::CallRequest_res(res = Network Problem)");
                    return new Builder()
                        .SetRes(ResponseType.NetworkProblem)
                        .Build();
                }
            }
        }

        public ResponsePacket OnCallCoordinationReceived(RequestPacket requestPacket)
        {
            // Get ConnectionRequest_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            string srcName = requestPacket.SrcName;
            string dstName = requestPacket.DstName;
            int slotsNumber = requestPacket.SlotsNumber;

            LOG.Info($"Received NPCC::CallCoordination_req" + $"(srcName = {srcName}, dstName = {dstName},slotsNumber = {slotsNumber})");
            
            // < C A L L   A D M I S S I O N   C O N T R O L >
            LOG.Info("Call Admission Control");
            // P O L I C Y
            // Randomize chance of rejecting ConnectionRequest_req by Policy component
            int chanceToRejectRequestInPolicy = _rnd.Next(0, 100);
            if (chanceToRejectRequestInPolicy > 5)
                LOG.Info("Call Request meets conditions of Policy component");
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

            // If Directory couldn't find dstPort send NCC::CallRequest_res(res=No client);
            if (dstPort == null)
            {
                LOG.Info($"Directory could not find port for user {dstName}");
                LOG.Info($"NPCC::CallRequest_res(res = {ResponseTypeToString(ResponseType.NoClient)})");
                return new Builder()
                    .SetRes(ResponseType.NoClient)
                    .Build();
            }
            
            IApiClient<RequestPacket, ResponsePacket> callAcceptClient = new ApiClient<RequestPacket, ResponsePacket>(_serverAddress, int.Parse(dstPort.ToString() + "9"));

            LOG.Info("Send CPCC::CallAccept_req" + $"(srcName = {srcName})");
            ResponsePacket callAcceptRes = callAcceptClient.Get(new RequestPacket.Builder()
                .SetSrcName(srcName)
                .Build()
            );
            LOG.Info($"Received CPCC:CallAccept_res(res = {callAcceptRes.Res})");
            if (callAcceptRes.Res == ResponseType.Refused) 
                return new Builder()
                    .SetRes(ResponseType.RefusedByCalledParty)
                    .Build();
            
            LOG.Info($"Call Admission Control ended succesfully");
            
            // </ C A L L   A D M I S S I O N   C O N T R O L >
            LOG.Info($"Send NPCC::CallCoordination_res(res = Ok)");
            // If Call passed CAC response with OK
            return new Builder()
                .SetRes(ResponseType.Ok)
                .Build();
        }

        public ResponsePacket OnCallTeardownReceived(RequestPacket requestPacket)
        {
            int id = requestPacket.Id;
            _ccConnectionRequests.Remove(id, out RequestPacket ccConnectionTeardownRequest);
            LOG.Info($"Received NPCC::CallTeardown_req(id = {id})");
            if (ccConnectionTeardownRequest == null)
            {
                LOG.Error($"Could not find a connection with id = {id}");
                LOG.Info($"Send NPCC::CallTeardown_res(res = Refused)");
                return new Builder()
                    .SetRes(ResponseType.Refused)
                    .Build();
            }

            ccConnectionTeardownRequest.Establish = RequestPacket.Est.Teardown;

            LOG.Info($"Send CC::ConnectionRequest(id = {ccConnectionTeardownRequest.Id}, src = {ccConnectionTeardownRequest.SrcPort}," +
                     $" dst = {ccConnectionTeardownRequest.DstPort}, sl = {ccConnectionTeardownRequest.SlotsNumber}, teardown = {ccConnectionTeardownRequest.Establish})");
            
            ResponsePacket connectionTeardownResponse = _ccConnectionRequestClient.Get(ccConnectionTeardownRequest);
            

            if (connectionTeardownResponse.Res != ResponseType.Ok)
            {
                LOG.Info($"Send NPCC::CallTeardown_res(res = NetworkProblem)");
                return new Builder().SetRes(ResponseType.NetworkProblem).Build();
            }
            LOG.Info("Received CC::ConnectionRequest(res = OK)");
            _connections.RemoveAll(connection => connection.Id == id);
            LOG.Info($"Send NPCC::CallTeardown_res(res = OK, id = {id})");
            return new Builder()
                .SetRes(ResponseType.Ok)
                .Build();
        }

        private string GetDomainFromPort(string portAlias)
        {
            foreach (KeyValuePair<string, string> portDomain in _portDomains.Where(portDomain =>
                Checkers.PortMatches(portDomain.Key, portAlias) > -1)) return portDomain.Value; // TODO: Check for matches value
            LOG.Error($"Empty domain from GetDomainFromPort() for portAlias: {portAlias}");
            return "";
        }
    }
}
