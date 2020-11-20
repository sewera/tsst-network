using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using cc.Models;
using NLog;

namespace cc.Networking.Client
{
    public class ClientWorker : IClientWorker
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private ClientState _state;

        public ClientWorker(ClientState state)
        {
            _state = state;
            // FIXME: Catch connection reset exception to avoid program crashes after client crashes
            _state.ClientSocket.BeginReceive(state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, state);
            LOG.Debug("Created ClientWorker for client socket: " +
                      $"{((IPEndPoint)_state.ClientSocket.RemoteEndPoint).Address}" +
                      $":{((IPEndPoint)_state.ClientSocket.RemoteEndPoint).Port}");
        }

        public void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            _state = (ClientState) ar.AsyncState;
            if (_state != null)
            {
                Socket handler = _state.ClientSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    _state.Packet = MplsPacket.FromBytes(_state.Buffer);
                    LOG.Debug($"Received: {_state.Packet}");

                    // Echo the data back to the client.
                    // Send(_state.Packet);

                    handler.BeginReceive(_state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, _state);
                }
            }
            else
            {
                LOG.Fatal("_state.WorkSocket is null in ReadCallback");
            }
        }

        public void Send(MplsPacket mplsPacket)
        {
            byte[] bytes = MplsPacket.ToBytes(mplsPacket);
            _state.ClientSocket.BeginSend(bytes, 0, bytes.Length, 0, SendCallback, _state.ClientSocket);
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

        public int GetPort()
        {
            return ((IPEndPoint) _state.ClientSocket.RemoteEndPoint).Port;
        }
    }
}
