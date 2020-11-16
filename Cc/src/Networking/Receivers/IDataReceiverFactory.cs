using System.Net.Sockets;

namespace Cc.Networking.Receivers
{
    public interface IDataReceiverFactory
    {
        IDataReceiver GetDataReceiver(Socket socket);
    }
}
