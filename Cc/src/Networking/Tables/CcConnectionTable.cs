using System.Collections.Generic;
using Cc.Models;

namespace Cc.Networking.Tables
{
    internal class CcConnectionTable : IConnectionTable
    {
        private IList<(CcConnection, CcConnection, bool)> _connectionTable;

        public (CcConnection, CcConnection, bool) GetRouteFor(long portSerialNo)
        {
            return (null, null, false); // TODO
        }
    }
}
