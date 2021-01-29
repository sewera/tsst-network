using System.Text.RegularExpressions;

namespace Common.Utils
{
    public class Checkers
    {
        public static bool PortMatches(string pattern, string port)
        {
            string rPattern = Regex.Replace(pattern, "x", "[0-9]");
            return Regex.IsMatch(port, $"^{rPattern}$");
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
