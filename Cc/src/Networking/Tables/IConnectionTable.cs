using Cc.Models;
using Cc.Networking.Client;

namespace Cc.Networking.Tables
{
    public interface IConnectionTable
    {
        /// <summary>Method for finding the connection for given <paramref name="portSerialNo"/></summary>
        /// <param name="portSerialNo">Port serial number where the packet came from</param>
        /// <returns>A tuple with: CcConnection of the port where the packet came from, CcConnection of
        /// of connected port (destination) and bool whether the port is alive in this exact order</returns>
        (CcConnection, CcConnection, bool) GetRouteFor(long portSerialNo);

        void AddClientWorker(IClientWorker clientWorker);

        IClientWorker GetClientWorker(int index);
    }
}
