using System;
using System.Net.Sockets;
using Cc.Models;

namespace Cc.Networking.Client
{
    public interface IClientWorker
    {
        void ReadCallback(IAsyncResult ar);
        void Send(MplsPacket mplsPacket);
        void SendCallback(IAsyncResult ar);
        int GetPort();
    }
}
