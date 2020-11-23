namespace nn.src.Networking
{
    public interface IClientPortFactory
    {
        IClientPort GetPort(string portAlias);
    }
}
