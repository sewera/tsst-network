using System;
using System.Net.Sockets;
using MessagePack;
using NLog;
using nn.Config;
using nn.Models;
using nn.Networking.Delegates;

namespace nn.Networking.Client
{
    public class ClientPort : IPort<MplsPacket>
    {
        private const int BufferSize = 1024;
        private const int Retries = 10;
        private readonly Configuration _configuration;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Socket _clientSocket;
        private readonly string _clientPortAlias;

        public event ReceiveMessageDelegate<MplsPacket> MessageReceived;

        public ClientPort(string clientPortAlias, Configuration configuration)
        {
            _clientPortAlias = clientPortAlias;
            _configuration = configuration;
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Send(MplsPacket mplsPacket)
        {
            LOG.Debug($"Sending packet: {mplsPacket}");
            byte[] packetBytes = mplsPacket.ToBytes();
            _clientSocket.BeginSend(packetBytes, 0, packetBytes.Length, SocketFlags.None, SendCallback, _clientSocket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = _clientSocket.EndSend(ar);
                LOG.Trace($"Sent {bytesSent} bytes to server");
            }
            catch (Exception e)
            {
                LOG.Warn(e, "Could not send data");
            }
        }

        public void Connect()
        {
            for (int i = 1; i <= Retries; i++)
            {
                try
                {
                    LOG.Info($"Connecting to CableCloud on port: {_configuration.CableCloudPort}");
                    _clientSocket.Connect(_configuration.CableCloudEndPoint);
                    LOG.Info("Connected");
                    MplsPacket packet = new MplsPacket.Builder()
                        .SetSourcePortAlias(_clientPortAlias)
                        .Build();
                    _clientSocket.Send(packet.ToBytes());
                    LOG.Debug($"Sent hello packet to CC: {packet}");
                    return;
                }
                catch (Exception e)
                {
                    LOG.Warn(e, $"Failed to connect to cable cloud, try {i}/{Retries}");
                }
            }
            LOG.Fatal($"Failed to connect to cable cloud after {Retries} tries");
            Environment.Exit(1);
        }

        public void StartReceiving()
        {
            for (int i = 1; i <= Retries; i++)
            {
                try
                {
                    byte[] buffer = new byte[BufferSize];
                    _clientSocket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, buffer);
                    return;
                }
                catch (Exception e)
                {
                    LOG.Warn(e, $"Could not start receiving, try {i}/{Retries}");
                }
            }

            LOG.Fatal($"Could not start receiving after {Retries} retries");
            Environment.Exit(2);
        }

        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate<MplsPacket> receiveMessage)
        {
            MessageReceived += receiveMessage;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = (byte[]) ar.AsyncState;
                int bytesRead = _clientSocket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    MplsPacket receivedPacket = MplsPacket.FromBytes(buffer);
                    LOG.Debug($"Received: {receivedPacket} on port: {_clientPortAlias}");
                    OnMessageReceived(receivedPacket);
                }
            }
            catch (MessagePackSerializationException e)
            {
                LOG.Warn(e, "Could not deserialize MessagePack (MplsPacket) from received bytes");
            }
            catch (Exception e)
            {
                LOG.Error(e, "Error in ReceiveCallback / OnMessageReceived event");
            }
            finally
            {
                byte[] buffer = new byte[BufferSize];
                _clientSocket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, buffer);
            }
        }

        protected virtual void OnMessageReceived(MplsPacket mplsPacket)
        {
            MessageReceived?.Invoke((_clientPortAlias, mplsPacket));
        }
    }
}
