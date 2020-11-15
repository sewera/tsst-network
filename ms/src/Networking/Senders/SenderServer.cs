using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

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
        /// Send data as string
        /// <param name="data"> String to be sent </param>
        /// <summary>
        public void Send(string data)
        {
            try
            {
                /* what happens here:
                1. Create a list of bytes
                2. Add the length of the string to the list.
                So if this message arrives at the server we can easily read the length of the
                coming message.
                3. Add the message(string) bytes
                */
                // 1.
                var fullPacket = new List<byte>();
                // 2.
                fullPacket.AddRange(BitConverter.GetBytes(data.Length));
                // 3.
                fullPacket.AddRange(Encoding.Default.GetBytes(data));
                /* Send the message to the server we are currently connected to.
                Or package stucture is {length of data 4 bytes (int32), actual data}*/
                _sendSocked.Send(fullPacket.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception($"Sending error {ex}");
            }
        }
    }
}
