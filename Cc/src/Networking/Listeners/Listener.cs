using NLog;
using System;
using System.Net;
using System.Net.Sockets;
using Cc.Config;
using Cc.Networking.Controllers;

namespace Cc.Networking.Listeners
{
    public class Listener : IListener
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        public void Listen()
        {
            try
            {
                ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, configuration.listeningPort));
                ListenerSocket.Listen(10);
                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception ex)
            {
                throw new Exception("listening error" + ex);
            }
        }

        public void AcceptCallback(IAsyncResult asyncResult)
        {
            try
            {
                LOG.Info($"AcceptCallback port: {configuration.listeningPort}. Protocol type: {ProtocolType.Tcp}");
                var acceptedSocket = ListenerSocket.EndAccept(asyncResult);
                ClientController.AddClient(acceptedSocket);

                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception ex)
            {
                LOG.Error($"Error in AcceptCallback on port: {configuration.listeningPort}, asyncResult: {asyncResult}");
                throw new SocketException((int) ListenerErrorCode.AcceptCallbackError);
            }
        }

        public Socket ListenerSocket;

        private Configuration configuration;

        public Listener(Configuration configuration)
        {
            this.configuration = configuration;
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        } 
    }
}
