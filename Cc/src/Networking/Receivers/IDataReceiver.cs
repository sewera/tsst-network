using System;

namespace Cc.Networking.Receivers
{
    public interface IDataReceiver
    {
        void StartReceiving();
        void ReceiveCallback(IAsyncResult asyncResult);
        void Disconnect();
    }
}
