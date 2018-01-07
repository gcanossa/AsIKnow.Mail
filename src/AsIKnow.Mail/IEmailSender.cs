using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsIKnow.Mail
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string message, string from);
        Task SendEmailAsync(string to, string subject, string message);
    }
}
