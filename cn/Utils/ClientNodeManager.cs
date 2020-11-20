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
        
        private IUserInterface _userInterface;
        private Configuration _configuration;

        /// <summary>
        ///  Number of sent packets, indicating their id
        /// </summary>
        private int _packetsSend = 0;

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

        private void ConnectToCableCloud()
        {
            try
            {
                LOG.Info($"Connecting to cable cloud at port: {_configuration.CableCloudPort}");
                Sender.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 
                    Int32.Parse(_configuration.CableCloudPort)));
                LOG.Info("Connection established");
                Send(); // TODO: send port alias as first message
            }//use msgpack 
            catch (Exception e)
            {
                LOG.Error("Failed to connect to cable cloud due to: " + e);
            }
        }

        private void SendPacket()
        {
            LOG.Info($"Preparing MPLS packet no {_packetsSend}");
            
            (string destinationPort, string message) = _userInterface.EnterReceiverAndMessage();
            Send(destinationPort, message);
  
            _packetsSend++;
            LOG.Info("Packet send");
        }

        private void Send(string destinationPort, string message)
        {
            MplsPacket packet = 
                new MplsPacket(_configuration.SourcePort, _configuration.CableCloudPort, destinationPort, message);
            Sender.Send(MplsPacket.ToBytes(packet));
            byte[] buffer = new byte[1024];
            Sender.Receive(buffer);
            LOG.Info($"Received: {Encoding.ASCII.GetString(buffer)}");
        }

        private void Send()
        {
            //Sender.Send(Encoding.ASCII.GetBytes($"Hello from port: {_configuration.SourcePort}"));
            MplsPacket packet = new MplsPacket(_configuration.SourcePort, _configuration.CableCloudPort);
            Sender.Send(MplsPacket.ToBytes(packet));
            byte[] buffer = new byte[1024];
            Sender.Receive(buffer);
            LOG.Info($"Received: {Encoding.ASCII.GetString(buffer)}");
        }
    }
}
