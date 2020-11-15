namespace Cc.Config
{
    public class Configuration
    {
        public short listeningPort { get; set; }

        private Configuration(short listeningPort)
        {
            this.listeningPort = listeningPort;
        }

        public class Builder
        {
            public Builder SetListeningPort(short listeningPort)
            {
                this.listeningPort = listeningPort;
                return this;
            }

            public Configuration Build()
            {
                return new Configuration(listeningPort);
            }

            private short listeningPort;
        }
    }
}
