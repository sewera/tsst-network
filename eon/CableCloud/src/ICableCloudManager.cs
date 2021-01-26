using Common.Startup;

namespace CableCloud
{
    public interface ICableCloudManager : IManager
    {
        public void SetConnectionAlive((string, string, bool) requestedConnection);
    }
}
