using System;
using cc.Models;
using cc.Networking.Delegates;

namespace cc.Networking.Client
{
    public interface IClientWorker
    {
        public void ReadCallback(IAsyncResult ar);
        public void Send(MplsPacket mplsPacket);
        public void SendCallback(IAsyncResult ar);
        public int GetPort();
        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate receiveMessageDelegate);
    }
}
