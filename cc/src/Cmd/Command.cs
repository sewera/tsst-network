namespace cc.Cmd
{
    public enum CommandType
    {
        SEND
    }
    public class Command
    {
        public CommandType CommandType { get; set; }

        public Command(CommandType commandType)
        {
            CommandType = commandType;
        }
    }
}
