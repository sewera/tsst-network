namespace Cc.Cmd.Parsers
{
    public interface ICommandParser
    {
        Command ParseCommand(string input);
    }
}
