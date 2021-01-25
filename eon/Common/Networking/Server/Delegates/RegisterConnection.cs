using Common.Models;

namespace Common.Networking.Server.Delegates
{
    public delegate void RegisterConnection<TPacket>((string, IWorker<TPacket>) keyAndWorker) where TPacket: ISerializablePacket;
}
