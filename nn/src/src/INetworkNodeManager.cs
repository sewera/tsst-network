using nn.src.Networking.Delegates;

namespace nn.src
{
    public interface INetworkNodeManager
    {
        public void Start();
        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage);
        public void Send(string destinationPortAlias, string message);
    }
}
