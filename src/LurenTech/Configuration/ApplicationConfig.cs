using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LurenTech.Configuration
{
    public class ApplicationConfig
    {
        private static ApplicationConfig _instance = null;

        private static IConfigurationRoot _config;

        private ApplicationConfig()
        {

        }

        public static void Initialize(IConfigurationRoot config)
        {
            _instance = new ApplicationConfig();
            _config = config;
        }

        public static ApplicationConfig Configuration
        {
            get
            {
                return _instance;
            }
        }

        public string GetSetting(string key)
        {
            return _config.GetSection(key).Value;
        }
    }
}
