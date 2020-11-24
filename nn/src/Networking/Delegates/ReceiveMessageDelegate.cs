using nn.Models;

namespace nn.Networking.Delegates
{
    public delegate void ReceiveMessageDelegate<T>(T packet) where T : ISerializablePacket;
}
