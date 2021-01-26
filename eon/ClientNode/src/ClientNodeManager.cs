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
        private readonly IPersistentClientPort<MplsPacket> _clientPort;
        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _callAcceptPort;

        public ClientNodeManager(Configuration configuration,
                                 IPersistentClientPort<MplsPacket> clientPort,
                                 ReceiveRequest<GenericPacket, GenericPacket> callAcceptDelegate)
        {
            _configuration = configuration;
            _clientPort = clientPort;
            _callAcceptPort = new OneShotServerPort<GenericPacket, GenericPacket>(
                configuration.CallingPartyCallControllerAddress,
                configuration.CallingPartyCallControllerPort);
            _callAcceptPort.RegisterReceiveRequestDelegate(callAcceptDelegate);
        }

        public void Start()
        {
            // TODO: Register delegate when callAccept arrives
            _callAcceptPort.Listen();
            _clientPort.ConnectPermanentlyToServer(new MplsPacket.Builder().SetSourcePortAlias(_configuration.ClientPortAlias).Build());
            _clientPort.StartReceiving();
        }

        public void RegisterReceiveMessageEvent(Common.Networking.Client.Delegates.ReceiveMessage<MplsPacket> receiveMessage)
        {
            _clientPort.RegisterReceiveMessageEvent(receiveMessage);
        }

        public void Send(string mplsOutLabel, string message, (List<long>, string) labels)
        {
            (List<long> mplsLabels, string remoteHostAlias) = labels;
            MplsPacket packet = new MplsPacket.Builder()
                .SetSourcePortAlias(_configuration.ClientPortAlias)
                .SetDestinationPortAlias(remoteHostAlias)
                .SetMplsLabels(mplsLabels)
                .SetMessage(message)
                .Build();

            _clientPort.Send(packet);
        }
    }
}
