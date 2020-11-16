namespace Cc.Config
{
    public class Configuration
    {
        public short ListeningPort { get; set; }

        private Configuration(short listeningPort)
        {
            ListeningPort = listeningPort;
        }

        public class Builder
        {
            public Builder SetListeningPort(short listeningPort)
            {
                _listeningPort = listeningPort;
                return this;
            }

            public Configuration Build()
            {
                return new Configuration(_listeningPort);
            }

            private short _listeningPort;
        }
    }
}
