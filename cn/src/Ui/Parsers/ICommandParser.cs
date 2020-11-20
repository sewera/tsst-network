using cn.Models;

namespace cn.Ui.Parsers
{
    public interface ICommandParser
    {
        (string, string) ParseCommand(string command);
    }
}
