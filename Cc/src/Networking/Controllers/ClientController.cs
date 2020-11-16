using System.Collections.Generic;
using System.Net.Sockets;
using Cc.Networking.Clients;
using Cc.Networking.Receivers;

namespace Cc.Networking.Controllers
{
    internal class ClientController : IClientController
    {
        private readonly IDataReceiverFactory _dataReceiverFactory;
        private readonly List<Client> _clients;
        
        public ClientController(IDataReceiverFactory dataReceiverFactory)
        {
            _dataReceiverFactory = dataReceiverFactory;
            _clients = new List<Client>();
        }
        
        public void AddClient(Socket socket)
        {
            _clients.Add(new Client(socket, _clients.Count, _dataReceiverFactory.GetDataReceiver(socket)));
        }

        public void RemoveClient(int id)
        {
            _clients.RemoveAt(_clients.FindIndex(x => x.Id == id));
        }
    }
}
