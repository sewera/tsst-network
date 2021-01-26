using Common.Models;

namespace Common.Api
{
    public interface IApiClientFactory<TRequestPacket, TResponsePacket>
            where TRequestPacket : ISerializablePacket
            where TResponsePacket : ISerializablePacket
    {
        ApiClient<TRequestPacket, TResponsePacket> GetApiClient();
    }
}
