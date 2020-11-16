using System.Net.Sockets;
using cn.Models;

namespace cn.Utils
{
    interface IClientNodeManager
    {

        public void ConnectToCableCloud();

        public void Listen(Socket listenerSocket);

        public void SendPacket();

        public int Send(long destinationPort, string message);
    }
}
