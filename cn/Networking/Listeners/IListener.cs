using System;

namespace cn.Networking.Listeners
{
    public interface IListener
    {
        void Listen();
        void AcceptCallback(IAsyncResult asyncResult);
    }
}
