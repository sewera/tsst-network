using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using Common.Networking.Server.Delegates;
using NLog;

namespace Common.Networking.Server
{
    public class Worker<TPacket> : IWorker<TPacket> where TPacket : ISerializablePacket
    {
        private event HandleClientRemoval ClientRemovedEvent;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private Socket _socket;

        private const int BufferSize = 1024;
        private byte[] _buffer = new byte[BufferSize];

        private readonly string _portAlias;

        private event ReceiveMessage<TPacket> MessageReceivedEvent;

        public Worker(string portAlias, Socket socket)
        {
            _portAlias = portAlias;
            _socket = socket;
            // FIXME: Catch connection reset exception to avoid program crashes after client crashes
            _socket.BeginReceive(_buffer, 0, BufferSize, 0, ReadCallback, _buffer);
            _log.Debug("Created ClientWorker for client socket: " +
                      $"{((IPEndPoint) _socket.RemoteEndPoint).Address}" +
                      $":{((IPEndPoint) _socket.RemoteEndPoint).Port}");
        }

        private void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            try
            {
                _buffer = (byte[]) ar.AsyncState;
                if (_buffer != null)
                {
                    Socket handler = _socket;

                    // Read data from the client socket.
                    int bytesRead = handler.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        TPacket packet = ISerializablePacket.FromBytes<TPacket>(_buffer);
                        _log.Debug($"Received: {packet}");

                        OnMessageReceived(packet);

                        handler.BeginReceive(_buffer, 0, BufferSize, 0, ReadCallback, _buffer);
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                else
                {
                    _log.Fatal("_buffer.WorkSocket is null in ReadCallback");
                }
            }
            catch
            {
                // If exception is thrown, check if socket is connected, because you can start receiving again. If not - Disconnect.
                if (!_socket.Connected)
                {
                    Disconnect();
                }
                else
                {
                    // HERE
                    _socket.BeginReceive(_buffer, 0, BufferSize, 0, ReadCallback, _buffer);
                }
            }
        }

        public void Send(TPacket packet)
        {
            byte[] bytes = packet.ToBytes();
            _socket.BeginSend(bytes, 0, bytes.Length, 0, SendCallback, _socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                if (handler != null)
                {
                    int bytesSent = handler.EndSend(ar);
                    _log.Debug($"Sent {bytesSent} bytes to client");
                    handler.BeginReceive(_buffer, 0, BufferSize, 0, ReadCallback, _buffer);
                }
                else
                {
                    _log.Fatal("handler is null in SendCallback");
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        public int GetPort()
        {
            return ((IPEndPoint) _socket.RemoteEndPoint).Port;
        }

        public void RegisterReceiveMessageDelegate(ReceiveMessage<TPacket> receiveMessageDelegate)
        {
            MessageReceivedEvent += receiveMessageDelegate;
        }

        public void RegisterClientRemovedDelegate(HandleClientRemoval handleClientRemovalDelegate)
        {
            ClientRemovedEvent += handleClientRemovalDelegate;
        }

        protected virtual void OnMessageReceived(TPacket packet)
        {
            MessageReceivedEvent?.Invoke((_portAlias, packet));
        }

        private void Disconnect()
        {
            try
            {
                _socket.Disconnect(true);
                ClientRemovedEvent?.Invoke(_portAlias);
                _log.Info($"Client: {_portAlias} disconnected");
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }
    }
}
