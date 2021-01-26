using System.Net;
using Common.Models;

namespace Common.Api
{
    public class GenericApiClientFactory : ApiClientFactory<GenericPacket, GenericPacket>
    {
        public GenericApiClientFactory(IPAddress serverAddress, int serverPort) : base(serverAddress, serverPort)
        {
        }
    }
}
