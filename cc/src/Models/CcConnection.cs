using System.Net;
using cc.Networking.Client;

namespace cc.Models
{
    public class CcConnection
    {
        public string PortAlias { get; set; }
        public IClientWorker ClientWorker { get; set; }

        public CcConnection(string portAlias, IClientWorker clientWorker)
        {
            PortAlias = portAlias;
            ClientWorker = clientWorker;
        }

        public override string ToString()
        {
            return $"[{PortAlias}, ClientWorker:{ClientWorker.GetPort()}]";
        }
    }
}
