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

        /// <summary>
        /// Alias representation of out port of host client node
        /// </summary>
        [Key(1)]
        private string SourcePortAlias { get; set; }

        /// <summary>
        /// Alias representation port of cable cloud 
        /// </summary>
        [Key(2)]
        private string CableCloudPortAlias { get; set; }

        /// <summary>
        /// Alias representation port of remote destination client node
        /// </summary>
        [Key(3)]
        private string DestinationPortAlias { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        [Key(4)]
        private string Message { get; set; }

        /// <summary>
        /// Constructor used to prepare MPLS packet with message and remote host receiver
        /// </summary>
        /// <param name="sourcePortAlias"></param>
        /// <param name="cableCloudPortAlias"></param>
        /// <param name="destinationPortAlias"></param>
        /// <param name="message"></param>
        /// <param name="packetId"></param>
        public MplsPacket(string sourcePortAlias, string cableCloudPortAlias, string destinationPortAlias, string message)
        {
            SourcePortAlias = sourcePortAlias; 
            CableCloudPortAlias = cableCloudPortAlias; 
            DestinationPortAlias = destinationPortAlias;
            Message = message;
        }
        
        /// <summary>
        /// Constructor used to prepare MPLS packet sent in initial message to Cloud Cable when connected
        /// </summary>
        /// <param name="sourcePortAlias"></param>
        /// <param name="cableCloudPortAlias"></param>
        public MplsPacket(string sourcePortAlias, string cableCloudPortAlias)
        {
            SourcePortAlias = sourcePortAlias; 
            CableCloudPortAlias = cableCloudPortAlias;
            DestinationPortAlias = "";
            Message = "";
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
        
        public override string ToString()
        {
            return $"[{SourcePortAlias}, {DestinationPortAlias}, [{string.Join(", ", Labels)}], {Message}]";
        }
    }
}

