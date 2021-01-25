using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using Common.Networking.Server.Delegates;
using NLog;

namespace Common.Networking.Server
{
    public class ServerPort<TPacket> : IServerPort<TPacket> where TPacket : ISerializablePacket
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private const int BufferSize = 1024;
        private byte[] _buffer = new byte[BufferSize];

        private readonly IPAddress _listeningAddress;
        private readonly short _listeningPort;
        private readonly IWorkerFactory<TPacket> _clientWorkerFactory;

        private event RegisterConnection<TPacket> ConnectionRegisteredEvent;

        public ServerPort(IPAddress listeningAddress,
                          short listeningPort,
                          IWorkerFactory<TPacket> clientWorkerFactory)
        {
            _listeningAddress = listeningAddress;
            _listeningPort = listeningPort;
            _clientWorkerFactory = clientWorkerFactory;
        }

        public void Listen()
        {
            // Establish the local endpoint for the socket.
            IPEndPoint localEndPoint = new IPEndPoint(_listeningAddress, _listeningPort);

            // Create a TCP/IP socket.
            Socket listener = new Socket(_listeningAddress.AddressFamily, SocketType.Stream,
                ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                // Start an asynchronous socket to listen for connections.
                _log.Info("Waiting for a connection...");
                listener.BeginAccept(AcceptCallback, listener);
            }
            catch (Exception e)
            {
                _log.Error(e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            if (listener != null)
            {
                Socket handler = listener.EndAccept(ar);

                _log.Debug("Connection accepted");
                // Create the state object.
                _log.Debug("Waiting for Hello packet...");
                // FIXME: Add error handling when client doesn't send a hello packet
                handler.Receive(_buffer);
                TPacket receivedPacket = ISerializablePacket.FromBytes<TPacket>(_buffer);
                _log.Info($"Received: {receivedPacket}");
                _log.Trace("Adding Connection");
                string portAlias = receivedPacket.GetKey();
                ConnectionRegisteredEvent?.Invoke((portAlias, _clientWorkerFactory.GetClientWorker(portAlias, handler)));
                listener.BeginAccept(AcceptCallback, listener);
            }
            else
            {
                _log.Fatal("listener is null in AcceptCallback");
            }
        }

        public void RegisterRegisterConnectionDelegate(RegisterConnection<TPacket> registerConnectionDelegate)
        {
            ConnectionRegisteredEvent += registerConnectionDelegate;
        }
    }
}
