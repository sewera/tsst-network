using Common.Config.Parsers;

namespace ConnectionController.Config.Parsers
{
    public class MockConfigurationParser : IConfigurationParser<Configuration>
    {
        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetConnectionRequestLocalPort(6021)
                .SetPeerCoordinationLocalPort(6022)
                .Build();
        }
    }
}
