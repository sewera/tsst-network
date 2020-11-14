using System;
using System.Net;

namespace cn.Utils
{
    class UserInterface : IUserInterface
    {
        /// <summary>
        /// IP address of message receiver
        /// </summary>
        public IPAddress destinationAddress { get; set; }

        /// <summary>
        /// User's input message
        /// </summary>
        public string message { get; set; }

        public UserInterface()
        {

        }

        public void EnterDestAddressAndMessage()
        {
            Console.WriteLine("Enter address of remote host and message you want to send.\nInput format: <<address>> [space] <<message>>");

            string input = Console.ReadLine();
            string[] parts = input.Split(' ', 2);

            try
            {
                destinationAddress = IPAddress.Parse(parts[0]);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException caught!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine("FormatException caught!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            message = parts[1];
        }
    }
}

