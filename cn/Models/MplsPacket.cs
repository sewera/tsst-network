using System.Collections.Generic;
using System.Net;
using MessagePack;

namespace cn.Models
{
    [MessagePackObject]
    public class MplsPacket
    {
        /// <summary>
        /// List of MPLS labels
        /// <summary>
        [Key(0)]
        private IList<long> Labels { get; set; }

        /// <summary>
        /// IP address (for sake of this project it's just localhost)
        /// </summary>
        [Key(1)]
        public string Address { get; set; }

        /// <summary>
        /// Out port of host client node
        /// </summary>
        [Key(2)]
        public long SourcePort { get; set; }

        /// <summary>
        /// Port of cable cloud 
        /// </summary>
        [Key(3)]
        public long CableCloudPort { get; set; }

        /// <summary>
        /// Port of remote detination client node
        /// </summary>
        [Key(4)]
        public long DestinationPort { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        [Key(5)]
        public string Message { get; set; }

        public MplsPacket(long destinationPort, string message)
        {
            Address = "127.0.0.1";
            SourcePort = 2137; //should be read from config file
            CableCloudPort = 7357; // should be read from config file
            DestinationPort = destinationPort;
            Message = message;
        }

        /// <summary>Converts <c>MPLSPacket</c> object to bytes</summary>
        /// <returns>MPLSPacket as bytes array</returns>
        public static byte[] ToBytes(MplsPacket packet)
        {
            return MessagePackSerializer.Serialize(packet);
        }

        /// <summary>Converts bytes to <c>MPLSPacket</c> object</summary>
        /// <returns>MPLSPacket object</returns>
        public static MplsPacket ToObject(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<MplsPacket>(bytes);
        }
    }
}

