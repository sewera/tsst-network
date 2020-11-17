using System;
using System.Collections.Generic;

namespace ms
{
    /// <summary>
    /// Class handling messages sending
    /// </summary>
    static class MessageSender
    {
        /// <summary>
        /// Queue with messages
        /// </summary>
        private static Queue<Message> messages= new Queue<Message>();

        /// <summary>
        /// Auxiliary field to determine if Start method should break its while loop
        /// </summary>
        private static bool quit = false;

        /// <summary>
        /// Read config file, actually config messages
        /// </summary>
        public static void ReadConfig(Config config)
        {
            foreach(Message m in config.configMessages)
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
        /// Quit while loop in Start method
        /// </summary>
        public static void Quit()
        {
            quit = true;
        }

        /// <summary>
        /// Start sending messages
        /// </summary>
        public static void Start()
        {
            while(quit==false)
            {
                if(messages.Count > 0)
                {
                    Message m = messages.Dequeue();
                    if(!(ClientController.SendData(m._content,m.clientAlias)))
                    {
                        messages.Enqueue(m);
                    }
                }
            }
        }
    }
}
