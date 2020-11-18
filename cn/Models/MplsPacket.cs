using System.Collections.Generic;
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

        /// <summary> unnecessary
        /// IP address (for sake of this project it's just localhost)
        /// </summary>
        [Key(1)]
        private string Address { get; set; }

        /// <summary>
        /// Alias representation of out port of host client node
        /// </summary>
        [Key(2)]
        private string SourcePortAlias { get; set; }

        /// <summary>
        /// Alias representation port of cable cloud 
        /// </summary>
        [Key(3)]
        private string CableCloudPortAlias { get; set; }

        /// <summary>
        /// Alias representation port of remote destination client node
        /// </summary>
        [Key(4)]
        private string DestinationPort { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        [Key(5)]
        private string Message { get; set; }
        
        /// <summary>
        /// MPLS packet identifier
        /// </summary>
        [Key(6)]
        private int PacketId { get; set; }

        public MplsPacket(string sourcePortAlias, string cableCloudPortAlias, string destinationPort, string message, int packetId)
        {
            Address = "127.0.0.1";
            SourcePortAlias = sourcePortAlias; 
            CableCloudPortAlias = cableCloudPortAlias; 
            DestinationPort = destinationPort;
            Message = message;
            PacketId = packetId;
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

