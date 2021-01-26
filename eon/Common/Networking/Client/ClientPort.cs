using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using Common.Networking.Client.Delegates;
using NLog;

namespace Common.Networking.Client
{
    public abstract class ClientPort<TRequestPacket, TResponsePacket> : IClientPort<TRequestPacket, TResponsePacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        protected const int BufferSize = 4096;
        protected readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected readonly IPEndPoint ServerEndPoint;
        protected readonly IPAddress ServerAddress;
        protected readonly int ServerPort;

        protected string ClientPortAlias;

        protected readonly Socket ClientSocket;

        public event ReceiveMessage<TResponsePacket> MessageReceivedEvent;

        public ClientPort(IPAddress serverAddress, int serverPort)
        {
            ServerAddress = serverAddress;
            ServerPort = serverPort;
            ServerEndPoint = new IPEndPoint(serverAddress, serverPort);
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public abstract void Send(TRequestPacket packet);

        protected void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = ClientSocket.EndSend(ar);
                Log.Trace($"Sent {bytesSent} bytes to server");
            }
            catch (Exception e)
            {
                Log.Warn(e, "Could not send data");
            }
        }

        public void StartReceiving()
        {
            try
            {
                byte[] buffer = new byte[BufferSize];
                ClientSocket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, buffer);
            }
            catch (Exception e)
            {
                Log.Warn(e, "Could not start receiving");
            }
        }

        public void RegisterReceiveMessageEvent(ReceiveMessage<TResponsePacket> receiveMessage)
        {
            MessageReceivedEvent += receiveMessage;
        }

        protected void OnMessageReceivedEvent(TResponsePacket packet)
        {
            MessageReceivedEvent?.Invoke((ClientPortAlias, packet));
        }

        protected abstract void ReceiveCallback(IAsyncResult ar);
    }
}
