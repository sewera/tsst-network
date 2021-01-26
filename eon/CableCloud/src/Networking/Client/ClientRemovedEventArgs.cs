using System;
namespace CableCloud.Networking.Client
{
    // Class responsible for sending an email whehnever video is encoded
    public class ClientRemovedEventArgs : EventArgs
    {
        public String PortAlias { get; set; }
    }
}
