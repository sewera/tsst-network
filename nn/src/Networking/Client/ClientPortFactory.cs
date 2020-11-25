using nn.Config;
using nn.Models;

namespace nn.Networking.Client
{
    public class ClientPortFactory : IClientPortFactory
    {
        private readonly Configuration _configuration;

        public ClientPortFactory(Configuration configuration)
        {
            _configuration = configuration;
        }

        public IPort<MplsPacket> GetPort(string portAlias)
        {
            return new ClientPort(portAlias, _configuration);
        }
    }
}
