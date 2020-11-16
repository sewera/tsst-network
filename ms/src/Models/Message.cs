using System;

namespace ms
{
    /// <summary>
    /// Class representing a message that can be sent from UserInterface to ManagementManager
    /// </summary>
    class Message
    {
        /// <summary>
        /// Network Node alias of the client who will receive the message
        /// </summary>
        public string clientAlias;
        /// <summary>
        /// Message content
        /// </summary>
        public string _content="";
        /// <summary>
        /// Class contructor from string
        /// <param name="input"> String from which the message will be created </param>
        /// </summary>
        public Message(string input)
        {
            // Message needs to be inputted in specified format e.g 'R1 content' where 'R1' is router aliast, 'content' is the message content
            string[] words = input.Split(' ');
            clientAlias = words[0];

            for (int i = 1; i < words.Length; i++)
            {
                _content += $"{words[i]} ";
            }
            //TODO fix this code
        }
    }
}