using System.Net.Sockets;
using System.Text;

namespace Cc.Networking.Client
{
    public class ClientState
    {
        public ClientState(Socket workSocket)
        {
            WorkSocket = workSocket;
        }

        // Size of receive buffer.
        public const int BufferSize = 1024;

        // Receive buffer.
        public byte[] Buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder Sb = new StringBuilder();

        // Client socket.
        public Socket WorkSocket;
    }
}
