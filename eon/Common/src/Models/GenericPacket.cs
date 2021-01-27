using MessagePack;

namespace Common.Models
{
    [MessagePackObject]
    public abstract class GenericPacket : ISerializablePacket
    {
        [Key(0)] public PacketType Type { get; }

        /// <summary>
        /// Constructor only for MessagePack deserialization
        /// </summary>
        /// To manually create an object of a child class, use a Builder
        public GenericPacket()
        {
        }

        protected GenericPacket(PacketType type)
        {
            Type = type;
        }

        public enum PacketType
        {
            Request,
            Response
        }

        public static string PacketTypeToString(PacketType type)
        {
            return type switch
            {
                PacketType.Request => "Request",
                PacketType.Response => "Response",
                _ => "Other"
            };
        }

        public virtual byte[] ToBytes()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
