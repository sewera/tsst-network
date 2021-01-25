namespace cn.Networking
{
    public interface IClientPortFactory
    {
        IClientPort GetPort(string portAlias);
    }
}
