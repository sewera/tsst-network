using System;
using NLog;

namespace cn.Utils
{
    class UserInterface : IUserInterface
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        public UserInterface()
        {

        }

        public (string, string) EnterReceiverAndMessage()
        {
            LOG.Info("Enter alias of remote host and message you want to send.\nInput format: <<port_serial_no>> [space] <<message>>");

            string input = Console.ReadLine();
            string[] parts = input.Split(' ', 2);

            return (parts[0], parts[1]);
        }
    }
}

