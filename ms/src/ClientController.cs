using System.Net.Sockets;
using System.Collections.Generic;

namespace ms
{
    /// <summary>
    /// Class handling list with client connections
    /// </summary>
    static class ClientController
    {
        /// <summary>
        /// List with client connections
        /// </summary>
        public static List<Client> Clients = new List<Client>();

        /// <summary>
        /// Add new client connection to the list
        /// <param name="socket"> Socket associated with a specific client </param>
        /// </summary>
        public static void AddClient(Socket socket)
        {
            Clients.Add(new Client(socket, Clients.Count));
            SendData($"Server connected u as client: {Clients.Count-1}",Clients.Count-1);
        }
        /// <summary>
        /// Remove client connection from the list
        /// <param name="id"> Id of client to be deleted </param>
        /// </summary>
        public static void RemoveClient(int id)
        {
            Clients.RemoveAt(Clients.FindIndex(x => x.Id == id));
        }
        /// <summary>
        /// Send data to specific client
        /// <param name="data"> Data to be sent </param>
        /// <param name="id"> Id of the client </param>
        /// </summary>
        public static void SendData(string data, int id)
        {
            Clients[Clients.FindIndex(x => x.Id == id)].SendData(data);
        }
    }

}