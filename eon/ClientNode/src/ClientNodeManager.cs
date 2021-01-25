using System.Collections.Generic;
using ClientNetwork.Config;
using ClientNetwork.Models;
using ClientNetwork.Networking;
using ClientNetwork.Networking.Delegates;
using NLog;

namespace ClientNetwork
{
    public class ClientNodeManager : IClientNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private Configuration _configuration;
        private IClientPortFactory _clientPortFactory;
        private IClientPort _clientPort;

        public ClientNodeManager(Configuration config, IClientPortFactory clientPortFactory)
        {
            _configuration = config;
            _clientPortFactory = clientPortFactory;
        }

        public void Start()
        {
            _clientPort = _clientPortFactory.GetPort(_configuration.ClientPortAlias);
            _clientPort.ConnectToCableCloud();
            _clientPort.StartReceiving();
        }

        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage)
        {
            _clientPort.RegisterReceiveMessageEvent(receiveMessage);
        }

        public void Send(string mplsOutLabel, string message, (List<long>, string) labels)
        {
            MplsPacket packet = new MplsPacket.Builder()
                .SetSourcePortAlias(_configuration.ClientPortAlias)
                .SetDestinationPortAlias(labels.Item2)
                .SetMplsLabels(labels.Item1)
                .SetMessage(message)
                .Build();

            _clientPort.Send(packet);
        }
    }
}
