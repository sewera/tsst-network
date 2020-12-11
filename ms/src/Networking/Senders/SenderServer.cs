using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace ms
{
    /// <summary>
    /// Sender Server for each client
    /// <summary>
    public class SenderServer
    {
        /// <summary>
        /// Client Socket
        /// <summary>
        private Socket _sendSocked;
        public SenderServer(Socket sendSocket)
        {
            _sendSocked = sendSocket;
        }
        /// <summary>
        /// Send data as ManagementPacket 
        /// <param name="data"> String to be sent </param>
        /// <summary>
        public void Send(string data)
        {
            List<string> words = new List<string>(data.Split(' '));
            words.RemoveAll(item => item == "");
            string commandType = words[0];
            words.RemoveAt(0);
            string commandData = string.Join(' ', words);

            try
            {
                // Create ManagementPacket
                ManagementPacket packet = new ManagementPacket.Builder()
                        .SetCommandType(commandType)
                        .SetCommandData(commandData) 
                        .Build();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"{packet.ToString()}");
                    Console.ResetColor();
                    Thread.Sleep(100);
                    _sendSocked.Send(packet.ToBytes());
            }
            catch (Exception e)
            {
                throw new Exception($"Sending error {e}");
            }
        }
    }
}
