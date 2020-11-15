using System;
using System.Net;
using System.Net.Sockets;
using cn.Networking.Controllers;

namespace cn.Utils
{
    class ClientNodeManager : IClientNodeManager
    {
        /// <summary>
        /// Socket listening for incoming connections
        /// </summary>
        public Socket ListenerSocket { get; set; }

        /// <summary>
        /// Port client node socket is listening on
        /// </summary>
        public short Port { get; set; }

        public ClientNodeManager(short port)
        {
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Port = port;
        }

        public void ConnectToCableCloud()
        {

        }

        public void Listen(Socket listenerSocket)
        {
            try
            {
                Console.WriteLine($"Listening started on port: {Port}");
                ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                ListenerSocket.Listen(10);
                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception ex)
            {
                throw new Exception("Listening error" + ex);
            }
        }

        public void SendPacket()
        {

        }

        /// <summary>
        /// This method is called when incoming connection needs to be serviced
        /// <summary> 
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine($"Accept Callback port:{Port}");
                Socket acceptedSocket = ListenerSocket.EndAccept(ar);
                ClientController.AddClient(acceptedSocket);
                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception ex)
            {
                throw new Exception("Base Accept error" + ex);
            }
        }
    }
}
