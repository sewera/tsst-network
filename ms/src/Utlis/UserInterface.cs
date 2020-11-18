using System;
using System.Collections.Generic;

namespace ms
{
    /// <summary>
    /// Class handling user input (checking its correctness, processing it, sending further) and console output
    /// </summary>
    static class UserInterface
    {
        /// <summary>
        /// Represents type of the output shwon on the screen
        /// </summary>
        public enum Type{
            Server,Received,Console,Syntax,Help
        }


        /// <summary>
        /// Run the UserInterface
        /// </summary>
        public static void Start()
        {
            string input="";
            while (true)
            {
                Console.ForegroundColor=ConsoleColor.Yellow;
                input = Console.ReadLine();
                Console.ResetColor();
                if (input == "clear")
                {
                    Console.Clear();
                }
                else if (input == "help")
                {
                    ShowHelp();
                }
                else if (input == "exit")
                {
                    MessageSender.Quit();
                    break;
                }
                else
                {
                     if (CheckSyntax(input))
                     {
                        SendMessage(new Message(ProcessCommand(input)));
                     }
                }
            }
        }
        /// <summary>
        /// Send message to ManagementSystem
        /// <param name="message"> Message to be sent to ManagementManager <param>
        /// </summary>
        private static void SendMessage(Message message)
        {
            if (ClientController.FindAlias(message.clientAlias))
            {
                MessageSender.AddMessage(message);
            }
            else
            {
                WriteLine($"Error finding client!\nThere is no client with alias {message.clientAlias}",Type.Syntax);
            }
        }
        /// <summary>
        /// Show output on console
        /// <param name="output"> String to be shown on the screen <param>
        /// </summary>
        public static void WriteLine(string output, Type type)
        {
            switch(type)
            {
                case Type.Server:
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                }
                case Type.Received:
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                }
                case Type.Console:
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                case Type.Syntax:
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                }
                case Type.Help:
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                }
            }

            Console.WriteLine(output);
            Console.ResetColor();
        }
        /// <summary>
        /// Check syntax of Management protocol command. Syntax is described in docs/stages/Management_Protocol_Specification.md
        /// <param name="Input">  String eneterd from console <param>
        /// </summary>
        private static bool CheckSyntax(string input)
        {
            List<string> words = new List<string>(input.Split(' '));

            // This is the value method returns, it should pass lots of tests.
            // Method could simply return false in every if statement, but this way we can inform user about errors in his command
            bool result=true;

            
            // There is an indexing range issue with inputs containg invisible elements ("")
            words.RemoveAll(item => item == "");

            //TODO now if you allow command like that to pass remember to process it the same way before sending to network node

            if (words.Count<6)
            {
                WriteLine("Syntax error!\nCommand too short",Type.Syntax);
                return false;
            }
            // First word should be router alias
            if (words[0][0] != 'R')
            {
                WriteLine("Syntax error!\nFirst param should be network node alias",Type.Syntax);
                result = false;
            }
            // The rest of the first should be all numbers
            for (int i=1; i<words[0].Length; i++)
            {
                if(!Char.IsDigit(words[0],i))
                {
                    WriteLine("Syntax error!\nFirst param should be network node alias",Type.Syntax);
                    result = false;
                }
            }

            // Second word can only be 'add' or 'delete'
            if (!(words[1] == "add" || words[1] == "delete"))
            {
                WriteLine("Syntax error!\nOnly keywords are 'add' and 'delete'",Type.Syntax);
                result = false;
            }

            // The next four params can only be numbers or dots
            for (int i=2;i<6;i++)
            {
                int arg;
                if ((!(int.TryParse(words[i], out arg) && (arg>0) ) || words[i] =="."))
                {
                    WriteLine("Syntax error!\nLinks and labels are expressed in positive numbers or dots",Type.Syntax);
                    result = false;
                }
            }


            // The next parameter is optional so we need to check if it exists
            if (words.Count == 7)
            {
                if (words[6].Length < 2)
                {
                    WriteLine("Syntax error!\nLast param is too short",Type.Syntax);
                     result = false;
                }

                if (words[6][0] != '-')
                {
                    WriteLine("Syntax error!\nLast param should start with '-'",Type.Syntax);
                    result = false;
                }
                for (int i=1; i<words[6].Length;i++)
                {
                    if(!Char.IsDigit(words[6],i))
                    {
                        WriteLine("Syntax error!\nLast param should end with a number",Type.Syntax);
                    }
                }
            }

            if (words.Count > 7)
            {
                WriteLine("Syntax error!\nToo much params",Type.Syntax);
                result = false;
            }

            if (result != true)
            {
                WriteLine("Type 'help' for help",Type.Syntax);
            }

            return result;
        }
        /// <summary>
        /// Process command to match management protocol
        /// </summary>
        private static string ProcessCommand(string input)
        {
            List<string> words = new List<string>(input.Split(' '));
            words.RemoveAll(item => item == "");
            input="";
            for(int i=0;i<words.Count-1;i++)
            {
                input+=$"{words[i]} ";
            }
            // Last element does not containt a ' ' after him
            input+=words[words.Count-1];
            // Here input can only be this format e.g. 'R1 add 2 3 4 5 -3' or 'R1 add 2 3 4 5'.
            // No spaces at the end or beginning and only one space between params.
            // TODO use StringBuilder here
            return input;
        }

        /// <summary>
        /// Show help for user
        /// </summary>
        private static void ShowHelp()
        {
            // TODO
            WriteLine("Help: ///TODO",Type.Help);
        }
    }
}