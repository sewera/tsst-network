using System.Collections.Generic;

namespace cc.Ui.Parsers
{
    public interface ICommandParser
    {
        (string, string, bool) ParseCommand(string command);
    }
}
