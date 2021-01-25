using ClientNetwork.Config;

namespace ClientNetwork.Networking
{
    public class ClientPortFactory : IClientPortFactory
    {
        private readonly Configuration _configuration;

        public ClientPortFactory(Configuration configuration)
        {
            _configuration = configuration;
        }

        public IClientPort GetPort(string portAlias)
        {
            return new ClientPort(portAlias, _configuration);
        }
    }
}
