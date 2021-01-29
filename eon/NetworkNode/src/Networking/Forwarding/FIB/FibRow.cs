using System.Collections.Generic;
using Common.Utils;

namespace NetworkNode.Networking.Forwarding.FIB
{
    /// <summary>
    /// Class representing single row in MPLS-FIB
    /// </summary>
    class FibRow
    {
        private string _inPort;
        private int _lowerSlotsValue;
        private int _upperSlotsValue;
        private string _outPort;
        
        /// <summary>
        /// Class constructor from strig
        /// <param name="CommandData"> String being CommandData from ManagementSystem </param>
        /// </summary>
        public FibRow(string CommandData)
        {
            List<string> words = new List<string>(CommandData.Split(' '));
            // Assert there will be no "" elements (which happens sometimes after Split)
            words.RemoveAll(item => item == "");

            _inPort = words[0];
            _lowerSlotsValue = int.Parse(words[1]);
            
            _upperSlotsValue = int.Parse(words[2]);
            _outPort = words[3];
        }
        /// <summary>
        /// Check if the first part of row match given params
        /// <returns>True if inLink and inLabel match given params </returns>
        /// </summary>
        public bool FirstPartMatch(string inPort, (int, int) slots)
        {
            if (Checkers.PortMatches(_inPort, inPort) && slots == (_lowerSlotsValue, _upperSlotsValue))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get second part of row
        /// <returns>_outLink,_outLabel,_isNextLabel,_nextLabel </returns>
        /// </summary>
        public string SecondPartGet()
        {
            return _outPort;
        }

        public override string ToString()
        {
            return $"inPort: {_outPort}, slots = ({_lowerSlotsValue}, {_upperSlotsValue}, outPort: {_outPort}";
        }

         public static bool operator ==(FibRow a, FibRow b)
         {
            return  (a._inPort == b._inPort) &&
                    (a._lowerSlotsValue == b._lowerSlotsValue) &&
                    (a._upperSlotsValue == b._upperSlotsValue) &&
                    (a._outPort == b._outPort);
         }

         public static bool operator !=(FibRow a, FibRow b)
         {
             return !(a==b);
         }
    }
}
