namespace cc
{
    public interface ICableCloudManager
    {
        public void Start();
        public void SetConnectionAlive((string, string, bool) requestedConnection);
    }
}
