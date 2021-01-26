namespace Common.Config.Parsers
{
    public interface IConfigurationParser<out TConfiguration>
    {
        TConfiguration ParseConfiguration();
    }
}
