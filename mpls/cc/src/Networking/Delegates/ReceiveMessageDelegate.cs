using cc.Models;

namespace cc.Networking.Delegates
{
    public delegate void ReceiveMessageDelegate((string, MplsPacket) receiveMessageTuple);
}
