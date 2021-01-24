using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ms.Config;
using ms.Networking.Controllers;
using NLog;

namespace ms.Networking.Listeners
{
    /// <summary>
    /// ManagementManager implemented as server
    /// </summary>
    class ManagementManager : IManagementManager
    {
        /// <summary>
        /// This socket will listen for incoming connections
        /// </summary>
        public Socket ListenerSocket;

        /// <summary>
        /// Port on which socket will listen
        /// </summary>
        public int Port = 1234;

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Class contructor initializing ListenerSocket
        /// </summary>
        public ManagementManager()
        {
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartListening()
        {
            try
            {
                LOG.Info($"Listening started port: {Port}");
                // Bind socket to the IPEndPoint, class representing a network endpoint as an IP address and a port number
                ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                // 10 is then number of incoming connection that can be queued up for acceptance
                ListenerSocket.Listen(10);
                // The server will start listening for incoming connections and will go on with other logic. When there is an
                // connection the server switches back to this method and will run the AcceptCallBack method
                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception ex)
            {
                throw new Exception("Listening error" + ex);
            }
        }


        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                LOG.Info($"Accepted Callback port: {Port}");
                // Create new socket for client
                Socket handler = ListenerSocket.EndAccept(ar);
                byte[] buffer = new byte[2];
                handler.Receive(buffer);
                string data = Encoding.Default.GetString(buffer);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Data: '{data}'");
                Console.ResetColor();
                // Add new client to ClientController
                ClientController.AddClient(handler, data);

                // Start listening again
                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception)
            {
                //throw new Exception("Base Accept error" + ex);
                Console.WriteLine("Connection was forcibly closed by the remote host");
            }
        }

        public void ReadConfig(Configuration configuration)
        {
            Port = configuration.Port;
        }
    }
}
