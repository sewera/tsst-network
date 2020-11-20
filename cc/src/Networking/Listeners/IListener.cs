using System;

namespace cc.Networking.Listeners
{
    public interface IListener
    {
        void Listen();
        void AcceptCallback(IAsyncResult asyncResult);
    }
}
