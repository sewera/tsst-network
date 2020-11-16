using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cc.Networking.Client;
using Cc.Networking.Tables;
using NLog;

namespace Cc.Networking.Listeners
{
    public class Listener : IListener
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private readonly ManualResetEvent _allDone = new ManualResetEvent(false);

        private readonly IClientWorkerFactory _clientWorkerFactory;
        private readonly IConnectionTable _connectionTable;

        public Listener(IClientWorkerFactory clientWorkerFactory, IConnectionTable connectionTable)
        {
            _clientWorkerFactory = clientWorkerFactory;
            _connectionTable = connectionTable;
        }

        public void Listen()
        {
            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 3001);

            // Create a TCP/IP socket.
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true) {
                    // Set the event to nonsignaled state.
                    _allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    LOG.Info("Waiting for a connection...");
                    listener.BeginAccept(AcceptCallback, listener);

                    // Wait until a connection is made before continuing.
                    _allDone.WaitOne();
                }

            } catch (Exception e) {
                LOG.Error(e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            _allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            if (listener != null)
            {
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                ClientState state = new ClientState(handler);
                LOG.Trace("Adding ClientWorker");
                _connectionTable.AddClientWorker(_clientWorkerFactory.GetClientWorker(state));
            }
            else
            {
                LOG.Fatal("listener is null in AcceptCallback");
            }
        }
    }
}
