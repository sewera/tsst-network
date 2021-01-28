using System;
using System.Net;
using System.Net.Sockets;
using Common.Models;
using NLog;

namespace Common.Networking.Server
{
    public abstract class ServerPort<TPacket> : IServerPort<TPacket> where TPacket : ISerializablePacket
    {
        protected readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IPEndPoint _listeningEndPoint;
        private readonly IPAddress _listeningAddress;
        private readonly int _listeningPort;

        protected ServerPort(IPAddress listeningAddress, int listeningPort)
        {
            _listeningAddress = listeningAddress;
            _listeningPort = listeningPort;
            _listeningEndPoint = new IPEndPoint(_listeningAddress, _listeningPort);
        }

        public void Listen()
        {
            // Create a TCP/IP socket.
            Socket listener = new Socket(_listeningAddress.AddressFamily, SocketType.Stream,
                ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(_listeningEndPoint);
                listener.Listen(100);
                // Start an asynchronous socket to listen for connections.
                Log.Debug($"Waiting for a connection on port {_listeningPort}");
                listener.BeginAccept(AcceptCallback, listener);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        protected abstract void AcceptCallback(IAsyncResult ar);
    }
}
