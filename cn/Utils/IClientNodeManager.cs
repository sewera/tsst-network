using System.Net.Sockets;

namespace cn.Utils
{
    interface IClientNodeManager
    {
        /// <summary>Accept destination addres and user's message
        /// entered by user</summary>
        public void ConnectToCableCloud();

        public void Listen(Socket listenerSocket);

        /// <summary>Accept destination addres and user's message
        /// entered by user</summary>
        public void SendPacket();
    }
}
