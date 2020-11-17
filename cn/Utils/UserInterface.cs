using System;
using NLog;

namespace cn.Utils
{
    class UserInterface : IUserInterface
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Remote host port serial no - in our case its alias
        /// </summary>
        public long PortSerialNo { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        public string Message { get; set; }

        public UserInterface()
        {

        }

        public (long, string) EnterReceiverAndMessage()
        {
            LOG.Info("Enter alias of remote host and message you want to send.\nInput format: <<port_serial_no>> [space] <<message>>");

            string input = Console.ReadLine();
            string[] parts = input.Split(' ', 2);

            return (long.Parse(parts[0]), parts[1]);
        }
    }
}

