using System.Collections.Generic;
using cc.Models;

namespace cc.Networking.Tables
{
    class CcConnectionTable : IConnectionTable
    {
        private IList<(CcConnection, CcConnection, bool)> connectionTable;

        public (CcConnection, CcConnection, bool) GetRouteFor(long portSerialNo)
        {
            return (null, null, false); // TODO
        }
    }
}
