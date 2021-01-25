namespace ClientNode.Networking
{
    public interface IClientPortFactory
    {
        IClientPort GetPort(string portAlias);
    }
}
