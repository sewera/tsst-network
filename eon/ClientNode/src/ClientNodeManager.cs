using System.Collections.Generic;
using ClientNode.Config;
using Common.Models;
using Common.Networking.Client.Persistent;
using Common.Networking.Server.Delegates;
using Common.Networking.Server.OneShot;
using NLog;

namespace ClientNode
{
    public class ClientNodeManager : IClientNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Configuration _configuration;
        private readonly IPersistentClientPort<EonPacket> _clientPort;
        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _callAcceptPort;

        public ClientNodeManager(Configuration configuration,
                                 IPersistentClientPort<EonPacket> clientPort,
                                 ReceiveRequest<RequestPacket, ResponsePacket> callAcceptDelegate)
        {
            _configuration = configuration;
            _clientPort = clientPort;
            _callAcceptPort = new OneShotServerPort<RequestPacket, ResponsePacket>(
                configuration.CallingPartyCallControllerAddress,
                configuration.CallingPartyCallControllerPort);
            _callAcceptPort.RegisterReceiveRequestDelegate(callAcceptDelegate);
        }

        public void Start()
        {
            // TODO: Register delegate when callAccept arrives
            _callAcceptPort.Listen();
            _clientPort.ConnectPermanentlyToServer(new EonPacket.Builder().SetSrcPort(_configuration.ClientPortAlias).Build());
            _clientPort.StartReceiving();
        }

        public void RegisterReceiveMessageEvent(Common.Networking.Client.Delegates.ReceiveMessage<EonPacket> receiveMessage)
        {
            _clientPort.RegisterReceiveMessageEvent(receiveMessage);
        }

        public void Send(string message, string connection, (int, int) slots)
        {
            LOG.Info($"Sending message '{message}' through connection {connection} on port {_configuration.ClientPortAlias}");

            EonPacket packet = new EonPacket.Builder()
                .SetSrcPort(_configuration.ClientPortAlias)
                .SetSlots(slots)
                .SetMessage(message)
                .Build();

            _clientPort.Send(packet);
        }
    }
}
