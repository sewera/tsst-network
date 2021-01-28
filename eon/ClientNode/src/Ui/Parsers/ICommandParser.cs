using System.Collections.Generic;

namespace ClientNode.Ui.Parsers
{
    public interface ICommandParser
    {
        (string, (int, int)) ParseCpccCommand(string command);

        (string, string) ParseCommand(string command);

        (List<long>, string) CheckMplsOutLabel(string mplsOutLabel);
    }
}
