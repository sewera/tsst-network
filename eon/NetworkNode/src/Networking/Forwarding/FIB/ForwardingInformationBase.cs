using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Models;

namespace NetworkNode.Networking.Forwarding.FIB
{
    /// <summary>
    /// Class representing MPLS-Table
    /// </summary>
    class ForwardingInformationBase
    {
        /// <summary>
        /// List of rows that Table contains
        /// </summary>
        private List<FibRow> rows;


        /// <summary>
        /// Class contructor
        /// </summary>
        public ForwardingInformationBase()
        {
            rows = new List<FibRow>();
        }


        /// <summary>
        /// Add row to the table
        /// <param name="CommandData"> String being CommandData from ManagementSystem, representing row to add<param>
        /// </summary>
        public void AddRow(string CommandData)
        {
            rows.Add( new FibRow(CommandData));
        }


        /// <summary>
        /// Delete row from the table
        /// <param name="CommandData"> String being CommandData from Management System, indicating which row to delete<param>
        /// </summary>
        public void DeleteRow(string CommandData)
        {
            FibRow predicate = new FibRow(CommandData);
            rows.RemoveAll(item => item==predicate);
        }
        /// <summary>
        /// Show Mpls-Table
        /// </summary>
        public void ShowTable()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-----------------------------------");
            foreach (var v in rows)
            {
                Console.WriteLine(v.ToString());
            }
            Console.ResetColor();
        }


        /// <summary>
        /// Search Mpls-Table in order to find outLink and new label stack for incoming packet
        /// <param name="portAlias"> Port where the packet came<param>
        /// <param name="packet"> incoming MplsPacket<param>
        /// <returns>The packet to send and its output port</returns>
        /// </summary>
        public (string, EonPacket) Commutate((string portAlias, EonPacket packet) forwardPacketTuple)
        {
            string resultPort = "";
            
            // The Port where the packet came
            string inPort =forwardPacketTuple.portAlias;
            // The incoming packet
            EonPacket inPacket = forwardPacketTuple.packet;
            
            // Search rows for matching inPort and sluts
            bool isFound=false;
            foreach (FibRow row in rows)
            {
                if(row.FirstPartMatch(inPort, inPacket.Slots))
                {
                    resultPort = row.SecondPartGet();
                    isFound=true;
                }
            }

            if(isFound!=true)
            {
                // If we couldnt find any information to forward this packet
                throw new NoForwardingInformationFound("No forwarding information found for given packet");
                // Method catching this exception should print that packet is lost
            }
            
            return (resultPort, inPacket);
        }
    }

    public class NoForwardingInformationFound : Exception
    {
        public NoForwardingInformationFound() { }
        public NoForwardingInformationFound(string message) : base(message) { }
        public NoForwardingInformationFound(string message, Exception inner) : base(message, inner) { }
        protected NoForwardingInformationFound(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
