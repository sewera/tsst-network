using System;
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
            MplsPacket packet = new MplsPacket();
            IUserInterface ui = new UserInterface();

            ui.EnterDestAddressAndMessage();
        }
    }
}
