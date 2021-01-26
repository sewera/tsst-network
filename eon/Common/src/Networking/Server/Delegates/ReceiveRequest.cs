using Common.Models;

namespace Common.Networking.Server.Delegates
{
    public delegate TResponsePacket ReceiveRequest<in TRequestPacket, out TResponsePacket>(TRequestPacket requestPacket)
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket;
}
