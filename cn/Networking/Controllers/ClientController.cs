using System.Collections.Generic;
using System.Net.Sockets;
using cn.Networking.Clients;

namespace cn.Networking.Controllers
{
     static class ClientController
     {
          public static List<Client> Clients = new List<Client>();

          public static void AddClient(Socket socket)
          {
              Clients.Add(new Client(socket,Clients.Count));
          }

          public static void RemoveClient(int id)
          {
              Clients.RemoveAt(Clients.FindIndex(x => x.Id == id));
          }
      }
}