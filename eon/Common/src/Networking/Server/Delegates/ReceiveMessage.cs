using Common.Models;

namespace Common.Networking.Server.Delegates
{
    public delegate void ReceiveMessage<TPacket>((string, TPacket) receiveMessageTuple) where TPacket: ISerializablePacket;
}
