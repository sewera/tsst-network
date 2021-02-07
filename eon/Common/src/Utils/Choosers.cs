using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Utils
{
    public class Choosers
    {
        public static string GetGatewayFromRibRow(string rowGateway)
        {
            if (!rowGateway.Contains(','))
                return rowGateway;

            Random random = new Random();
            string[] possibleGateways = rowGateway.Split(",");
            return possibleGateways[random.Next(possibleGateways.Length)];
        }
    }
}
