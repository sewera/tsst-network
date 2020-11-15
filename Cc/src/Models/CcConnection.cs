using System.Net;

namespace Cc.Models
{
    class CcConnection
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public long PortSerialNo { get; set; }
    }
}
