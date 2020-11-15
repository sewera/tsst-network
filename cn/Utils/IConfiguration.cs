using System.Collections.Generic;

namespace cn.Utils
{
    interface IConfiguration
    {
        /// <summary>Read configuration file and 
        /// load set of data about network elements</summary>
        /// <param name="configFile">File with network data
        /// saved in defined convention</param>
        /// <returns>Configuration class instance with read
        /// data</returns>
        public Configuration ReadConfigFile(string configFile);
    }
}
