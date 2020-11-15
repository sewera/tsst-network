using System.Net.Sockets;
using Cc.Networking.Receivers;

namespace Cc.Networking.Clients
{
    public class Client
    {
        public Socket _socket { get; set; }
        public IDataReceiver Receive { get; set; }
        public int Id { get; set; }

        public Client(Socket socket, int id)
        {
            Receive = new RawDataReceiver(socket, id);
            Receive.StartReceiving();
            _socket = socket;
            Id = id;
        }
    }
}
