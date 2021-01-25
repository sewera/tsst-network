namespace Common.Config.Parsers
{
    public interface IConfigurationParser<C>
    {
        C ParseConfiguration();
    }
}
