using System.Net;
using Common.Models;

namespace Common.Api
{
    public class ApiClientFactory<TRequestPacket, TResponsePacket> : IApiClientFactory<TRequestPacket, TResponsePacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        private readonly IPAddress _serverAddress;
        private readonly int _serverPort;

        public ApiClientFactory(IPAddress serverAddress, int serverPort)
        {
            _serverAddress = serverAddress;
            _serverPort = serverPort;
        }

        public ApiClient<TRequestPacket, TResponsePacket> GetApiClient()
        {
            return new ApiClient<TRequestPacket, TResponsePacket>(_serverAddress, _serverPort);
        }
    }
}
