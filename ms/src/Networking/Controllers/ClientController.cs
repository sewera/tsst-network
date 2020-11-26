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
        /// Id of next client connected to server
        /// </summary>
        private static int counter=0;

        /// <summary>
        /// Add new client connection to the list
        /// <param name="socket"> Socket associated with a specific client </param>
        /// </summary>
        public static void AddClient(Socket socket)
        {
            Clients.Add(new Client(socket, counter++));
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
        /// <param name="clientAlias"> Alias of the client </param>
        /// <returns> True if the data is sent, False if not <returns>
        /// </summary>
        public static bool SendData(string data, string clientAlias)
        {
            try
            {
                Clients[Clients.FindIndex(x => x.Alias == clientAlias)].SendData(data);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Send data to specific client
        /// <param name="data"> Data to be sent </param>
        /// <param name="id"> Id of the client </param>
        /// </summary>
        private static void SendData(string data, int id)
        {
            Clients[Clients.FindIndex(x => x.Id == id)].SendData(data);
        }
        /// <summary>
        /// Add alias for specific client
        /// <param name="alias"> String meant to be alias </param>
        /// <param name="id"> Id of the client </param>
        /// </summary>
        public static void AddAlias(string alias, int id)
        {
            if (!(CheckAlias(alias)))
            {
                UserInterface.WriteLine($"Can't add alias to client\n'{alias}' is not an alias",UserInterface.Type.Syntax);
            }
            else
            {
               Clients[Clients.FindIndex(x => x.Id == id)].Alias=alias;
               UserInterface.WriteLine($"'{alias}' added successfully for client: {id} ",UserInterface.Type.Received);
            }
        }
        /// <summary>
        /// Check if string match network node alias format
        /// <param name="alias"> String meant to be alias </param>
        /// </summary>
        private static bool CheckAlias(string alias)
        {
            // Alias format is a string begginng with 'R' and end ending with a digits chain e.g. "R12"
            // Check if first char is 'R'
            if (alias[0] != 'R')
            {
                return false;
            }

            // Alias must containt at least 2 chars
            if (alias.Length < 2)
            {
                return false;
            }

            // Check if the rest of the string are digits.
            for (int i=1;i<alias.Length;i++)
            {
                if (!(char.IsDigit(alias[i])))
                {
                    return false;
                }
            }
            
            return true;
        }
        /// <summary>
        /// Check if client with matching alias exists
        /// <param name="alias"> Alias </param>
        /// <returns> True if yes, False otherwise </returns>
        /// </summary>
        public static bool FindAlias(string alias)
        {
               
            if(Clients.FindIndex(x => x.Alias == alias) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
