using NetworkNode.Models;

namespace NetworkNode.Networking.Delegates
{
    public delegate void ReceiveMessageDelegate<T>((string portAlias, T packet) receiveMessageTuple) where T : ISerializablePacket;
}
