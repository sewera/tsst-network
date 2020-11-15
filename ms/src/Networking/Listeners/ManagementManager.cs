using System;
using System.Net;
using System.Net.Sockets;

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
        public short port = 1234;

        /// <summary>
        /// Class contructor initializing ListenerSocket
        /// </summary>
        public ManagementManager()
        {
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// This method starts the process of listening for incoming connections from clients - network nodes
        /// </summary>
        public void startListening()
        {
            try
            {
                UserInterface.WriteLine($"Listening started port: {port}", UserInterface.Type.Server);
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

        /// <summary>
        /// This method is called when new connection popped up and needs to be serviced
        /// <summary> 
        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                UserInterface.WriteLine($"Accept Callback port:{port}",UserInterface.Type.Server);
                // Create new socket for client
                Socket acceptedSocket = ListenerSocket.EndAccept(ar);
                // Add new client to ClientController
                ClientController.AddClient(acceptedSocket);

                // Start listening again
                ListenerSocket.BeginAccept(AcceptCallback, ListenerSocket);
            }
            catch (Exception ex)
            {
                throw new Exception("Base Accept error" + ex);
            }
        }
    }
}