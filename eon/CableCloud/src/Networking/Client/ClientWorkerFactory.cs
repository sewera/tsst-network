namespace CableCloud.Networking.Client
{
    public class ClientWorkerFactory : IClientWorkerFactory
    {
        public IClientWorker GetClientWorker(ClientState state)
        {
            return new ClientWorker(state);
        }
    }
}
