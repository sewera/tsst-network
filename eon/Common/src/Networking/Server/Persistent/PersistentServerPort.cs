using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using Common.Networking.Server.Delegates;

namespace Common.Networking.Server.Persistent
{
    public class PersistentServerPort<TPacket> : ServerPort<TPacket>, IPersistentServerPort<TPacket>
        where TPacket : ISerializablePacket
    {
        private const int BufferSize = 1024;
        private readonly byte[] _buffer = new byte[BufferSize];

        private readonly IWorkerFactory<TPacket> _clientWorkerFactory;

        private event RegisterConnection<TPacket> ConnectionRegisteredEvent;

        public PersistentServerPort(IPAddress listeningAddress, short listeningPort, IWorkerFactory<TPacket> clientWorkerFactory)
            : base(listeningAddress, listeningPort)
        {
            _clientWorkerFactory = clientWorkerFactory;
        }

        protected override void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            if (listener != null)
            {
                Socket handler = listener.EndAccept(ar);

                Log.Trace("Connection accepted");
                // Create the state object.
                Log.Trace("Waiting for Hello packet...");
                // FIXME: Add error handling when client doesn't send a hello packet
                handler.Receive(_buffer);
                TPacket receivedPacket = ISerializablePacket.FromBytes<TPacket>(_buffer);
                Log.Debug($"Received: {receivedPacket}");
                Log.Trace("Adding Connection");
                string portAlias = receivedPacket.GetKey();
                ConnectionRegisteredEvent?.Invoke((portAlias, _clientWorkerFactory.GetClientWorker(portAlias, handler)));
                listener.BeginAccept(AcceptCallback, listener);
            }
            else
            {
                Log.Fatal("listener is null in AcceptCallback");
            }
        }

        public void RegisterRegisterConnectionDelegate(RegisterConnection<TPacket> registerConnectionDelegate)
        {
            ConnectionRegisteredEvent += registerConnectionDelegate;
        }
    }
}
