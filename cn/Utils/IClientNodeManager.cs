namespace cn.Utils
{
    interface IClientNodeManager
    {

        public void ConnectToCableCloud();

        public void SendPacket();

        public int Send(long destinationPort, string message, int packetsSend);
    }
}
