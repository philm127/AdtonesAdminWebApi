using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services.Mailer
{
    public interface ISendEmailMailer
    {
        void SendEmail(SendEmailModel mail);
    }
}
