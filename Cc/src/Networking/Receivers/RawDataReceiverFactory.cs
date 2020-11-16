using System.Net.Sockets;

namespace Cc.Networking.Receivers
{
    public class RawDataReceiverFactory : IDataReceiverFactory
    {
        public IDataReceiver GetDataReceiver(Socket socket)
        {
            return new RawDataReceiver(socket);
        }
    }
}
