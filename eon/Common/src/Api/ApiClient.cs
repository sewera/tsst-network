using System.Net;
using System.Threading;
using Common.Models;
using Common.Networking.Client;
using Common.Networking.Client.OneShot;

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
            OneShotClientPort<TRequestPacket, TResponsePacket> clientPort = new OneShotClientPort<TRequestPacket, TResponsePacket>(_serverAddress, _serverPort);

            clientPort.RegisterReceiveMessageEvent(OnMessageReceived);
            clientPort.Send(requestPacket);
            _receiveDone.WaitOne();
            _receiveDone.Reset();
            clientPort.ShutdownAndClose();
            return _responsePacket;
        }

        private void OnMessageReceived((string, TResponsePacket) responseTuple)
        {
            (_, _responsePacket) = responseTuple;
            _receiveDone.Set();
        }
    }
}
