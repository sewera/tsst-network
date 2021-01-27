using System;
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

        public static bool SlotsOverlap((int, int) slots1, (int, int) slots2)
        {
            (int lower1, int upper1) = slots1;
            (int lower2, int upper2) = slots2;

            throw new NotImplementedException();
        }
    }
}
