namespace ClientNetwork.Networking
{
    public interface IClientPortFactory
    {
        IClientPort GetPort(string portAlias);
    }
}
