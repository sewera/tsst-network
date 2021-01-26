using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using MessagePack;

namespace Common.Networking.Client.OneShot
{
    public class OneShotClientPort<TRequestPacket, TResponsePacket> : ClientPort<TRequestPacket, TResponsePacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        public OneShotClientPort(IPAddress serverAddress, int serverPort) : base(serverAddress, serverPort)
        {
            ClientPortAlias = "";
        }

        public override void Send(TRequestPacket packet)
        {
            try
            {
                Log.Info($"Connecting to server on port: {ServerPort}");
                ClientSocket.Connect(ServerEndPoint);
                Log.Info("Connected");
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to connect to server");
            }

            byte[] packetBytes = packet.ToBytes();
            ClientSocket.BeginSend(packetBytes, 0, packetBytes.Length, SocketFlags.None, SendCallback, ClientSocket);
        }

        protected override void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = (byte[]) ar.AsyncState;
                int bytesRead = ClientSocket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    TResponsePacket packet = ISerializablePacket.FromBytes<TResponsePacket>(buffer);
                    Log.Info($"Received: {packet}");
                    OnMessageReceivedEvent(packet);
                }
            }
            catch (MessagePackSerializationException e)
            {
                Log.Warn(e, $"Could not deserialize MessagePack ({typeof(TResponsePacket).Name}) from received bytes");
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in data receiving");
            }
        }
    }
}
