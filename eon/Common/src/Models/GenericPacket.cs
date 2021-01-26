using MessagePack;

namespace Common.Models
{
    /// <summary>
    /// A template, generic packet to present how protocol-specific packets
    /// should look like. If only those two fields (Type and Data) are needed,
    /// it can be also used in a protocol.
    /// </summary>
    [MessagePackObject]
    public class GenericPacket : ISerializablePacket
    {
        [Key(0)] public PacketType Type { get; set; }

        [Key(1)] public string Data { get; set; }

        public byte[] ToBytes()
        {
            return MessagePackSerializer.Serialize(this);
        }

        public static GenericPacket FromBytes(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<GenericPacket>(bytes);
        }

        public override string ToString()
        {
            string pt;
            switch (Type)
            {
                case PacketType.Request:
                    pt = "Request";
                    break;
                case PacketType.Response:
                    pt = "Response";
                    break;
                default:
                    pt = "Other";
                    break;
            }
            return $"[{pt}, {Data}]";
        }

        private GenericPacket(PacketType type, string data)
        {
            Type = type;
            Data = data;
        }

        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create GenericPacket object, use <see cref="Builder"/>
        public GenericPacket()
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

            public GenericPacket Build()
            {
                return new GenericPacket(_type, _data);
            }
        }

        public enum PacketType
        {
            Request,
            Response
        }
    }
}
