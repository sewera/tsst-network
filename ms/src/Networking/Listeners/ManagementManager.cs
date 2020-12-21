using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ms.Config;
using NLog;

namespace ms
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
        public int port = 1234;
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

        public void startListening()
        {
            try
            {
                LOG.Info($"Listening started port: {port}");
                // Bind socket to the IPEndPoint, class representing a network endpoint as an IP address and a port number
                ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
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
                LOG.Info($"Accepted Callback port: {port}");
                // Create new socket for client
                Socket handler = ListenerSocket.EndAccept(ar);
                byte[]_buffer = new byte[2];
                handler.Receive(_buffer);
                string data = Encoding.Default.GetString(_buffer);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Data: '{data}'");
                Console.ResetColor();
                // Add new client to ClientController
                ClientController.AddClient(handler,data);

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
            port = configuration.Port;
        }
    }
}
