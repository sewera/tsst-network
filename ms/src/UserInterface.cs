using System;

namespace ms
{
    /// <summary>
    /// Class handling user input and console output
    /// </summary>
    static class UserInterface
    {
        /// <summary>
        /// Run the UserInterface
        /// </summary>
        public static void Start()
        {
            while (true)
            {
                Console.Write("# ");
                string input = Console.ReadLine();

                SendMessage(new Message(input));
            }
        }
        /// <summary>
        /// Send message to ManagementSystem
        /// <param name="message"> Message to be sent to ManagementManager <param>
        /// </summary>
        private static void SendMessage(Message message)
        {
            ClientController.SendData(message._content, message._id);
        }
        /// <summary>
        /// Show output on console
        /// <param name="output"> String to be shown on the screen <param>
        /// </summary>
        public static void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
    }
}