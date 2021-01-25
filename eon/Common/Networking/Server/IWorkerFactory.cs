using System.Net.Sockets;
using Common.Models;

namespace Common.Networking.Server
{
    public interface IWorkerFactory<TPacket> where TPacket: ISerializablePacket
    {
        IWorker<TPacket> GetClientWorker(string portAlias, Socket socket);
        IWorker<TPacket> GetClientWorker(Socket socket);
    }
}
