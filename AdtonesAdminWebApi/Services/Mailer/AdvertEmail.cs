using AdtonesAdminWebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services.Mailer
{
    public interface IAdvertEmail
    {
        void SendMail(string AdvertName, int? OperatorAdmin, int UserId, string CampaignName, string CountryName, string OperatorName, DateTime AdvertDateTime);
    }


    public class AdvertEmail : IAdvertEmail
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _configuration;
        private readonly IUserManagementDAL _userDAL;
        private readonly ISendEmailMailer _mailer;

        public AdvertEmail(IConfiguration configuration, IWebHostEnvironment _env, IUserManagementDAL userDAL, ISendEmailMailer mailer)
        {
            _configuration = configuration;
            env = _env;
            _userDAL = userDAL;
            _mailer = mailer;
        }

        public void SendMail(string AdvertName, int? OperatorAdmin, int UserId, string CampaignName, string CountryName, string OperatorName, DateTime AdvertDateTime)
        {
            try
            {
                string url = "";
                string advertURL = "";
                string siteAddress = _configuration.GetValue<string>("AppSettings:siteAddress");

                var advertAdminDetails = _userDAL.GetAdvertOperatorAdmins((int)Enums.UserRole.AdvertAdmin).Result.ToList();
                var operatorAdminDetails = _userDAL.GetAdvertOperatorAdmins((int)Enums.UserRole.OperatorAdmin, OperatorAdmin.Value).Result.ToList();
                var advertiserDetails = _userDAL.GetUserById(UserId).Result;

                TimeZone curTimeZone = TimeZone.CurrentTimeZone;
                DateTime advertUTC = curTimeZone.ToUniversalTime(AdvertDateTime);

                string subject = "New Advert: " + AdvertName + " - Campaign: " + CampaignName + " - Advertiser: " + advertiserDetails.FirstName + " " + advertiserDetails.LastName;

                var mail = new SendEmailModel();
                if (advertAdminDetails != null)
                {
                    var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                    var template = _configuration.GetSection("AppSettings").GetSection("AdvertEmailTemplateForAdvertAdmin").Value;
                    var path = Path.Combine(otherpath, template);
                    string emailContent = string.Empty;
                    using (var reader = new StreamReader(path))
                    {
                        emailContent = reader.ReadToEnd();
                    }

                    advertURL = "<a href='" + siteAddress + "AdvertAdmin/UserAdvert/Index'>" + AdvertName + "</a>";
                    url = siteAddress + "AdvertAdmin/UserAdvert/Index";
                    url = "<a href='" + url + "'>" + url + " </a>";

                    emailContent = string.Format(emailContent, advertURL, CampaignName, CountryName, OperatorName, advertUTC, url);

                    mail.SingleTo = advertAdminDetails[0];
                    mail.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SiteEmailAddress").Value;
                    mail.Subject = subject;

                    mail.Body = emailContent.Replace("\n", "<br/>");

                    mail.isBodyHTML = true;

                    _mailer.SendBasicEmail(mail, OperatorAdmin.Value, (int)Enums.UserRole.AdvertAdmin);

                }

                if (operatorAdminDetails.Count() > 0)
                {
                    foreach (var operatorAdminData in operatorAdminDetails)
                    {
                        mail = new SendEmailModel();
                        var safaricomOperatorAdminSiteAddress = "";
                        string campaignURL = "";

                        var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                        var template = _configuration.GetSection("AppSettings").GetSection("AdvertEmailTemplateForOperatorAdmin").Value;
                        var path = Path.Combine(otherpath, template);
                        string emailContent = string.Empty;
                        using (var reader = new StreamReader(path))
                        {
                            emailContent = reader.ReadToEnd();
                        }

                        if (OperatorAdmin == (int)Enums.OperatorTableId.Safaricom)
                        {
                            safaricomOperatorAdminSiteAddress = _configuration.GetValue<string>("AppSettings:SafaricomOperatorAdminSiteAddress");
                            advertURL = "<a href='" + safaricomOperatorAdminSiteAddress + "Advert/Index'>" + AdvertName + "</a>";
                            campaignURL = "<a href='" + safaricomOperatorAdminSiteAddress + "OperatorAdmin/UserCampaign/Index'>" + CampaignName + "</a>";
                            url = safaricomOperatorAdminSiteAddress + "OperatorAdmin/UserAdvert/Index";
                            url = "<a href='" + url + "'>" + url + " </a>";

                            mail.SingleTo = operatorAdminData;
                            mail.From = "";

                            emailContent = string.Format(emailContent, advertURL, campaignURL, advertUTC, url);

                            mail.Subject = subject;
                            mail.Body = emailContent.Replace("\n", "<br/>");
                            mail.isBodyHTML = true;
                        }
                        else
                        {
                            advertURL = "<a href='" + siteAddress + "OperatorAdmin/UserAdvert/Index'>" + AdvertName + "</a>";
                            campaignURL = "<a href='" + siteAddress + "OperatorAdmin/UserCampaign/Index'>" + CampaignName + "</a>";
                            url = siteAddress + "OperatorAdmin/UserAdvert/Index";
                            url = "<a href='" + url + "'>" + url + " </a>";

                            mail.SingleTo = operatorAdminData;
                            mail.From = "";

                            emailContent = string.Format(emailContent, advertURL, campaignURL, advertUTC, url);

                            mail.Subject = subject;
                            mail.Body = emailContent.Replace("\n", "<br/>");
                            mail.isBodyHTML = true;

                        }

                       _mailer.SendBasicEmail(mail, OperatorAdmin.Value, (int)Enums.UserRole.OperatorAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.ToString();
            }
        }
    }
}
