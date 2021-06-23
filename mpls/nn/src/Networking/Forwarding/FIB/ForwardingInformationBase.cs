using System.Collections.Generic;
using nn.Models;
using System;

namespace nn.Networking.Forwarding.FIB
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
        /// Class constructor
        /// </summary>
        public ForwardingInformationBase()
        {
            rows = new List<FibRow>();
        }


        /// <summary>
        /// Add row to the table
        /// <param name="commandData"> String being CommandData from ManagementSystem, representing row to add</param>
        /// </summary>
        public void AddRow(string commandData)
        {
            rows.Add( new FibRow(commandData));
        }


        /// <summary>
        /// Delete row from the table
        /// <param name="commandData"> String being CommandData from Management System, indicating which row to delete<param>
        /// </summary>
        public void DeleteRow(string commandData)
        {
            FibRow predicate = new FibRow(commandData);
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
        /// <param name="portAlias"> Port where the packet came</param>
        /// <param name="packet"> incoming MplsPacket</param>
        /// <returns>The packet to send and its output port</returns>
        /// </summary>
        public (string, MplsPacket) Commutate((string portAlias, MplsPacket packet) forwardPacketTuple, int lastRowId)
        {
            string resultPort = "";
            MplsPacket resultPacket = new MplsPacket();

            // The Port where the packet came
            int inPort = int.Parse(forwardPacketTuple.portAlias);
            // The incoming packet
            MplsPacket inPacket = forwardPacketTuple.packet;
            //Popping the label from the incoming packet
            int inLabel = inPacket.PopLabel();


            // Search rows for matching inPort and inLabel
            bool isFound=false;
            foreach (FibRow row in rows)
            {
                if(row.FirstPartMatch(inPort, inLabel, lastRowId))
                {
                    (int outLink,int outLabel,bool isNextLabel,int nextLabel) = row.SecondPartGet();
                    isFound=true;

                    if(outLink == 0 && outLabel == 0)
                    {
                        // It means that tunel terminates here, need the recursion to be done
                        // If the tunnel terminates here, it means that MplsPacket has at least one label remaining
                        (resultPort, resultPacket) = Commutate((inPort.ToString(), inPacket), row.get_id());
                    }
                    else if (isNextLabel == true)
                    {
                        resultPort = outLink.ToString();
                        // Remember that inPacket has its label popped
                        resultPacket=inPacket; 
                        // The pushed label here is the same level label as the previously popped one
                        resultPacket.PushLabel(outLabel);
                        // Now push next level label
                        resultPacket.PushLabel(nextLabel);
                    }
                    else
                    {
                        // The most common case
                        resultPort = outLink.ToString();
                        resultPacket=inPacket;
                        // Now we push new label, (pop and push is the same as change)
                        resultPacket.PushLabel(outLabel);
                        // There is some redundancy in two last cases, but i want to clearly mark the diff between them in comments.
                    }
                }
            }

            if(isFound!=true)
            {
                // If we couldnt find any information to forward this packet
                throw new NoForwardingInformationFound("No forwarding information found for given packet");
                // Method catching this exception should print that packet is lost
            }
            
            return (resultPort,resultPacket);
        }
    }

    public class NoForwardingInformationFound : System.Exception
    {
        public NoForwardingInformationFound() { }
        public NoForwardingInformationFound(string message) : base(message) { }
        public NoForwardingInformationFound(string message, System.Exception inner) : base(message, inner) { }
        protected NoForwardingInformationFound(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
