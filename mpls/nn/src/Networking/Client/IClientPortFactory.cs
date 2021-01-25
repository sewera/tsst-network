using nn.Models;

namespace nn.Networking.Client
{
    public interface IClientPortFactory
    {
        IPort<MplsPacket> GetPort(string portAlias);
    }
}
