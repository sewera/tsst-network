namespace nn.Networking
{
    public interface IClientPortFactory
    {
        IClientPort GetPort(string portAlias);
    }
}
