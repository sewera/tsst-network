using System;
using System.Net;
using System.Net.Sockets;
using MessagePack;
using NLog;
using nn.Config;
using nn.Models;
using nn.Networking.Delegates;

namespace nn.Networking.Management
{
    public class ManagementPort : IPort<ManagementPacket>
    {
        private const int BufferSize = 1024;
        private const int Retries = 10;
        private readonly Configuration _configuration;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Socket _managementSocket;

        public event ReceiveMessageDelegate<ManagementPacket> MessageReceived;

        public ManagementPort(Configuration configuration)
        {
            _configuration = configuration;
            _managementSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Send(ManagementPacket managementPacket)
        {
            LOG.Debug($"Sending packet: {managementPacket}");
            byte[] packetBytes = managementPacket.ToBytes();
            _managementSocket.BeginSend(packetBytes, 0, packetBytes.Length, SocketFlags.None, SendCallback, _managementSocket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = _managementSocket.EndSend(ar);
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
                    LOG.Info($"Connecting to ManagementSystem on port: {_configuration.ManagementSystemPort}");
                    _managementSocket.Connect(_configuration.ManagementSystemEndPoint);
                    LOG.Info("Connected");
                    ManagementPacket packet = new ManagementPacket.Builder()
                        .SetCommandType("SYNC")
                        .SetCommandData($"{((IPEndPoint) _managementSocket.LocalEndPoint).Port}") // TODO: Send router alias from config
                        .Build();
                    _managementSocket.Send(packet.ToBytes());
                    LOG.Debug($"Sent hello packet to MS: {packet}");
                    return;
                }
                catch (Exception e)
                {
                    throw e;
                    LOG.Warn(e, $"Failed to connect to management system, try {i}/{Retries}");
                }
            }

            LOG.Fatal($"Failed to connect to management system after {Retries} tries");
            Environment.Exit(1);
        }

        public void StartReceiving()
        {
            for (int i = 1; i <= Retries; i++)
            {
                try
                {
                    byte[] buffer = new byte[BufferSize];
                    _managementSocket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, buffer);
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

        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate<ManagementPacket> receiveMessage)
        {
            MessageReceived += receiveMessage;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = (byte[]) ar.AsyncState;
                int bytesRead = _managementSocket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    ManagementPacket receivedPacket = ManagementPacket.FromBytes(buffer);
                    LOG.Debug($"Received: {receivedPacket}");
                    OnMessageReceived(("", receivedPacket));
                }
            }
            catch (MessagePackSerializationException e)
            {
                LOG.Warn(e, "Could not deserialize MessagePack (ManagementPacket) from received bytes");
            }
            catch (Exception e)
            {
                LOG.Error(e, "Error in data receiving");
            }
            finally
            {
                byte[] buffer = new byte[BufferSize];
                _managementSocket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, buffer);
            }
        }

        protected virtual void OnMessageReceived((string portAlias, ManagementPacket managementPacket) managementTuple)
        {
            MessageReceived?.Invoke(managementTuple);
        }
    }
}
