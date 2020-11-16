using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NLog;

namespace Cc.Networking.Client
{
    public class ClientWorker
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private ClientState _state;

        public ClientWorker(ClientState state)
        {
            _state = state;
            _state.WorkSocket.BeginReceive(state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, state);
            LOG.Debug("Created ClientWorker for socket: " +
                      $"{IPAddress.Parse(((IPEndPoint)_state.WorkSocket.RemoteEndPoint).Address.ToString())}" +
                      $":{((IPEndPoint)_state.WorkSocket.RemoteEndPoint).Port}");
        }

        public void ReadCallback(IAsyncResult ar)
        {
            LOG.Trace("ReadCallback");
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            _state = (ClientState) ar.AsyncState;
            Socket handler = _state.WorkSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0) {
                // There  might be more data, so store the data received so far.
                _state.Sb.Append(Encoding.ASCII.GetString(
                    _state.Buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.
                content = _state.Sb.ToString();
                if (content.IndexOf("\n") > -1) {
                    // All the data has been read from the
                    // client. Display it on the console.
                    LOG.Debug($"Read {content.Length} bytes from socket.\nData: {content}");
                    // Echo the data back to the client.
                    Send(handler, content);
                } else {
                    // Not all data received. Get more.
                    handler.BeginReceive(_state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, _state);
                }
            }
        }

        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                LOG.Debug($"Sent {bytesSent} bytes to client");

                handler.BeginReceive(_state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, _state);

                // handler.Shutdown(SocketShutdown.Both);
                // handler.Close();
            }
            catch (Exception e)
            {
                LOG.Error(e);
            }
        }
    }
}
