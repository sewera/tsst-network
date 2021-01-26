using MessagePack;

namespace Common.Models
{
    public interface ISerializablePacket
    {
        byte[] ToBytes();

        static TPacket FromBytes<TPacket>(byte[] bytes) where TPacket : ISerializablePacket
        {
            return MessagePackSerializer.Deserialize<TPacket>(bytes);
        }

        /// <summary>
        /// Method useful with the first (hello) packet, when the worker
        /// has to be registered in a HashMap, e.g. in MplsPacket it is SourcePortAlias
        /// </summary>
        /// <returns>Key for registering a worker</returns>
        string GetKey()
        {
            return "";
        }
    }
}
