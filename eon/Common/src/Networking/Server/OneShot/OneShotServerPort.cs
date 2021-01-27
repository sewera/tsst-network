using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using Common.Networking.Server.Delegates;

namespace Common.Networking.Server.OneShot
{
    public class OneShotServerPort<TRequestPacket, TResponsePacket> : ServerPort<TRequestPacket>, IOneShotServerPort<TRequestPacket, TResponsePacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        private const int BufferSize = 4096;
        private readonly byte[] _buffer = new byte[BufferSize];

        private event ReceiveRequest<TRequestPacket, TResponsePacket> RequestReceivedEvent;

        public OneShotServerPort(IPAddress listeningAddress, int listeningPort) : base(listeningAddress, listeningPort)
        {
        }

        public void RegisterReceiveRequestDelegate(ReceiveRequest<TRequestPacket, TResponsePacket> registerConnectionDelegate)
        {
            RequestReceivedEvent = registerConnectionDelegate;
        }

        protected override void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            if (listener != null)
            {
                Socket handler = listener.EndAccept(ar);

                Log.Trace("Connection accepted");
                Log.Trace("Waiting for a packet...");
                handler.Receive(_buffer);
                TRequestPacket receivedRequestPacket = ISerializablePacket.FromBytes<TRequestPacket>(_buffer);
                Log.Debug($"Received: {receivedRequestPacket}");
                TResponsePacket responsePacket = OnRequestReceivedEvent(receivedRequestPacket);
                handler.Send(responsePacket.ToBytes());
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

                listener.BeginAccept(AcceptCallback, listener);
            }
            else
            {
                Log.Fatal("listener is null in AcceptCallback");
            }
        }

        private TResponsePacket OnRequestReceivedEvent(TRequestPacket requestPacket)
        {
            if (RequestReceivedEvent != null)
                return RequestReceivedEvent.Invoke(requestPacket);

            Log.Error("No delegate was registered in RequestReceivedEvent");
            throw new Exception("No delegate was registered in RequestReceivedEvent");
        }
    }
}
