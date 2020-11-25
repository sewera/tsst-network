using System.Collections.Generic;
using cc.Config;
using cc.Networking.Client;
using cc.Networking.Forwarding;
using cc.Networking.Listeners;
using NLog;

namespace cc
{
    public class CableCloudManager : ICableCloudManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private Configuration _configuration;
        private IListener _listener;
        private IPacketForwarder _packetForwarder;
        private Dictionary<string, IClientWorker> _clientWorkers = new Dictionary<string, IClientWorker>();

        public CableCloudManager(Configuration configuration,
                           IListener listener,
                           IPacketForwarder packetForwarder)
        {
            _configuration = configuration;
            _listener = listener;
            _packetForwarder = packetForwarder;
        }

        public void Start()
        {
            LOG.Info("CableCloud started");
            _packetForwarder.SetClientPorts(_clientWorkers);
            _listener.RegisterWorkerConnectionEvent(RegisterWorker);
            _listener.Listen();
            while (true)
            {
                // wait
            }
        }

        private void RegisterWorker((string, IClientWorker) worker)
        {
            (string portAlias, IClientWorker clientWorker) = worker;
            _clientWorkers.Add(portAlias, clientWorker);
            clientWorker.RegisterReceiveMessageEvent(_packetForwarder.ForwardPacket);
        }
    }
}
