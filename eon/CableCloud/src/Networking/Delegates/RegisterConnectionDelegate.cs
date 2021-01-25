using CableCloud.Networking.Client;

namespace CableCloud.Networking.Delegates
{
    public delegate void RegisterConnectionDelegate((string, IClientWorker) worker);
}
