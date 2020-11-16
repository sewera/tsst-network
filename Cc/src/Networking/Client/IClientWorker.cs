using System;
using System.Net.Sockets;

namespace Cc.Networking.Client
{
    public interface IClientWorker
    {
        void ReadCallback(IAsyncResult ar);
        void Send(string data);
        void SendCallback(IAsyncResult ar);
    }
}
