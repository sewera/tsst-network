using System.Net.Sockets;
using Common.Models;

namespace Common.Networking.Server
{
    public class WorkerFactory<TPacket> : IWorkerFactory<TPacket> where TPacket : ISerializablePacket
    {
        public IWorker<TPacket> GetClientWorker(string portAlias, Socket socket)
        {
            return new Worker<TPacket>(portAlias, socket);
        }

        public IWorker<TPacket> GetClientWorker(Socket socket)
        {
            return new Worker<TPacket>("", socket);
        }
    }
}
