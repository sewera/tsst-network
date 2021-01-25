using NetworkNode.Models;

namespace NetworkNode.Networking.Client
{
    public interface IClientPortFactory
    {
        IPort<MplsPacket> GetPort(string portAlias);
    }
}
