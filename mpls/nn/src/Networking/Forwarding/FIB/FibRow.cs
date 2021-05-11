using System;
using System.Collections.Generic;


namespace nn.Networking.Forwarding.FIB
{
    /// <summary>
    /// Class representing single row in MPLS-FIB
    /// </summary>
    internal class FibRow
    {
        private readonly int _id = 0;
        private readonly int _inLink = 0;
        private readonly int _inLabel = 0;
        private readonly int _outLink = 0;
        private readonly int _outLabel = 0;
        private readonly int _5th = 0;
        private readonly bool _isNextLabel = false;
        private readonly int _nextLabel;

        /// <summary>
        /// Class constructor from string
        /// <param name="commandData"> String being CommandData from ManagementSystem </param>
        /// </summary>
        public FibRow(string commandData)
        {
            List<string> words = new List<string>(commandData.Split(' '));
            // Assert there will be no "" elements (which happens sometimes after Split)
            words.RemoveAll(item => item == "");

            _id = int.Parse(words[0]);
            _inLink = int.Parse(words[1]);
            _inLabel = int.Parse(words[2]);
            // Tunnel can terminate
            _outLink = (words[3]==".") ? 0 : int.Parse(words[2]);
            _outLabel = (words[4]==".") ? 0 : int.Parse(words[3]);

            _5th = int.Parse(words[5]);

            //If command has more than 6 elements it means that row will contain nextLabel field
            if (words.Count <= 6) return;
            _isNextLabel = true;
            _nextLabel = -int.Parse(words[6]);
        }
        /// <summary>
        /// Check if the first part of row match given params
        /// <returns>True if inLink and inLabel match given params </returns>
        /// </summary>
        public bool FirstPartMatch(int inLink, int inLabel, int lastRowId)
        {
            if (_inLink==inLink && _inLabel == inLabel && _5th == lastRowId)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get second part of row
        /// <returns>_outLink,_outLabel,_isNextLabel,_nextLabel </returns>
        /// </summary>
        public (int,int,bool,int) SecondPartGet()
        {
            return (_outLink,_outLabel,_isNextLabel,_nextLabel);
        }
        /// <summary>
        /// Get value of 5th column
        /// <returns> _5th field</returns>
        /// </summary>
        public int get_5th()
        {
            return _5th;
        }

        public override string ToString()
        {
            return $"InLink: {_inLink},InLabel: {_inLabel},OutLink: {_outLink},OutLabel: {_outLabel},isNextLabel: {_isNextLabel.ToString()},NextLabel: {_nextLabel}, 5th: {_5th}";
        }

         public static bool operator ==(FibRow a, FibRow b)
         {
             return b is { } && a is { } && a._inLink == b._inLink && (a._inLabel == b._inLabel) && (a._outLink == b._outLink) && (a._outLabel == b._outLabel) && (a._5th == b._5th) && (a._isNextLabel == b._isNextLabel) && (a._nextLabel == b._nextLabel);
         }

         public static bool operator !=(FibRow a, FibRow b)
         {
             return !(a==b);
         }
         
         private bool Equals(FibRow other)
         {
             return _id == other._id && _inLink == other._inLink && _inLabel == other._inLabel && _outLink == other._outLink && _outLabel == other._outLabel && _5th == other._5th && _isNextLabel == other._isNextLabel && _nextLabel == other._nextLabel;
         }

         public override bool Equals(object obj)
         {
             if (ReferenceEquals(null, obj)) return false;
             if (ReferenceEquals(this, obj)) return true;
             if (obj.GetType() != this.GetType()) return false;
             return Equals((FibRow) obj);
         }

         public override int GetHashCode()
         {
             return HashCode.Combine(_id, _inLink, _inLabel, _outLink, _outLabel, _5th, _isNextLabel, _nextLabel);
         }

    }
}
