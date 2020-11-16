using System.Net.Sockets;
using Cc.Networking.Receivers;

namespace Cc.Networking.Clients
{
    public class Client
    {
        private Socket _socket;
        private readonly IDataReceiver _dataReceiver;
        public int Id { get; set; }

        public Client(Socket socket, int id, IDataReceiver dataReceiver)
        {
            _dataReceiver = dataReceiver;
            _dataReceiver.StartReceiving();
            _socket = socket;
            Id = id;
        }
    }
}
