using System.Collections.Generic;
using Cc.Models;
using Cc.Networking.Client;

namespace Cc.Networking.Tables
{
    internal class MockConnectionTable : IConnectionTable
    {
        private readonly List<IClientWorker> _clientWorkers = new List<IClientWorker>();
        private List<(CcConnection, CcConnection, bool)> _connectionTable;

        public (CcConnection, CcConnection, bool) GetRouteFor(long portSerialNo)
        {
            return (null, null, false); // TODO
        }

        public void AddClientWorker(IClientWorker clientWorker)
        {
            _clientWorkers.Add(clientWorker);
        }

        public IClientWorker GetClientWorker(int index)
        {
            return _clientWorkers[index];
        }
    }
}
