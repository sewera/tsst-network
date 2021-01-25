namespace cc.Networking.Client
{
    public interface IClientWorkerFactory
    {
        IClientWorker GetClientWorker(ClientState state);
    }
}
