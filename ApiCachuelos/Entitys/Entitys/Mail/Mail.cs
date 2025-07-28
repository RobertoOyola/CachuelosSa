using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.Entitys.Mail
{
    public class MailInfo
    {
        public string Mail { get; set; }
    }

    public class MailReturn
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
    }

    public class SmtpConfig
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
