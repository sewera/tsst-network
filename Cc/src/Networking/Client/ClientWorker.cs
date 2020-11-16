using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Cc.Networking.Client
{
    public class ClientWorker : IClientWorker
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private ClientState _state;

        public ClientWorker(ClientState state)
        {
            _state = state;
            _state.WorkSocket.BeginReceive(state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, state);
            LOG.Debug("Created ClientWorker for client socket: " +
                      $"{((IPEndPoint)_state.WorkSocket.RemoteEndPoint).Address}" +
                      $":{((IPEndPoint)_state.WorkSocket.RemoteEndPoint).Port}");
        }

        public void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            _state = (ClientState) ar.AsyncState;
            if (_state != null)
            {
                Socket handler = _state.WorkSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    _state.Sb.Append(Encoding.ASCII.GetString(_state.Buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.
                    string content = _state.Sb.ToString();
                    LOG.Trace("ReadCallback: All data transferred, got '\\n\\n'");
                    LOG.Debug($"Read {content.Length} bytes from socket.\nData: {content}");
                    // Echo the data back to the client.
                    Send(content);
                }
            }
            else
            {
                LOG.Fatal("_state.WorkSocket is null in ReadCallback");
            }
        }

        public void Send(string data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            _state.WorkSocket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, _state.WorkSocket);
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                if (handler != null)
                {
                    int bytesSent = handler.EndSend(ar);
                    LOG.Debug($"Sent {bytesSent} bytes to client");
                    handler.BeginReceive(_state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, _state);
                }
                else
                {
                    LOG.Fatal("handler is null in SendCallback");
                }
            }
            catch (Exception e)
            {
                LOG.Error(e);
            }
        }
    }
}
