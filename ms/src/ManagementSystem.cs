using System;

namespace ms
{
    class ManagementSystem
    {
        static void Main(string[] args)
        {
            IManagementManager mm = new ManagementManager();
            mm.startListening();
            while(true)
            {
                string input = Console.ReadLine();

                string[] words = input.Split(' ');
                int id = Int32.Parse(words[0]);
                string data="";

                for (int i = 1; i < words.Length; i++)
                {
                    data += $"{words[i]} ";
                }

                ClientController.SendData(data,id);
            }
        }
    }
}
