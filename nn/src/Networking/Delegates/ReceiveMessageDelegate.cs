using nn.Models;

namespace nn.Networking.Delegates
{
    public delegate void ReceiveMessageDelegate<T>((string portAlias, T packet) receiveMessageTuple) where T : ISerializablePacket;
}
