using System;
using System.Net;
using System.Net.Sockets;
using cn.Config;
using cn.Models;
using MessagePack;
using NLog;

namespace cn.Networking
{
    public class ClientPort : IClientPort
    {
        private const int BUFFER_SIZE = 1024;
        private readonly Configuration _configuration;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private Socket _clientSocket;
        private string _clientPortAlias;

        public ClientPort(string clientPortAlias, Configuration configuration)
        {
            _clientPortAlias = clientPortAlias;
            _configuration = configuration;
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Send(MplsPacket mplsPacket)
        {
            if (mplsPacket.SourcePortAlias != _clientPortAlias)
            {
                LOG.Warn("Source port alias is not the same as current client port alias");
            }

            LOG.Debug($"Sending packet: {mplsPacket}");
            byte[] packetBytes = MplsPacket.ToBytes(mplsPacket);
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

        public void ConnectToCableCloud()
        {
            try
            {
                LOG.Info($"Connecting to CableCloud on port: {_configuration.CableCloudPort}");
                _clientSocket.Connect(_configuration.CableCloudEndPoint);
                LOG.Info("Connected");
                MplsPacket packet = new MplsPacket.Builder()
                    .SetSourcePortAlias(_configuration.ClientPortAlias)
                    .Build();
                _clientSocket.Send(MplsPacket.ToBytes(packet));
                LOG.Debug($"Sent hello packet to CC: {packet}");
            }
            catch (Exception e)
            {
                LOG.Fatal(e, "Failed to connect to cable cloud");
                Environment.Exit(1);
            }
        }

        public void StartReceiving()
        {
            try
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                _clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, buffer);
            }
            catch (Exception e)
            {
                LOG.Warn(e, "Could not start receiving");
            }
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
                    // TODO: Send receivedPacket back to UserInterface, probably with events
                    // https://docs.microsoft.com/en-us/dotnet/standard/events/how-to-raise-and-consume-events
                    LOG.Info($"Received: {receivedPacket}");
                }
            }
            catch (MessagePackSerializationException e)
            {
                LOG.Warn(e, "Could not deserialize MessagePack (MplsPacket) from received bytes");
            }
            catch (Exception e)
            {
                LOG.Error(e, "Error in data receiving");
            }
            finally
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                _clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, buffer);
            }
        }
    }
}
