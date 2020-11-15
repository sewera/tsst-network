using System.Net;
using cn.Models;
using cn.Utils;

namespace cn
{
    class ClientNode
    {
        /// <summary>
        /// IP address of message receiver
        /// </summary>
        private IPAddress destinationAddress { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        private string message { get; set; }

        static void Main(string[] args)
        {
            IConfiguration config = new Configuration();
            IUserInterface ui = new UserInterface();
            IClientNodeManager cnManager = new ClientNodeManager(2137);
            MplsPacket packet = new MplsPacket();


            ui.EnterDestAddressAndMessage();
        }
    }
}
