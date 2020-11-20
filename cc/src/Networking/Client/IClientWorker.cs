using System;
using System.Net.Sockets;
using cc.Models;

namespace cc.Networking.Client
{
    public interface IClientWorker
    {
        void ReadCallback(IAsyncResult ar);
        void Send(MplsPacket mplsPacket);
        void SendCallback(IAsyncResult ar);
        int GetPort();
    }
}
