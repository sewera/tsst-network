using System.Net;

namespace cc.Models
{
    class CcConnection
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public long PortSerialNo { get; set; }
    }
}
