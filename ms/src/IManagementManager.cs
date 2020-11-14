using System;

namespace ms
{
    interface IManagementManager
    {
        void startListening();

        void AcceptCallback(IAsyncResult ar);
    }
}