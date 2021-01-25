using Common.Models;

namespace Common.Networking.Client.Delegates
{
    public delegate void ReceiveMessage<in TPacket>(TPacket packet) where TPacket: ISerializablePacket;
}
