using Common.Models;

namespace Common.Networking.Client.Delegates
{
    public delegate void ReceiveMessage<TPacket>((string portAlias, TPacket packet) portAliasAndPacketTuple) where TPacket: ISerializablePacket;
}
