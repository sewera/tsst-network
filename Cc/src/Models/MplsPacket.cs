using System.Collections.Generic;

namespace Cc.Models
{
    internal class MplsPacket
    {
        public IList<long> Labels { get; set; }
        public string Data { get; set; }
    }
}
