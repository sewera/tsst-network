namespace ms.Config.Parsers
{
    public class MockConfigurationParser : IConfigurationParser
    {
        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetPort(4001)
                .AddConfigMessage(new Message("R1", "add 12 101 14 141"))
                .AddConfigMessage(new Message("R1", "add 12 102 13 131 -1001"))
                .AddConfigMessage(new Message("R1", "add 12 103 13 132 -1001"))
                .AddConfigMessage(new Message("R1", "add 13 1002 . ."))
                .AddConfigMessage(new Message("R1", "add 13 521 12 122"))
                .AddConfigMessage(new Message("R1", "add 13 522 12 123"))
                .AddConfigMessage(new Message("R1", "add 14 221 12 121"))
                .Build();
        }
    }
}
