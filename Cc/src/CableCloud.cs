using Cc.Config;
using Cc.Config.Parsers;
using Cc.Networking.Controllers;
using Cc.Networking.Forwarders;
using Cc.Networking.Listeners;
using Cc.Networking.Receivers;
using NLog;

namespace Cc
{
    internal class CableCloud
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private Configuration _configuration;
        private IListener _listener;
        private IPacketForwarder _packetForwarder;
        private IClientController _clientController;
        private IDataReceiverFactory _dataReceiverFactory;

        private CableCloud(Configuration configuration,
                           IListener listener,
                           IPacketForwarder packetForwarder,
                           IClientController clientController,
                           IDataReceiverFactory dataReceiverFactory)
        {
            _configuration = configuration;
            _listener = listener;
            _packetForwarder = packetForwarder;
            _clientController = clientController;
            _dataReceiverFactory = dataReceiverFactory;
        }

        public void Start()
        {
            LOG.Info("Started listening");
            _listener.Listen();
            while (true)
            {
                //wait
            }
        }

        public class Builder
        {
            private Configuration _configuration;
            private IListener _listener;
            private IPacketForwarder _packetForwarder;
            private IClientController _clientController;
            private IDataReceiverFactory _dataReceiverFactory;

            public Builder SetConfiguration(Configuration configuration)
            {
                _configuration = configuration;
                return this;
            }

            public Builder SetListener(IListener listener)
            {
                _listener = listener;
                return this;
            }

            public Builder SetPacketForwarder(IPacketForwarder packetForwarder)
            {
                _packetForwarder = packetForwarder;
                return this;
            }

            public Builder SetClientController(IClientController clientController)
            {
                _clientController = clientController;
                return this;
            }

            public Builder SetDataReceiverFactory(IDataReceiverFactory dataReceiverFactory)
            {
                _dataReceiverFactory = dataReceiverFactory;
                return this;
            }

            public CableCloud Build()
            {
                return new CableCloud(_configuration,
                    _listener,
                    _packetForwarder,
                    _clientController,
                    _dataReceiverFactory);
            }
        }
    }
}
