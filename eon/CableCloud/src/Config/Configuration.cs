using System.Collections.Generic;
using System.Net;

namespace CableCloud.Config
{
    public class Configuration
    {
        public IPAddress ListeningAddress { get; set; }
        public short ListeningPort { get; set; }
        
        public List<(string, string, bool)> ConnectionTable;

        private Configuration(IPAddress listeningAddress, short listeningPort, List<(string, string, bool)> connectionTable)
        {
            ListeningAddress = listeningAddress;
            ListeningPort = listeningPort;
            ConnectionTable = connectionTable;
        }

        public class Builder
        {
            public Builder SetListeningAddress(string listeningAddress)
            {
                _listeningAddress = IPAddress.Parse(listeningAddress);
                return this;
            }

            public Builder SetListeningPort(short listeningPort)
            {
                _listeningPort = listeningPort;
                return this;
            }

            public Builder SetConnectionTable(List<(string, string, bool)> connectionTable)
            {
                _connectionTable = connectionTable;
                return this;
            }

            public Builder AddConnection((string, string, bool) connection)
            {
                _connectionTable ??= new List<(string, string, bool)>();
                _connectionTable.Add(connection);
                return this;
            }

            public Configuration Build()
            {
                _connectionTable ??= new List<(string, string, bool)>();
                return new Configuration(_listeningAddress, _listeningPort, _connectionTable);
            }

            private IPAddress _listeningAddress;
            private short _listeningPort;
            private List<(string, string, bool)> _connectionTable;
        }
    }
}
