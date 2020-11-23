namespace nn.src.Ui.Parsers
{
    public interface ICommandParser
    {
        (string, string) ParseCommand(string command);
    }
}
