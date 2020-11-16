using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NLog;
using cn.Networking.Controllers;
using cn.Models;

namespace cn.Utils
{
    class ClientNodeManager : IClientNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public IUserInterface userInterface;
        public Configuration configuration;

        /// <summary>
        /// Socket listening for incoming connections
        /// </summary>
        public Socket ListenerSocket { get; set; }

        /// <summary>
        /// Socket sending messages to the server
        /// </summary>
        public Socket Sender { get; set; }

        public ClientNodeManager(Configuration config)
        {
            userInterface = new UserInterface();
            configuration = config;

            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToCableCloud()
        {
            try
            {
                //TODO: CC PORT MUST BE READ FROM CONFIG FILES
                LOG.Info($"Connecting to cable cloud at port: {configuration.CloudPort}");
                Sender.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7357));
                Sender.Send(Encoding.ASCII.GetBytes("Heeeeeeeeres Johny"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect to cable cloud due to: " + e);
            }
        }

        public void Listen(Socket listenerSocket)
        {
            try
            {
                Console.WriteLine($"Listening started on port: {configuration.CnPort}");
                ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, configuration.CnPort));
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
            (long destinationPort, string message) = userInterface.EnterReceiverAndMessage();
            Send(destinationPort, message);
        }

        public int Send(long destinationPort, string message)
        {
            MplsPacket packet = new MplsPacket(destinationPort, message);
            Sender.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7357));
            return Sender.Send(MplsPacket.ToBytes(packet));
        }


        /// <summary>
        /// This method is called when incoming connection needs to be serviced
        /// <summary> 
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine($"Accept Callback port:{configuration.CnPort}");
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
