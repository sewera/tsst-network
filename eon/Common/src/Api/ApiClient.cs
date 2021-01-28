using System;
using System.Net;
using System.Threading;
using Common.Models;
using Common.Networking.Client.OneShot;
using NLog;

namespace Common.Api
{
    /// <summary>
    /// A simple client for synchronous request / response.
    /// </summary>
    /// <typeparam name="TRequestPacket">Request packet type</typeparam>
    /// <typeparam name="TResponsePacket">Response packet type</typeparam>
    public class ApiClient<TRequestPacket, TResponsePacket> : IApiClient<TRequestPacket, TResponsePacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IPAddress _serverAddress;
        private readonly int _serverPort;
        private readonly ManualResetEvent _receiveDone = new ManualResetEvent(false);
        private TResponsePacket _responsePacket;

        public ApiClient(IPAddress serverAddress, int serverPort)
        {
            _serverAddress = serverAddress;
            _serverPort = serverPort;
        }

        public TResponsePacket Get(TRequestPacket requestPacket)
        {
            const int n = 10;
            for (int i = 1; i <= n; i++)
            {
                try
                {
                    OneShotClientPort<TRequestPacket, TResponsePacket> clientPort =
                        new OneShotClientPort<TRequestPacket, TResponsePacket>(_serverAddress, _serverPort);

                    clientPort.RegisterReceiveMessageEvent(OnMessageReceived);
                    clientPort.Send(requestPacket);
                    _receiveDone.WaitOne();
                    _receiveDone.Reset();
                    clientPort.ShutdownAndClose();
                    return _responsePacket;
                }
                catch (Exception)
                {
                    if (i < 5)
                        _log.Debug($"Could not connect to the server on port: {_serverPort}. ({i} out of {n})");
                    else
                        _log.Warn($"Could not connect to the server on port: {_serverPort}. ({i} out of {n})");
                    _log.Trace($"Sending packet: {requestPacket}");
                    Thread.Sleep(1000);
                }
            }

            throw new Exception($"Could not connect to the server on port: {_serverPort} after {n} retries");
        }

        private void OnMessageReceived((string, TResponsePacket) responseTuple)
        {
            (_, _responsePacket) = responseTuple;
            _receiveDone.Set();
        }
    }
}
