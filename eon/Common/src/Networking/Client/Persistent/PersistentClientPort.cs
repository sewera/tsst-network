using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using MessagePack;

namespace Common.Networking.Client.Persistent
{
    public class PersistentClientPort<TPacket> : ClientPort<TPacket, TPacket>, IPersistentClientPort<TPacket>
        where TPacket : ISerializablePacket
    {
        public PersistentClientPort(string clientPortAlias, IPAddress serverAddress, int serverPort) : base(serverAddress, serverPort)
        {
            ClientPortAlias = clientPortAlias;
        }

        public override void Send(TPacket packet)
        {
            byte[] packetBytes = packet.ToBytes();
            ClientSocket.BeginSend(packetBytes, 0, packetBytes.Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        public void ConnectPermanentlyToServer(TPacket helloPacket)
        {
            try
            {
                Log.Debug($"Connecting to server on port: {ServerPort}");
                ClientSocket.Connect(ServerEndPoint);
                Log.Debug("Connected");
                if (helloPacket.GetKey() != ClientPortAlias)
                {
                    Log.Warn("Provided helloPacket has a different key than it should (helloPacket.GetKey() != _clientPortAlias)");
                    Log.Warn($"({helloPacket.GetKey()} != {ClientPortAlias})");
                }

                ClientSocket.Send(helloPacket.ToBytes());
                Log.Debug($"Sent hello packet to server: {helloPacket}");
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Failed to connect to server");
                Environment.Exit(1);
            }
        }

        protected override void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = (byte[]) ar.AsyncState;
                int bytesRead = ClientSocket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    TPacket packet = ISerializablePacket.FromBytes<TPacket>(buffer);
                    Log.Debug($"Received: {packet}");
                    OnMessageReceivedEvent(packet);
                }
            }
            catch (MessagePackSerializationException e)
            {
                Log.Warn(e, $"Could not deserialize MessagePack ({typeof(TPacket).Name}) from received bytes");
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in data receiving");
            }
            finally
            {
                byte[] buffer = new byte[BufferSize];
                ClientSocket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, buffer);
            }
        }
    }
}
