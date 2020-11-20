using cc.Models;
using cc.Networking.Client;

namespace cc.Networking.Tables
{
    public interface IConnectionTable
    {
        /// <summary>Method for finding the connection for given <paramref name="portAlias"/></summary>
        /// <param name="portAlias">Port alias where the packet came from</param>
        /// <returns>A tuple with: CcConnection of the port where the packet came from, CcConnection of
        /// of connected port (destination) and bool whether the link is alive, in this exact order</returns>
        (CcConnection, CcConnection, bool) GetRouteFor(string portAlias);

        void AddCcConnection(CcConnection ccConnection);

        CcConnection GetCcConnection(string portAlias);
    }
}
