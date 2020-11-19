using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cc.Config;
using Cc.Models;
using Cc.Networking.Client;
using Cc.Networking.Tables;
using NLog;

namespace Cc.Networking.Listeners
{
    public class Listener : IListener
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Configuration _configuration;
        private readonly IClientWorkerFactory _clientWorkerFactory;
        private readonly IConnectionTable _connectionTable;

        public Listener(Configuration configuration,
                        IClientWorkerFactory clientWorkerFactory,
                        IConnectionTable connectionTable)
        {
            _configuration = configuration;
            _clientWorkerFactory = clientWorkerFactory;
            _connectionTable = connectionTable;
        }

        public void Listen()
        {
            // Establish the local endpoint for the socket.
            IPEndPoint localEndPoint = new IPEndPoint(_configuration.ListeningAddress, _configuration.ListeningPort);

            // Create a TCP/IP socket.
            Socket listener = new Socket(_configuration.ListeningAddress.AddressFamily, SocketType.Stream,
                ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                // Start an asynchronous socket to listen for connections.
                LOG.Info("Waiting for a connection...");
                listener.BeginAccept(AcceptCallback, listener);

            } catch (Exception e) {
                LOG.Error(e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            if (listener != null)
            {
                Socket handler = listener.EndAccept(ar);

                LOG.Debug("Connection accepted");
                // Create the state object.
                ClientState state = new ClientState(handler);
                LOG.Debug("Waiting for Hello packet...");
                // FIXME: Add error handling when client doesn't send a hello packet
                handler.Receive(state.Buffer);
                MplsPacket receivedPacket = MplsPacket.FromBytes(state.Buffer);
                LOG.Debug($"Received: {receivedPacket}");
                LOG.Trace("Adding CcConnection");
                _connectionTable.AddCcConnection(
                    new CcConnection(receivedPacket.SourcePortAlias, _clientWorkerFactory.GetClientWorker(state)));
                listener.BeginAccept(AcceptCallback, listener);
            }
            else
            {
                LOG.Fatal("listener is null in AcceptCallback");
            }
        }
    }
}
