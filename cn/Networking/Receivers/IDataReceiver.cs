using System;

namespace cn.Networking.Receivers
{
    public interface IDataReceiver
    {
        void StartReceiving();
        void ReceiveCallback(IAsyncResult asyncResult);
        void Disconnect();
    }
}
