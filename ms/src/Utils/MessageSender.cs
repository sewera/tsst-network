using System.Collections.Generic;
using ms.Config;
using ms.Models;
using ms.Networking.Controllers;

namespace ms.Utils
{
    /// <summary>
    /// Class handling messages sending
    /// </summary>
    static class MessageSender
    {
        /// <summary>
        /// Queue with messages
        /// </summary>
        private static Queue<Message> messages = new Queue<Message>();

        /// <summary>
        /// Read config file, actually config messages
        /// </summary>
        public static void ReadConfig(Configuration configuration)
        {
            foreach (Message m in configuration.ConfigMessages)
            {
                messages.Enqueue(m);
            }
        }

        /// <summary>
        /// Add message to Queue
        /// </summary>
        public static void AddMessage(Message message)
        {
            messages.Enqueue(message);
        }

        /// <summary>
        /// Start sending messages
        /// </summary>
        public static void SendConfigCommands()
        {
            while (messages.Count > 0)
            {
                Message m = messages.Dequeue();
                if (!ClientController.SendData(m._content, m.clientAlias))
                {
                    messages.Enqueue(m);
                }
            }
        }
    }
}
