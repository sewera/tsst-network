using System;
using System.Net;
using System.Net.Sockets;
using CableCloud.Models;
using CableCloud.Networking.Delegates;
using NLog;

namespace CableCloud.Networking.Client
{
    public class ClientWorker : IClientWorker
    {
        public event ClientRemovedEventHandler ClientRemoved;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private ClientState _state;

        public event ReceiveMessageDelegate MessageReceived;

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
            try
            {
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

                        OnMessageReceived(_state.Packet);

                        handler.BeginReceive(_state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, _state);
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                else
                {
                    LOG.Fatal("_state.WorkSocket is null in ReadCallback");
                }
            }
            catch
            {
                // If exeption is throw check if socket is connected, because you can start receiving again. If not - Disconnect.
                if (!_state.ClientSocket.Connected)
                {
                    Disconnect();
                }
                else
                {
                    // HERE
                    _state.ClientSocket.BeginReceive(_state.Buffer, 0, ClientState.BufferSize, 0, ReadCallback, _state);
                }
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

        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate receiveMessageDelegate)
        {
            MessageReceived += receiveMessageDelegate;
        }

        public void RegisterClientRemovedEvent(ClientRemovedEventHandler ClientRemovedDelegate)
        {
            ClientRemoved += ClientRemovedDelegate;
        }

        protected virtual void OnMessageReceived(MplsPacket packet)
        {
            MessageReceived?.Invoke((_state.PortAlias, packet));
        }

        private void Disconnect()
        {
            try
            {
                _state.ClientSocket.Disconnect(true);
                OnClientRemoved(_state.PortAlias);
                LOG.Info($"Client: {_state.PortAlias} disconnected");
            }
            catch(Exception e)
            {
                LOG.Info($"{e}");
            }
            
        }

         protected virtual void OnClientRemoved(String portAlias)
        {
            // Check if there are any subsribers to this event:
            if (ClientRemoved != null)
            {
                ClientRemoved(this, new ClientRemovedEventArgs() {PortAlias = portAlias});
            }
        }
    }
}
