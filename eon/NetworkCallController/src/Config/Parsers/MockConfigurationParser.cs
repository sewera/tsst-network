using Common.Config.Parsers;

namespace NetworkCallController.Config.Parsers
{
    public class MockConfigurationParser : IConfigurationParser<Configuration>
    {
        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetCallCoordinationPort(6001)
                .SetCallTeardownPort(6002)
                .SetConnectionRequestPort(6003)
                .Build();
        }
    }
}
