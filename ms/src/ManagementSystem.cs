using System;
using System.Threading;

namespace ms
{
    class ManagementSystem
    {
        static void Main(string[] args)
        {
            Config config = new Config();
            config.ReadConfigFile("ManagementSystem.xml");
            IManagementManager mm = new ManagementManager();
            mm.ReadConfig(config);
            mm.startListening();
            MessageSender.ReadConfig(config);
            new Thread(MessageSender.Start).Start();
            
            UserInterface.Start();
        }
    }
}
