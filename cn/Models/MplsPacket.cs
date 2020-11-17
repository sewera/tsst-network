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
        public IList<long> Labels { get; set; }

        /// <summary> unnecessary
        /// IP address (for sake of this project it's just localhost)
        /// </summary>
        [Key(1)]
        public string Address { get; set; }

        /// <summary>
        /// Alias representation of out port of host client node
        /// </summary>
        [Key(2)]
        public string SourcePort { get; set; }

        /// <summary>
        /// Alias representation port of cable cloud 
        /// </summary>
        [Key(3)]
        public string CableCloudPort { get; set; }

        /// <summary>
        /// Alias representation port of remote detination client node
        /// </summary>
        [Key(4)]
        public string DestinationPort { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        [Key(5)]
        public string Message { get; set; }
        
        /// <summary>
        /// MPLS packet identifier
        /// </summary>
        [Key(6)]
        public int PacketId { get; set; }

        public MplsPacket(string destinationPort, string message, int packetId)
        {
            Address = "127.0.0.1";
            SourcePort = "2137"; //should be read from config file
            CableCloudPort = "7357"; // should be read from config file
            DestinationPort = destinationPort;
            Message = message;
            PacketId = packetId;
            //PacketId = packetId;
        }

        public MplsPacket()
        {
            
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

