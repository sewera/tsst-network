using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NLog;
using cn.Models;

namespace cn.Utils
{
    class ClientNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        ///  Number of sent packets, indicating their id
        /// </summary>
        private int _packetsSend = 0;

        private IUserInterface _userInterface;
        private Configuration _configuration;

        /// <summary>
        /// Socket sending messages to the server
        /// </summary>
        private Socket Sender { get; set; }

        public ClientNodeManager(Configuration config, IUserInterface userInterface)
        {
            _userInterface = userInterface;
            _configuration = config;

            Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartClientNode()
        {
            ConnectToCableCloud();
            while (true)
            {
                SendPacket();
            }
        }

        public void ConnectToCableCloud()
        {
            try
            {
                //TODO: CC PORT MUST BE READ FROM CONFIG FILES
                LOG.Info($"Connecting to cable cloud at port: {_configuration.CloudPort}");
                Sender.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3001));
                LOG.Info("Connection established");
                Send("Hello from 1234"); // TODO: send port alias as first message
            }//use msgpack 
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect to cable cloud due to: " + e);
            }
        }

        public void SendPacket()
        {
            LOG.Info($"Preparing MPLS packet no {_packetsSend}");
            
            (string destinationPort, string message) = _userInterface.EnterReceiverAndMessage();
            Send(destinationPort, message, _packetsSend);
  
            _packetsSend++;
            LOG.Info("Packet send");
        }

        public void Send(string destinationPort, string message, int packetId)
        {
            MplsPacket packet = new MplsPacket(destinationPort, message, packetId);
            //TODO: CC port should be taken from config file
            Sender.Send(MplsPacket.ToBytes(packet));
            byte[] buffer = new byte[1024];
            Sender.Receive(buffer);
            LOG.Info($"Received: {Encoding.ASCII.GetString(buffer)}");
        }

        public void Send(string message) // TODO: Implement hello packet sending
        {
            Sender.Send(Encoding.ASCII.GetBytes(message));
            byte[] buffer = new byte[1024];
            Sender.Receive(buffer);
            LOG.Info($"Received: {Encoding.ASCII.GetString(buffer)}");
        }
    }
}
