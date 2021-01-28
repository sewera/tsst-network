using System;
using System.Collections.Generic;
using System.Net;
using Common.Api;
using Common.Models;
using NLog;

namespace NetworkCallController
{
    public class ConnectionRequest
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Random _rnd = new Random();
        private readonly Dictionary<string, string> _clientPortAliases;
        
        private IApiClient<RequestPacket, ResponsePacket> _ccConnectionRequestClient;

        public ConnectionRequest(Dictionary<string, string> clientPortAliases, 
                                 IPAddress serverAddress, 
                                 int ccConnectionRequestRemotePort)
        {
            _clientPortAliases = clientPortAliases;
            _ccConnectionRequestClient = 
                new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccConnectionRequestRemotePort);
        }
        
        public ResponsePacket OnConnectionRequestReceived(RequestPacket requestPacket)
        {
            // Get ConnectionRequest_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            string srcName = requestPacket.SrcName;
            string dstName = requestPacket.DstName;
            int slotsNumber = requestPacket.SlotsNumber;
            RequestPacket.Who whoRequests = requestPacket.WhoRequests;
            
            LOG.Info($"Received NCC::ConnectionRequest_{GenericPacket.PacketTypeToString(type)} from {srcName} " +
                     $" for {slotsNumber} slots");

            // Randomize chance of rejecting ConnectionRequest_req by Policy component
            int chanceToRejectRequestInPolicy = _rnd.Next(0, 100);
            if (chanceToRejectRequestInPolicy > 5)
                LOG.Info($"ConnectionRequest meets conditions of Policy component");
            else
                return new ResponsePacket.Builder()
                    .SetRes(ResponsePacket.ResponseType.AuthProblem)
                    .Build();
            
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

            if (srcPort == null)
            {
                LOG.Info($"Directory could not find port for user {srcName}");
                return new ResponsePacket.Builder()
                    .SetRes(ResponsePacket.ResponseType.NoClient)
                    .Build();
            }

            // If dstPort is from the same domain we send ConnectionRequest to domain CC
            if (dstPort != null)
            {
                LOG.Info($"Directory found in-domain connection between ports {srcName}::{srcPort} and " +
                         $"{dstName}::{dstPort}.");
                LOG.Info($"Sending ConnectionRequest_req to domain CC for {slotsNumber} slots");
                
                ResponsePacket connectionRequestResponse = _ccConnectionRequestClient.Get(new RequestPacket.Builder()
                    .SetId(1) // TODO: Jak ustawiamy ID i gdzie, czy tutaj jest początek??????
                    .SetSrcPort(srcPort)
                    .SetDstPort(dstPort)
                    .SetSlotsNumber(slotsNumber)
                    .Build());

                // Get ConnectionRequest_res packet params // TODO: PO CO NAM ONE 
                ResponsePacket.ResponseType res = connectionRequestResponse.Res;
                string nextZonePort = connectionRequestResponse.NextZonePort;
                (int, int) slots = connectionRequestResponse.Slots;
                
                // Send ConnectionRequest_res to CPCC with adequate status
                if (res == ResponsePacket.ResponseType.Ok)
                    return new ResponsePacket.Builder()
                        .SetRes(ResponsePacket.ResponseType.Ok)
                        .Build();
                if (res == ResponsePacket.ResponseType.Refused)
                    return new ResponsePacket.Builder()
                        .SetRes(ResponsePacket.ResponseType.NetworkProblem)
                        .Build();
            }

            return null;
        }
    }
}
