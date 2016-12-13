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

        public SmtpConnection GetSmtpConnectionDetails()
        {
            SmtpConnection smtp = new SmtpConnection();

            var smtpSettings = _config.GetSection("SmtpClient");

            smtp.EmailFrom = smtpSettings["EmailFrom"];
            smtp.EmailFromPassword = smtpSettings["EmailFromPassword"];
            smtp.Host = smtpSettings["Host"];

            if (!string.IsNullOrEmpty(smtpSettings["EmailFromName"]))
                smtp.EmailFromName = smtpSettings["EmailFromName"];

            if (!string.IsNullOrEmpty(smtpSettings["Port"]))
                smtp.Port = int.Parse(smtpSettings["Port"]);

            if (!string.IsNullOrEmpty(smtpSettings["EnableSsl"]))
                smtp.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

            return smtp;
        }
    }
}
