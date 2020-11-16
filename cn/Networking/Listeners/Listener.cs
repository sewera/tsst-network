using NLog;
using System;
using System.Net;
using System.Net.Sockets;
using cn.Utils;
using cn.Networking.Controllers;

namespace cn.Networking.Listeners
{
    public class Listener : IListener
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public void Listen()
        {
            try
            {
                ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, configuration.CnPort));
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
                LOG.Info($"AcceptCallback port: {configuration.CnPort}. Protocol type: {ProtocolType.Tcp}");
                var acceptedSocket = ListenerSocket.EndAccept(asyncResult);
                ClientController.AddClient(acceptedSocket);

                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception ex)
            {
                LOG.Error($"Error in AcceptCallback on port: {configuration.CnPort}, asyncResult: {asyncResult}");
                throw new SocketException((int)ListenerErrorCode.AcceptCallbackError);
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
