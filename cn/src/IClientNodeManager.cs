namespace cn
{
    public interface IClientNodeManager
    {
        void Start();
        void Send(string destinationPortAlias, string message);
    }
}
