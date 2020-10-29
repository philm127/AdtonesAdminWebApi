using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services.Mailer
{
    public interface ISendEmailMailer
    {
        Task SendBasicEmail(SendEmailModel mail, int operatorId = 0, int roleId = 0);
    }
}
