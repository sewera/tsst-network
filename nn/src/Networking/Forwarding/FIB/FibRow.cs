using System.Collections.Generic;


namespace nn.Networking.Forwarding.FIB
{
    /// <summary>
    /// Class representing single row in MPLS-FIB
    /// </summary>
    class FibRow
    {
        private int _inLink=0;
        private int _inLabel=0;
        private int _outLink=0;
        private int _outLabel=0;
        private bool _isNextLabel=false;
        private int _nextLabel {get; set;}

        /// <summary>
        /// Class constructor from strig
        /// <param name="CommandData"> String being CommandData from ManagementSystem <param>
        /// </summary>

        public FibRow(string CommandData)
        {
            List<string> words = new List<string>(CommandData.Split(' '));
            // Assert there will be no "" elements (which happens sometimes after Split)
            words.RemoveAll(item => item == "");

            _inLink = int.Parse(words[0]);
            _inLabel = int.Parse(words[1]);
            // Tunel can terminate
            _outLink = (words[2]==".") ? 0 : int.Parse(words[2]);
            _outLabel = (words[3]==".") ? 0 : int.Parse(words[3]);

            //If command has more than 4 elements it means that row will contain nextLabel field
            if (words.Count > 4)
            {
                _isNextLabel = true;
                _nextLabel = int.Parse(words[4]);
            }
        }
        /// <summary>
        /// Check if the first part of row match given params
        /// <returns>True if inLink and inLabel match given params </returns>
        /// </summary>
        public bool FirstPartMatch(int inLink, int inLabel)
        {
            if (_inLink==inLink && _inLabel == inLabel)
            {
                return true;
            }
            return false;
            // Is that readable code?
        }
        /// <summary>
        /// Get second part of row
        /// <returns>_outLink,_outLabel,_isNextLabel,_nextLabel </returns>
        /// </summary>
        public (int,int,bool,int) SecondPartGet()
        {
            return (_outLink,_outLabel,_isNextLabel,_nextLabel);
        }

        public override string ToString()
        {
            return $"InLink: {_inLink},InLabel: {_inLabel},OutLink: {_outLink},OutLabel: {_outLabel},isNextLabel: {_isNextLabel.ToString()},NextLabel: {_nextLabel}";
        }

         public static bool operator ==(FibRow a, FibRow b)
         {
             return (a._inLink == b._inLink)           &&
                    (a._inLabel == b._inLabel)         &&
                    (a._outLink == b._outLink)         &&
                    (a._outLabel == b._outLabel)       &&
                    (a._isNextLabel == b._isNextLabel) &&
                    (a._nextLabel == b._nextLabel);
         }

         public static bool operator !=(FibRow a, FibRow b)
         {
             return !(a==b);
         }

    }
}