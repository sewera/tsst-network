using System.Collections.Generic;
using MessagePack;

namespace CableCloud.Models
{
    [MessagePackObject]
    public class MplsPacket
    {
        [Key(0)]
        public string SourcePortAlias { get; set; }

        [Key(1)]
        public string DestinationPortAlias { get; set; }

        [Key(2)]
        public List<long> MplsLabels { get; set; }

        [Key(3)]
        public string Message { get; set; }

        public static byte[] ToBytes(MplsPacket mplsPacket)
        {
            return MessagePackSerializer.Serialize(mplsPacket);
        }

        public static MplsPacket FromBytes(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<MplsPacket>(bytes);
        }

        public override string ToString()
        {
            return $"[{SourcePortAlias}, {DestinationPortAlias}, [{string.Join(", ", MplsLabels)}], {Message}]";
        }
    }
}
