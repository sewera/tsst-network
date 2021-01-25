using System;
using CableCloud.Networking.Delegates;

namespace CableCloud.Networking.Listeners
{
    public interface IListener
    {
        void Listen();
        void AcceptCallback(IAsyncResult asyncResult);
        public void RegisterWorkerConnectionEvent(RegisterConnectionDelegate registerConnectionDelegate);
    }
}
