using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsIKnow.Mail
{
    public class MailOptions
    {
        public string ViewTemplateBasePath { get; set; } = "~/Views/MailTemplates";
        public string SmtpServer { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string NoreplayAddress { get; set; }
    }
}
