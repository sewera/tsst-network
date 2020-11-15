namespace Cc.Config
{
    internal class Configuration
    {
        public string listeningPort { get; set; }

        private Configuration(string listeningPort)
        {
            this.listeningPort = listeningPort;
        }

        public class Builder
        {
            public Builder SetListeningPort(string listeningPort)
            {
                this.listeningPort = listeningPort;
                return this;
            }

            public Configuration Build()
            {
                return new Configuration(listeningPort);
            }

            private string listeningPort;
        }
    }
}
