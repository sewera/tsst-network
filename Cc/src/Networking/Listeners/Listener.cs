using System;
using System.Net;
using System.Net.Sockets;
using Cc.Config;
using Cc.Networking.Controllers;
using NLog;

namespace Cc.Networking.Listeners
{
    public class Listener : IListener
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly IClientController _clientController;
        private readonly Configuration _configuration;
        public Socket ListenerSocket;

        public Listener(IClientController clientController, Configuration configuration)
        {
            _clientController = clientController;
            _configuration = configuration;
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        
        public void Listen()
        {
            LOG.Trace("Listen");
            try
            {
                ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, _configuration.ListeningPort));
                LOG.Debug($"Successfully bound port {_configuration.ListeningPort}");
                ListenerSocket.Listen(10);
                LOG.Debug("Begin accepting client connections");
                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception e)
            {
                LOG.Error($"Error in Listen. Possibly could not bind port {_configuration.ListeningPort}");
                LOG.Error(e);
                throw new SocketException((int) ListenerErrorCode.ListenError);
            }
        }

        public void AcceptCallback(IAsyncResult asyncResult)
        {
            LOG.Trace("AcceptCallback");
            try
            {
                LOG.Info($"AcceptCallback port: {_configuration.ListeningPort}. Protocol type: {ProtocolType.Tcp}");
                var acceptedSocket = ListenerSocket.EndAccept(asyncResult);
                _clientController.AddClient(acceptedSocket);

                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception e)
            {
                LOG.Error($"Error in AcceptCallback on port: {_configuration.ListeningPort}, asyncResult: {asyncResult}");
                LOG.Error(e);
                throw new SocketException((int) ListenerErrorCode.AcceptCallbackError);
            }
        }
    }
}
