using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace cn.Models
{
    class MplsPacket
    {
        /// <summary>
        /// List of MPLS labels
        /// <summary>
        private IList<long> Labels { get; set; }

        /// <summary>
        /// IP address (for sake of this project it's just localhost)
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// Out port of host client node
        /// </summary>
        public short SourcePort { get; set; }

        /// <summary>
        /// Port of remote detination client node
        /// </summary>
        public short CableCloudPort { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        public string Message { get; set; }

        public MplsPacket()
        {

        }

        /// <summary>Converts <c>MPLSPacket</c> to bytes</summary>
        /// <returns>MPLSPacket as bytes array</returns>
        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Labels.SelectMany(BitConverter.GetBytes).ToArray());
            result.AddRange(Address.GetAddressBytes());
            result.AddRange(BitConverter.GetBytes(SourcePort));
            result.AddRange(BitConverter.GetBytes(CableCloudPort));
            result.AddRange(Encoding.ASCII.GetBytes(Message ?? ""));

            return result.ToArray();
        }
    }
}

