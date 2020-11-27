using System.Net.Sockets;
using NLog;
namespace ms
{
    /// <summary>
    /// class for servicing specific client - network node
    /// <summary> 
    class Client
    {
        /// <summary>
        /// Client socket
        /// <summary>
        public Socket _socket { get; set; }
        /// <summary>
        /// Object for receiving packets from this client
        /// <summary>
        public SenderServer Sender { get; set; }
        /// <summary>
        /// Client identifier
        /// <summary>
        public string Alias { get; set; }

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Class contructor
        /// <param name="socket"> Socket pinned to specific client connection </param>
        /// <param name="id"> Client id </param>
        /// <summary>
        public Client(Socket socket, string alias)
        {
            // Create receive server for this client
            _socket = socket;
            Alias = alias;
        }

        /// <summary>
        /// Send data to specific client
        /// <param name="data"> Data to be sent </param>
        /// <summary>
         public void SendData(string data)
        {
            Sender = new SenderServer(_socket);
            Sender.Send(data);
            LOG.Info($"Data: '{data}' sent to NetworkNode: {Alias}");
        }
    }
}
