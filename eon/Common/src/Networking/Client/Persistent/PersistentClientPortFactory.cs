using System.Net;
using Common.Models;

namespace Common.Networking.Client.Persistent
{
    public class PersistentClientPortFactory<TPacket> : IPersistentClientPortFactory<TPacket> where TPacket : ISerializablePacket
    {
        private readonly IPAddress _serverAddress;
        private readonly int _serverPort;

        public PersistentClientPortFactory(IPAddress serverAddress, int serverPort)
        {
            _serverAddress = serverAddress;
            _serverPort = serverPort;
        }

        public IPersistentClientPort<TPacket> GetPort(string clientPortAlias)
        {
            return new PersistentClientPort<TPacket>(clientPortAlias, _serverAddress, _serverPort);
        }
    }
}
