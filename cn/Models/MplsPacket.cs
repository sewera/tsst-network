using System.Collections.Generic;
using System.Net;

namespace cn.Models
{
    class MplsPacket
    {
        /// <summary>
        /// List of MPLS labels
        /// <summary>
        private IList<long> labels { get; set; }

        /// <summary>
        /// IP address of Client node sending message
        /// </summary>
        public IPAddress sourceAddress { get; set; }

        /// <summary>
        /// IP address of message receiver
        /// </summary>
        public IPAddress destinationAddress { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        public IPAddress message { get; set; }

        public MplsPacket()
        {

        }
    }
}

