using NLog;
using cn.Utils;

namespace cn
{
    class ClientNode
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            LOG.Info("Hi");

            Configuration configuration = new Configuration();
            ClientNodeManager cnManager = new ClientNodeManager(configuration);
            UserInterface ui = new UserInterface();

            cnManager.SendPacket();
        }
    }
}
