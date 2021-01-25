using System;
using CableCloud.Models;
using CableCloud.Networking.Delegates;

namespace CableCloud.Networking.Client
{
    public interface IClientWorker
    {
        public void ReadCallback(IAsyncResult ar);
        public void Send(MplsPacket mplsPacket);
        public void SendCallback(IAsyncResult ar);
        public int GetPort();
        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate receiveMessageDelegate);

        public void RegisterClientRemovedEvent(ClientRemovedEventHandler ClientRemovedDelegate);
    }
}
