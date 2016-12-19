using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LurenTech.Configuration
{
    public class SmtpConnection
    {
        public string EmailFrom { get; set; }
        public string EmailFromPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string EmailFromName { get; set; }
    }
}
