using System.Collections.Generic;

namespace cn.Ui.Parsers
{
    public interface ICommandParser
    {
        (string, string) ParseCommand(string command);

        (List<long>, string) CheckMplsOutLabel(string mplsOutLabel);
    }
}
