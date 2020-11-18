using System;
using System.Net.Sockets;
using System.Text;
using NLog;

namespace ms
{
    /// <summary>
    /// Receiver Server for each client
    /// <summary>
    public class ReceiverServer
    {
        /// <summary>
        /// Buffer for received bytes
        /// <summary>
        private byte[] _buffer;
        /// <summary>
        /// Client socket
        /// <summary>
        private Socket _receiveSocket;
        /// <summary>
        /// Client Id
        /// <summary>     
        private int _clientId;
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        public ReceiverServer(Socket receiveSocket, int clientId)
        {
            _receiveSocket = receiveSocket;
            _clientId = clientId;
        }
       
        /// <summary>
        /// After this method is called the _receiveSocket is waiting for the incoming bytes
        /// <summary> 
        public void StartReceiving()
        {
            try
            {
                // The first 4 bytes we reserve for the lenght of the data the rest for the actual data.
                _buffer = new byte[4];
                // If any beats come, save them in _buffer and call Receive Callback method
                _receiveSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch { }
        }
        /// <summary>
        /// This method is called when _receiveSocket has received aby bytes, it receives the bytes and call StartReceiving() again
        /// <summary> 
        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                // If only 1 byte received, it means client wants to disconnect from the server
                //TODO fix this if statement
                if ( _receiveSocket.EndReceive(AR) > 1)
                {
                    // Convert first 4 bytes to int,in order to set _buffer size
                    _buffer = new byte[BitConverter.ToInt32(_buffer, 0)];
                   
                    // Keep receiving data
                    _receiveSocket.Receive(_buffer, _buffer.Length, SocketFlags.None);

                    //TODO show received data
                    string data = Encoding.Default.GetString(_buffer);
                    LOG.Info($"Data: '{data}' received from client: {_clientId}");
                    // Add alias for client
                    ClientController.AddAlias(data,_clientId);

                    // Now we have to start all over again with waiting for a data to come from the socket
                    StartReceiving();
                }
                else
                {
                    Disconnect();
                }
            }
            catch
            {
                // If exeption is throw check if socket is connected, because you can start receiving again. If not - Disconnect.
                if (!_receiveSocket.Connected)
                {
                    Disconnect();
                }
                else
                {
                    StartReceiving();
                }
            }
        }

        /// <summary>
        /// Disconnect _receiveSockets
        /// <summary> 
        private void Disconnect()
        {
            // Close connection
            _receiveSocket.Disconnect(true);
            // Remove client from the list
            ClientController.RemoveClient(_clientId);
        }
    }
}
