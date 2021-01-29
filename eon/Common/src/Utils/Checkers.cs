using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Utils
{
    public class Checkers
    {
        public static int PortMatches(string pattern, string port)
        {
            int matches = -1;
            foreach ((char patternChar, char portChar) in pattern.Zip(port))
            {
                if (patternChar == 'x') // if x then set matches to 0 indicating that port matches, but with the lowest priority
                {
                    if (matches == -1)
                        matches = 0;
                }
                else if (patternChar != portChar) // if patternChar is not x and it doesn't match the portChar then the whole pattern doesn't match
                {
                    return -1;
                }
                else
                {
                    if (matches == -1)
                        matches = 1;
                    else
                        matches++; // else the pattern matches, so add 1 to the priority
                }
            }

            return matches;
        }

        public static bool MultipleGatewaysInRibRow(string rowGateway)
        {
            Regex regex = new Regex("^[0-9]{3},[0-9]{3}$");
            return regex.IsMatch(rowGateway);
        }

        public static bool SlotsOverlap((int, int) slots1, (int, int) slots2)
        {
            (int lower1, int upper1) = slots1;
            (int lower2, int upper2) = slots2;

            return !((lower2 > upper1) && (upper2 > upper1) || (lower1 > upper1) && (upper1 > upper2));
        }
    }
}
