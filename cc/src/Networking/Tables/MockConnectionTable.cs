using System.Collections.Generic;
using cc.Models;
using cc.Networking.Client;
using NLog;

namespace cc.Networking.Tables
{
    public class MockConnectionTable : IConnectionTable
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private readonly List<CcConnection> _ccConnections = new List<CcConnection>();
        private List<(CcConnection, CcConnection, bool)> _connectionTable = new List<(CcConnection, CcConnection, bool)>();

        public (CcConnection, CcConnection, bool) GetRouteFor(string portAlias)
        {
            return (null, null, false); // TODO
        }

        public void AddCcConnection(CcConnection ccConnection)
        {
            _ccConnections.Add(ccConnection);
            LOG.Debug($"Added CcConnection: {ccConnection}");
        }

        public CcConnection GetCcConnection(string portAlias)
        {
            LOG.Debug($"Returning CcConnection: {_ccConnections[0]}");
            return _ccConnections[0];
        }
    }
}
