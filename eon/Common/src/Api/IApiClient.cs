using Common.Models;

namespace Common.Api
{
    public interface IApiClient<in TRequestPacket, out TResponsePacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        TResponsePacket Get(TRequestPacket requestPacket);
    }
}
