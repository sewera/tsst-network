using nn.Networking.Delegates;

namespace nn
{
    public interface INetworkNodeManager
    {
        public void Start();
        public void RegisterReceiveMessageEvent(string sourcePortAlias, ReceiveMessageDelegate receiveMessageDelegate);
        public void Send(string sourcePortAlias, string destinationPortAlias, string message);
    }
}
