using cn.Utils;
using cn.Models;
using System;

namespace cn
{
    class ClientNode
    {
        public static void Main(string[] args)
        {
            Configuration configuration = new Configuration();
            ClientNodeManager cnManager = new ClientNodeManager(configuration);
            UserInterface ui = new UserInterface();

            cnManager.SendPacket();
        }
    }
}
