using CableCloud.Models;

namespace CableCloud.Networking.Delegates
{
    public delegate void ReceiveMessageDelegate((string, MplsPacket) receiveMessageTuple);
}
