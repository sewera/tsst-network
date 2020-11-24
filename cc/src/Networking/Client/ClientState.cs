using System.Net.Sockets;
using cc.Models;

namespace cc.Networking.Client
{
    public class ClientState
    {
        public ClientState(Socket workSocket)
        {
            ClientSocket = workSocket;
        }

        public const int BufferSize = 1024;

        // Receive buffer.
        public byte[] Buffer = new byte[BufferSize];

        // Received data string.
        public MplsPacket Packet;

        // Client socket.
        public Socket ClientSocket;

        public string PortAlias;
    }
}
