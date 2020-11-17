using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NLog;
using cn.Models;

namespace cn.Utils
{
    class ClientNodeManager : IClientNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        ///  Number of sent packets, indicating their id
        /// </summary>
        private int _packetsSend = 0;

        private IUserInterface userInterface;
        private Configuration configuration;

        /// <summary>
        /// Socket sending messages to the server
        /// </summary>
        private Socket Sender { get; set; }
        
        public ClientNodeManager(Configuration config)
        {
            userInterface = new UserInterface();
            configuration = config;

            Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToCableCloud()
        {
            try
            {
                //TODO: CC PORT MUST BE READ FROM CONFIG FILES
                LOG.Info($"Connecting to cable cloud at port: {configuration.CloudPort}");
                Sender.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7357));
                LOG.Info("Connection established");
                Sender.Send(Encoding.ASCII.GetBytes("Heeeeeeeeres Johny"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect to cable cloud due to: " + e);
            }
        }

        public void SendPacket()
        {
            LOG.Info($"Preparing MPLS packet no {_packetsSend}");
            
            (var destinationPort, var message) = userInterface.EnterReceiverAndMessage();
            Send(destinationPort, message, _packetsSend);
            _packetsSend++;
            LOG.Info("Packet send");
        }

        public int Send(long destinationPort, string message, int packetId)
        {
            MplsPacket packet = new MplsPacket(destinationPort, message, packetId);
            Sender.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7357));
            return Sender.Send(MplsPacket.ToBytes(packet));
        }
    }
}
