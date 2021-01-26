using System.Collections.Generic;
using ClientNode.Config;
using Common.Models;
using Common.Networking.Client.Delegates;
using Common.Networking.Client.Persistent;
using NLog;

namespace ClientNode
{
    public class ClientNodeManager : IClientNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Configuration _configuration;
        private readonly IPersistentClientPort<MplsPacket> _clientPort;

        public ClientNodeManager(Configuration configuration, IPersistentClientPort<MplsPacket> clientPort)
        {
            _configuration = configuration;
            _clientPort = clientPort;
        }

        public void Start()
        {
            _clientPort.ConnectPermanentlyToServer(new MplsPacket.Builder().SetSourcePortAlias(_configuration.ClientPortAlias).Build());
            _clientPort.StartReceiving();
        }

        public void RegisterReceiveMessageEvent(ReceiveMessage<MplsPacket> receiveMessage)
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
