using System.Collections.Generic;

namespace RoutingController.Model
{
    public class Link
    {
        public string PortAlias1 { get; }
        public string PortAlias2 { get; }
        public List<(int, int)> SlotsArray { get; set; }

        public Link(string portAlias1, string portAlias2, List<(int, int)> slotsArray)
        {
            PortAlias1 = portAlias1;
            PortAlias2 = portAlias2;
            SlotsArray = slotsArray;
        }
    }
}
