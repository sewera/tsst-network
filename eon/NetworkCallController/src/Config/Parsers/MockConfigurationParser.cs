using Common.Config.Parsers;

namespace NetworkCallController.Config.Parsers
{
    public class MockConfigurationParser : IConfigurationParser<Configuration>
    {
        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetCallCoordinationLocalPort(6001)
                .SetCallTeardownLocalPort(6002)
                .SetConnectionRequestLocalPort(6003)
                .Build();
        }
    }
}
