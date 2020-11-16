using System;

namespace ms
{
    public interface IConfig
    {
        /// <summary> 
        /// Read configuration file and load Listener port number and pending messages
        /// <param name="configFile">Input file</param>
        /// <returns>Configuration class instance with read data</returns>
        /// </summary>
        public Config ReadConfigFile(string configFile);
    }
}