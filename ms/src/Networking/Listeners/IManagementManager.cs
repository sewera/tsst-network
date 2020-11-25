using System;

namespace ms
{
    interface IManagementManager
    {
        /// <summary>
        /// This method starts the process of listening for incoming connections from clients - network nodes
        /// </summary>
        void startListening();
        /// <summary>
        /// This method is called when new connection popped up and needs to be serviced
        /// <summary> 
        void AcceptCallback(IAsyncResult ar);

        /// <summary>
        /// Read config, actually the Listener Socket port
        /// </summary>
        void ReadConfig(Config config);
    }
}