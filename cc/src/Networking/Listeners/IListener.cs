using System;
using cc.Networking.Delegates;

namespace cc.Networking.Listeners
{
    public interface IListener
    {
        void Listen();
        void AcceptCallback(IAsyncResult asyncResult);
        public void RegisterWorkerConnectionEvent(RegisterConnectionDelegate registerConnectionDelegate);
    }
}
