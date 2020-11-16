using System.Net.Sockets;

namespace Cc.Networking.Controllers
{
    public interface IClientController
    {
        void AddClient(Socket socket);
        void RemoveClient(int id);
    }
}
