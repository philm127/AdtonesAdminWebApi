using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services.Mailer
{
    /// <summary>
    /// TODO: Need to sort whole of the mailing out
    /// </summary>
    public class SendEmailMailer : ISendEmailMailer
    {
        private readonly IConfiguration _configuration;

        public SendEmailMailer(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void SendEmail(SendEmailModel mail)
        {
            using (var message = new MailMessage())
            {
                //foreach (var two in mail.To)
                //{
                //    message.To.Add(new MailAddress(two));
                //}
                message.To.Add(mail.SingleTo);
                message.From = new MailAddress(mail.From);
                if (mail.Bcc != null)
                {
                    foreach (var blind in mail.Bcc)
                    {
                        message.Bcc.Add(new MailAddress(blind));
                    }
                }
                if (mail.CC != null)
                {
                    foreach (var share in mail.CC)
                    {
                        message.CC.Add(new MailAddress(share));
                    }
                }
                message.Subject = mail.Subject;
                message.Body = mail.Body;
                message.IsBodyHtml = true;
               
                using (var client = new SmtpClient(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerAddress").Value))
                {
                    client.Port = int.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerPort").Value);
                    client.Credentials = new NetworkCredential(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPEmail").Value, _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPPassword").Value);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
        
        //public SendEmailMailer()
        //{
        //    MasterName = "_Layout";
        //}
        //public virtual MvcMailMessage SendEmail(SendEmailModel model)
        //{
        //    var mailMessage = new MvcMailMessage
        //    {
        //        Subject = model.Subject
        //    };
        //    if (model.To != null)
        //    {
        //        if (model.To.Count() > 0)
        //        {
        //            foreach (var mailto in model.To)
        //            {
        //                mailMessage.To.Add(mailto);
        //            }

        //        }
        //    }
        //    if (model.CC != null)
        //    {
        //        if (model.CC.Count() > 0)
        //        {
        //            foreach (var mailCC in model.CC)
        //            {
        //                mailMessage.CC.Add(mailCC);
        //            }

        //        }
        //    }
        //    if (model.Bcc != null)
        //    {
        //        if (model.Bcc.Count() > 0)
        //        {
        //            foreach (var mailBcc in model.Bcc)
        //            {
        //                mailMessage.Bcc.Add(mailBcc);
        //            }

        //        }
        //    }
        //    if (model.attachment != null)
        //    {
        //        if (model.attachment.Count() > 0)
        //        {
        //            foreach (var mailAttachment in model.attachment)
        //            {
        //                mailMessage.Attachments.Add(new Attachment(mailAttachment));
        //            }

        //        }
        //    }
        //    // Use a strongly typed model
        //    ViewData = new ViewDataDictionary(model);
        //    if (model.FormatId == 1)
        //    {
        //        PopulateBody(mailMessage, "UserApproveAdmin", null);
        //    }
        //    else if (model.FormatId == 2)
        //    {
        //        if (model.PaymentMethod == "Instantpayment")
        //        {
        //            PopulateBody(mailMessage, "InstantInvoice", null);
        //        }
        //        else
        //        {
        //            PopulateBody(mailMessage, "Invoice", null);
        //        }

        //        SendEmail(model.To, model.Subject, mailMessage.Body, model.attachment);
        //    }
        //    mailMessage.IsBodyHtml = model.isBodyHTML;

        //    return mailMessage;
        //}

        //public void SendEmail(string[] toEmail, string subject, string body, string[] attachment)
        //{

        //    using (MailMessage mail = new MailMessage())
        //    {
        //        if (toEmail != null)
        //        {
        //            foreach (var mailto in toEmail)
        //            {
        //                mail.To.Add(mailto);
        //            }

        //            if (attachment != null)
        //            {
        //                foreach (var mailAttachment in attachment)
        //                {
        //                    mail.Attachments.Add(new Attachment(mailAttachment));
        //                }
        //            }

        //            mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
        //            mail.Subject = subject;

        //            mail.Body = body;

        //            mail.IsBodyHtml = true;

        //            using (SmtpClient smtp = new SmtpClient())
        //            {
        //                smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
        //                smtp.Credentials = new System.Net.NetworkCredential
        //                     (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
        //                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

        //                //Or your Smtp Email ID and Password
        //                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
        //                smtp.Send(mail);
        //                smtp.Dispose();
        //                //throw new Exception("error.");
        //            }


        //        }

        //    }

        //}
    
    }
}