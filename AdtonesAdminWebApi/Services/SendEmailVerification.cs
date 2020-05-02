using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public class SendEmailVerification
    {
        //private void SendEmailVerificationCode(string firstName, string LastName, string email, string password)
        //{
        //    var reader = new StreamReader(
        //                           Server.MapPath(ConfigurationManager.AppSettings["OperatorAdminRegistrationEmailTemplete"]));
        //    var url = ConfigurationManager.AppSettings["OperatorAdminUrl"];
        //    string emailContent = reader.ReadToEnd();

        //    emailContent = string.Format(emailContent, firstName, LastName, url, email, password);

        //    MailMessage mail = new MailMessage();
        //    mail.To.Add(email);
        //    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
        //    mail.Subject = "Operator Registration";
        //    mail.Body = emailContent.Replace("\n", "<br/>");

        //    mail.IsBodyHtml = true;
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
        //    smtp.Credentials = new System.Net.NetworkCredential
        //         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
        //    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

        //    //Or your Smtp Email ID and Password
        //    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
        //    smtp.Send(mail);
        //}

    }
}
