using System.Net.Sockets;
using System.Text;
using Cc.Models;

namespace Cc.Networking.Client
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
    }
}
