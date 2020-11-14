using System.Net.Sockets;

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
        public ReceiverServer Receiver { get; set; }
        /// <summary>
        /// Client identifier
        /// <summary>
        public int Id { get; set; }
        /// <summary>
        /// <param name="socket"> Socket pinned to specific client connection </param>
        /// <param name="id"> Client id </param>
        /// <summary>
        public Client(Socket socket, int id)
        {
            // Create receive server for this client
            Receiver = new ReceiverServer(socket, id);
            Receiver.StartReceiving();
            _socket = socket;
            Id = id;
        }
    }
}
