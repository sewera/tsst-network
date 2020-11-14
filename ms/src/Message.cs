using System;

namespace ms
{
    /// <summary>
    /// Class representing a message that can be sent from UserInterface to ManagementManager
    /// </summary>
    class Message
    {
        /// <summary>
        /// Id of the client who will receive the message
        /// </summary>
        public int _id;
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
            // Message needs to be inputted in specified format e.g '0 hello' where '0' is router id, 'hello' is the message conten
            string[] words = input.Split(' ');
            _id = Int32.Parse(words[0]);

            for (int i = 1; i < words.Length; i++)
            {
                _content += $"{words[i]} ";
            }
            //TODO fix this code
        }
    }
}