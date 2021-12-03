using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        void SendMail(NewAdvertFormModel model);
    }


    public class AdvertEmail : IAdvertEmail
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _configuration;
        private readonly IUserManagementDAL _userDAL;
        private readonly ISendEmailMailer _mailer;
        private readonly ICampaignDAL _campDAL;
        private readonly ICountryAreaDAL _countryDAL;
        private readonly IOperatorDAL _operatorDAL;
        private readonly IHttpContextAccessor _httpAccessor;

        public AdvertEmail(IHttpContextAccessor httpAccessor, IConfiguration configuration, IWebHostEnvironment _env, IUserManagementDAL userDAL, ISendEmailMailer mailer,
                            ICampaignDAL campDAL, ICountryAreaDAL countryDAL, IOperatorDAL operatorDAL)
        {
            _configuration = configuration;
            env = _env;
            _userDAL = userDAL;
            _mailer = mailer;
            _campDAL = campDAL;
            _countryDAL = countryDAL;
            _operatorDAL = operatorDAL;
            _httpAccessor = httpAccessor;
        }

        public void SendMail(NewAdvertFormModel model)
            //string AdvertName, int? OperatorAdmin, int UserId, string CampaignName, string CountryName, string OperatorName, DateTime AdvertDateTime)
        {
            try
            {
                string url = "";
                string advertURL = "";
                string siteAddress = _configuration.GetValue<string>("AppSettings:siteAddress");

                CampaignProfileDto campaignDetails = _campDAL.GetCampaignProfileDetail(model.CampaignProfileId).Result;
                var countryDetails = _countryDAL.GetCountryById(campaignDetails.CountryId.Value).Result;
                var operatorDetails = _operatorDAL.GetOperatorById(model.OperatorId).Result;

                // Need this until Advert Admin users use admin.adtones.com
                string adtonesSiteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress");

                var advertAdminDetails = _userDAL.GetAdvertOperatorAdmins((int)Enums.UserRole.AdvertAdmin).Result.ToList();
                var operatorAdminDetails = _userDAL.GetAdvertOperatorAdmins((int)Enums.UserRole.OperatorAdmin, model.OperatorId).Result.ToList();
                var advertiserDetails = _userDAL.GetUserById(model.AdvertiserId).Result;

                TimeZone curTimeZone = TimeZone.CurrentTimeZone;
                DateTime advertUTC = curTimeZone.ToUniversalTime(DateTime.Now);

                string subject = "New Advert: " + model.AdvertName + " - Campaign: " + campaignDetails.CampaignName + " - Advertiser: " + advertiserDetails.FirstName + " " + advertiserDetails.LastName;


                if (advertAdminDetails != null)
                {
                    var mail = new SendEmailModel();
                    var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                    var template = _configuration.GetSection("AppSettings").GetSection("AdvertEmailTemplateForAdvertAdmin").Value;
                    var path = Path.Combine(otherpath, template);
                    string emailContent = string.Empty;
                    if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                    {
                        emailContent = "<b>This is a courtesy message as Advert HAS BEEN Pre-APPROVED at Advert Admin Level</b>\n";
                    }
                    using (var reader = new StreamReader(path))
                    {
                        emailContent += reader.ReadToEnd();
                    }

                    advertURL = "<a href='" + adtonesSiteAddress + "/AdvertAdmin/UserAdvert/Index'>" + model.AdvertName + "</a>";
                    url = adtonesSiteAddress + "/AdvertAdmin/UserAdvert/Index";
                    url = "<a href='" + url + "'>" + url + " </a>";

                    emailContent = string.Format(emailContent, advertURL, campaignDetails.CampaignName, countryDetails.Name, operatorDetails.OperatorName, advertUTC, url);

                    mail.SingleTo = advertAdminDetails[0];
                    mail.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SiteEmailAddress").Value;
                    mail.Subject = subject;

                    if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                    {
                        emailContent += "\n\n <b>This is a courtesy message as Advert HAS BEEN Pre-APPROVED at Advert Admin Level</b>";
                    }

                    mail.Body = emailContent.Replace("\n", "<br/>");

                    mail.isBodyHTML = true;

                    _mailer.SendBasicEmail(mail, model.OperatorId, (int)Enums.UserRole.AdvertAdmin);
                }

                if (operatorAdminDetails.Count() > 0)
                {
                    foreach (var operatorAdminData in operatorAdminDetails)
                    {
                        var mail = new SendEmailModel();
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

                        if (model.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                        {
                            safaricomOperatorAdminSiteAddress = _configuration.GetValue<string>("AppSettings:SafaricomOperatorAdminSiteAddress");
                            advertURL = "<a href='" + safaricomOperatorAdminSiteAddress + "'>" + model.AdvertName + "</a>";
                            campaignURL = "<a href='" + safaricomOperatorAdminSiteAddress + "'>" + campaignDetails.CampaignName + "</a>";
                            url = safaricomOperatorAdminSiteAddress;
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
                            advertURL = "<a href='" + siteAddress + "'>" + model.AdvertName + "</a>";
                            campaignURL = "<a href='" + siteAddress + "'>" + campaignDetails.CampaignName + "</a>";
                            url = siteAddress;
                            url = "<a href='" + url + "'>" + url + " </a>";

                            mail.SingleTo = operatorAdminData;
                            mail.From = "";

                            emailContent = string.Format(emailContent, advertURL, campaignURL, advertUTC, url);

                            mail.Subject = subject;
                            mail.Body = emailContent.Replace("\n", "<br/>");
                            mail.isBodyHTML = true;

                        }

                       _mailer.SendBasicEmail(mail, model.OperatorId, (int)Enums.UserRole.OperatorAdmin);
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
