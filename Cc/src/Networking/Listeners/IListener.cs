using System;

namespace Cc.Networking.Listeners
{
    public interface IListener
    {
        void Listen();
        void AcceptCallback(IAsyncResult asyncResult);
    }
}
