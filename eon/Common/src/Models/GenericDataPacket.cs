using MessagePack;

namespace Common.Models
{
    /// <summary>
    /// A template, generic packet to present how protocol-specific packets
    /// should look like. If only those two fields (Type and Data) are needed,
    /// it can be also used in a protocol.
    /// </summary>
    [MessagePackObject]
    public class GenericDataPacket : GenericPacket
    {
        [Key(1)] public string Data { get; set; }

        public override string ToString()
        {
            return $"[{PacketTypeToString(Type)}, {Data}]";
        }

        private GenericDataPacket(PacketType type, string data) : base(type)
        {
            Data = data;
        }

        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create GenericDataPacket object, use <see cref="Builder"/>
        public GenericDataPacket()
        {
        }

        public class Builder
        {
            private PacketType _type = PacketType.Request;
            private string _data = string.Empty;

            public Builder SetType(PacketType type)
            {
                _type = type;
                return this;
            }

            public Builder SetData(string data)
            {
                _data = data;
                return this;
            }

            public GenericDataPacket Build()
            {
                return new GenericDataPacket(_type, _data);
            }
        }

        public override byte[] ToBytes()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
