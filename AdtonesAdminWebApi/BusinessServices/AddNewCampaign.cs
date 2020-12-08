//using AdtonesAdminWebApi.DAL.Interfaces;
//using AdtonesAdminWebApi.ViewModels;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AdtonesAdminWebApi.BusinessServices
//{
//    public class AddNewCampaign
//    {
//        private readonly IConfiguration _configuration;
//        private readonly IConnectionStringService _connService;
//        private readonly IHttpContextAccessor _httpAccessor;
//        private readonly INewCampaignDAL _campDAL;
//        ReturnResult result = new ReturnResult();

//        public AddNewCampaign(IConfiguration configuration, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
//                                INewCampaignDAL campDAL) //ISaveFiles saveFile)

//        {
//            _configuration = configuration;
//            _connService = connService;
//            // _saveFile = saveFile;
//            _httpAccessor = httpAccessor;
//            _campDAL = campDAL;
//        }

//            public async Task<ReturnResult> AddNewCampaignInfo(AddNewCampaihnModel campModel)
//        {
//                try
//                {
//                    NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();
//                    // EFMVCDataContex db = new EFMVCDataContex();
//                    PostedTimesModel postedTimesModel = new PostedTimesModel();
//                    postedTimesModel.DayIds = new string[] { "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "24:00" };
//                    int CountryId = 0;
//                    if (model.CountryId == "12" || model.CountryId == "13" || model.CountryId == "14")
//                    {
//                        CountryId = 12;
//                    }
//                    else if (model.CountryId == "11")
//                    {
//                        CountryId = 8;
//                    }
//                    else
//                    {
//                        CountryId = Convert.ToInt32(model.CountryId);
//                    }
//                    var currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;

//                    if (campaignId != "")
//                    {
//                        IEnumerable<CampaignProfile> CampaignNameexists;
//                        if (CampaignProfileID == 0)
//                        {
//                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
//                        }
//                        else
//                        {
//                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
//                        }
//                        if (CampaignNameexists.Count() > 0)
//                        {
//                            FillCampaign(efmvcUser.UserId);
//                            FillCountry();

//                            return Json("Exists");
//                        }
//                        else
//                        {
//                            var campaignDetail = _profileRepository.GetById(Convert.ToInt32(campaignId));
//                            model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
//                            model.UserId = campaignDetail.UserId;
//                            model.ClientId = campaignDetail.ClientId;
//                            model.CampaignName = campaignName;
//                            model.CampaignDescription = campaignDescription;
//                            model.TotalBudget = campaignDetail.TotalBudget;
//                            model.MaxDailyBudget = campaignDetail.MaxDailyBudget;
//                            model.MaxBid = campaignDetail.MaxBid;
//                            model.MaxMonthBudget = campaignDetail.MaxMonthBudget;
//                            model.MaxWeeklyBudget = campaignDetail.MaxWeeklyBudget;
//                            model.MaxHourlyBudget = campaignDetail.MaxHourlyBudget;
//                            model.TotalCredit = campaignDetail.TotalCredit;
//                            model.SpendToDate = campaignDetail.SpendToDate;
//                            model.AvailableCredit = campaignDetail.AvailableCredit;
//                            model.PlaysToDate = campaignDetail.PlaysToDate;
//                            model.PlaysLastMonth = campaignDetail.PlaysLastMonth;
//                            model.PlaysCurrentMonth = campaignDetail.PlaysCurrentMonth;
//                            model.CancelledToDate = campaignDetail.CancelledToDate;
//                            model.CancelledLastMonth = campaignDetail.CancelledLastMonth;
//                            model.CancelledCurrentMonth = campaignDetail.CancelledCurrentMonth;
//                            model.SmsToDate = campaignDetail.SmsToDate;
//                            model.SmsLastMonth = campaignDetail.SmsLastMonth;
//                            model.SmsCurrentMonth = campaignDetail.SmsCurrentMonth;
//                            model.EmailToDate = campaignDetail.EmailToDate;
//                            model.EmailsLastMonth = campaignDetail.EmailsLastMonth;
//                            model.EmailsCurrentMonth = campaignDetail.EmailsCurrentMonth;
//                            model.EmailFileLocation = campaignDetail.EmailFileLocation;
//                            model.Active = campaignDetail.Active;
//                            model.NumberOfPlays = campaignDetail.NumberOfPlays;
//                            model.AverageDailyPlays = campaignDetail.AverageDailyPlays;
//                            model.SmsRequests = campaignDetail.SmsRequests;
//                            model.EmailsDelievered = campaignDetail.EmailsDelievered;
//                            model.EmailSubject = campaignDetail.EmailSubject;
//                            model.EmailBody = campaignDetail.EmailBody;
//                            model.EmailSenderAddress = campaignDetail.EmailSenderAddress;
//                            model.SmsOriginator = campaignDetail.SmsOriginator;
//                            model.SmsBody = campaignDetail.SmsBody;
//                            model.SMSFileLocation = campaignDetail.SMSFileLocation;
//                            model.CreatedDateTime = campaignDetail.CreatedDateTime;
//                            model.UpdatedDateTime = campaignDetail.UpdatedDateTime;
//                            model.Status = (int)CampaignStatus.InProgress;
//                            model.StartDate = campaignDetail.StartDate;
//                            model.EndDate = campaignDetail.EndDate;
//                            model.NumberInBatch = campaignDetail.NumberInBatch;
//                            model.CountryId = int.Parse(countryId);
//                            model.IsAdminApproval = false;
//                            model.RemainingMaxDailyBudget = campaignDetail.RemainingMaxDailyBudget;
//                            model.RemainingMaxHourlyBudget = campaignDetail.RemainingMaxHourlyBudget;
//                            model.RemainingMaxWeeklyBudget = campaignDetail.RemainingMaxWeeklyBudget;
//                            model.RemainingMaxMonthBudget = campaignDetail.RemainingMaxMonthBudget;
//                            model.ProvidendSpendAmount = campaignDetail.ProvidendSpendAmount;
//                            model.BucketCount = campaignDetail.BucketCount;
//                            model.PhoneticAlphabet = phoneticAlphabet;
//                            model.NextStatus = false;
//                            model.CurrencyCode = currencyCode;

//                            CreateOrUpdateCopyCampaignProfileCommand command =
//                                Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

//                            ICommandResult result = _commandBus.Submit(command);
//                            if (result.Success)
//                            {
//                                var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
//                                if (CampaignData.Count() > 0 && CampaignData != null)
//                                {
//                                    CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
//                                }

//                                //Update Campaign Profile Time Setting
//                                CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
//                                CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
//                                CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
//                                CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
//                                Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
//                                    CampaignProfileTimeSettingModel);
//                                ICommandResult result1 = _commandBus.Submit(command1);

//                                var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
//                                if (ConnString != null && ConnString.Count() > 0)
//                                {
//                                    UserMatchTableProcess obj = new UserMatchTableProcess();
//                                    foreach (var item in ConnString)
//                                    {
//                                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
//                                        var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
//                                        if (campaigndetails != null)
//                                        {
//                                            obj.AddCampaignData(campaigndetails, SQLServerEntities);
//                                            PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
//                                        }
//                                    }
//                                }

//                                var userDetails = _userRepository.GetById(efmvcUser.UserId);

//                                //Email Code
//                                var adminDetails = _contactsRepository.Get(s => s.UserId == 19);
//                                if (adminDetails != null)
//                                {
//                                    var reader =
//                                        new StreamReader(
//                                            Server.MapPath(ConfigurationManager.AppSettings["CampaignEmailTemplate"]));
//                                    var url = ConfigurationManager.AppSettings["AdminUrlForCampaign"];
//                                    string emailContent = reader.ReadToEnd();

//                                    emailContent = string.Format(emailContent, campaignName, userDetails.FirstName, userDetails.LastName, userDetails.Organisation == null ? "-" : userDetails.Organisation, userDetails.Email, DateTime.Now.ToString("HH:mm dd-MM-yyyy"), url);

//                                    MailMessage mail = new MailMessage();
//                                    mail.To.Add(adminDetails.Email);
//                                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
//                                    mail.Subject = "Campaign Verification";

//                                    mail.Body = emailContent.Replace("\n", "<br/>");

//                                    mail.IsBodyHtml = true;
//                                    SmtpClient smtp = new SmtpClient();
//                                    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
//                                    smtp.Credentials = new System.Net.NetworkCredential
//                                         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
//                                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

//                                    //Or your Smtp Email ID and Password
//                                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
//                                    smtp.Send(mail);
//                                }
//                                return Json("success");
//                            }
//                        }
//                    }
//                    else
//                    {
//                        IEnumerable<CampaignProfile> CampaignNameexists;
//                        if (CampaignProfileID == 0)
//                        {
//                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
//                        }
//                        else
//                        {
//                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
//                        }
//                        if (CampaignNameexists.Count() > 0)
//                        {
//                            FillCampaign(efmvcUser.UserId);
//                            FillCountry();

//                            return Json("Exists");
//                        }
//                        else
//                        {
//                            model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
//                            model.UserId = efmvcUser.UserId;
//                            model.ClientId = null;
//                            model.CampaignName = campaignName;
//                            model.CampaignDescription = campaignDescription;
//                            model.TotalBudget = decimal.Parse("0.00");
//                            model.MaxDailyBudget = float.Parse("0.00");
//                            model.MaxBid = float.Parse("0.00");
//                            model.MaxMonthBudget = float.Parse("0.00");
//                            model.MaxWeeklyBudget = float.Parse("0.00");
//                            model.MaxHourlyBudget = float.Parse("0.00");
//                            model.TotalCredit = decimal.Parse("0.00");
//                            model.SpendToDate = float.Parse("0.00");
//                            model.AvailableCredit = decimal.Parse("0.00");
//                            model.PlaysToDate = int.Parse("0");
//                            model.PlaysLastMonth = int.Parse("0");
//                            model.PlaysCurrentMonth = int.Parse("0");
//                            model.CancelledToDate = int.Parse("0");
//                            model.CancelledLastMonth = int.Parse("0");
//                            model.CancelledCurrentMonth = int.Parse("0");
//                            model.SmsToDate = int.Parse("0");
//                            model.SmsLastMonth = int.Parse("0");
//                            model.SmsCurrentMonth = int.Parse("0");
//                            model.EmailToDate = int.Parse("0");
//                            model.EmailsLastMonth = int.Parse("0");
//                            model.EmailsCurrentMonth = int.Parse("0");
//                            model.EmailFileLocation = null;
//                            model.Active = true;
//                            model.NumberOfPlays = int.Parse("0");
//                            model.AverageDailyPlays = int.Parse("0");
//                            model.SmsRequests = int.Parse("0");
//                            model.EmailsDelievered = int.Parse("0");
//                            model.EmailSubject = null;
//                            model.EmailBody = null;
//                            model.EmailSenderAddress = null;
//                            model.SmsOriginator = null;
//                            model.SmsBody = null;
//                            model.SMSFileLocation = null;
//                            model.CreatedDateTime = DateTime.Now;
//                            model.UpdatedDateTime = DateTime.Now;
//                            model.Status = (int)CampaignStatus.InProgress;
//                            model.StartDate = null;
//                            model.EndDate = null;
//                            model.NumberInBatch = 0;
//                            model.CountryId = int.Parse(countryId);
//                            model.IsAdminApproval = false;
//                            model.RemainingMaxDailyBudget = float.Parse("0.00"); ;
//                            model.RemainingMaxHourlyBudget = float.Parse("0.00"); ;
//                            model.RemainingMaxWeeklyBudget = float.Parse("0.00");
//                            model.RemainingMaxMonthBudget = float.Parse("0.00");
//                            model.ProvidendSpendAmount = decimal.Parse("0.00");
//                            model.BucketCount = int.Parse("0");
//                            model.PhoneticAlphabet = phoneticAlphabet;
//                            model.NextStatus = true;
//                            model.AdtoneServerCampaignProfileId = null;
//                            model.CurrencyCode = currencyCode;

//                            CreateOrUpdateCopyCampaignProfileCommand command =
//                                Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

//                            ICommandResult result = _commandBus.Submit(command);
//                            if (result.Success)
//                            {
//                                var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
//                                if (CampaignData.Count() > 0 && CampaignData != null)
//                                {
//                                    CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
//                                }

//                                //Update Campaign Profile Time Setting
//                                CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
//                                CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
//                                CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
//                                CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
//                                CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
//                                Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
//                                    CampaignProfileTimeSettingModel);
//                                ICommandResult result1 = _commandBus.Submit(command1);

//                                var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
//                                if (ConnString != null && ConnString.Count() > 0)
//                                {
//                                    UserMatchTableProcess obj = new UserMatchTableProcess();
//                                    foreach (var item in ConnString)
//                                    {
//                                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
//                                        var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
//                                        if (campaigndetails != null)
//                                        {
//                                            obj.AddCampaignData(campaigndetails, SQLServerEntities);
//                                            PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
//                                        }
//                                    }
//                                }

//                                var userDetails = _userRepository.GetById(efmvcUser.UserId);

//                                //Email Code
//                                var adminDetails = _contactsRepository.Get(s => s.UserId == 19);
//                                if (adminDetails != null)
//                                {
//                                    var reader =
//                                        new StreamReader(
//                                            Server.MapPath(ConfigurationManager.AppSettings["CampaignEmailTemplate"]));
//                                    var url = ConfigurationManager.AppSettings["AdminUrlForCampaign"];
//                                    string emailContent = reader.ReadToEnd();

//                                    emailContent = string.Format(emailContent, campaignName, userDetails.FirstName, userDetails.LastName, userDetails.Organisation == null ? "-" : userDetails.Organisation, userDetails.Email, DateTime.Now.ToString("HH:mm dd-MM-yyyy"), url);

//                                    MailMessage mail = new MailMessage();
//                                    mail.To.Add(adminDetails.Email);
//                                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
//                                    mail.Subject = "Campaign Verification";

//                                    mail.Body = emailContent.Replace("\n", "<br/>");

//                                    mail.IsBodyHtml = true;
//                                    SmtpClient smtp = new SmtpClient();
//                                    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
//                                    smtp.Credentials = new System.Net.NetworkCredential
//                                         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
//                                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

//                                    //Or your Smtp Email ID and Password
//                                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
//                                    smtp.Send(mail);
//                                }
//                                return Json("success");
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    TempData["Error"] = ex.InnerException.Message;
//                    return Json("fail");
//                }
//                return result;
//        }

//        [AuthorizeFilter] // Get Data
//        public ActionResult AddClientData(string campaignId, string campaignName, string campaignDescription, string phoneticAlphabet, string countryId, string clientCheck)
//        {
//            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
//            if (efmvcUser != null)
//            {
//                try
//                {
//                    NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();
//                    // EFMVCDataContex db = new EFMVCDataContex();
//                    PostedTimesModel postedTimesModel = new PostedTimesModel();
//                    postedTimesModel.DayIds = new string[] { "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "24:00" };
//                    int CountryId = 0;
//                    if (countryId == "12" || countryId == "13" || countryId == "14")
//                    {
//                        CountryId = 12;
//                    }
//                    else if (countryId == "11")
//                    {
//                        CountryId = 8;
//                    }
//                    else
//                    {
//                        CountryId = Convert.ToInt32(countryId);
//                    }
//                    var currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;

//                    if (clientCheck == "true")
//                    {
//                        if (campaignId != "")
//                        {
//                            IEnumerable<CampaignProfile> CampaignNameexists;
//                            if (CampaignProfileID == 0)
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
//                            }
//                            else
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
//                            }
//                            if (CampaignNameexists.Count() > 0)
//                            {
//                                FillCampaign(efmvcUser.UserId);
//                                FillCountry();

//                                return Json("Exists");
//                            }
//                            else
//                            {
//                                var campaignDetail = _profileRepository.GetById(Convert.ToInt32(campaignId));
//                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
//                                model.UserId = campaignDetail.UserId;
//                                model.ClientId = campaignDetail.ClientId;
//                                model.CampaignName = campaignName;
//                                model.CampaignDescription = campaignDescription;
//                                model.TotalBudget = campaignDetail.TotalBudget;
//                                model.MaxDailyBudget = campaignDetail.MaxDailyBudget;
//                                model.MaxBid = campaignDetail.MaxBid;
//                                model.MaxMonthBudget = campaignDetail.MaxMonthBudget;
//                                model.MaxWeeklyBudget = campaignDetail.MaxWeeklyBudget;
//                                model.MaxHourlyBudget = campaignDetail.MaxHourlyBudget;
//                                model.TotalCredit = campaignDetail.TotalCredit;
//                                model.SpendToDate = campaignDetail.SpendToDate;
//                                model.AvailableCredit = campaignDetail.AvailableCredit;
//                                model.PlaysToDate = campaignDetail.PlaysToDate;
//                                model.PlaysLastMonth = campaignDetail.PlaysLastMonth;
//                                model.PlaysCurrentMonth = campaignDetail.PlaysCurrentMonth;
//                                model.CancelledToDate = campaignDetail.CancelledToDate;
//                                model.CancelledLastMonth = campaignDetail.CancelledLastMonth;
//                                model.CancelledCurrentMonth = campaignDetail.CancelledCurrentMonth;
//                                model.SmsToDate = campaignDetail.SmsToDate;
//                                model.SmsLastMonth = campaignDetail.SmsLastMonth;
//                                model.SmsCurrentMonth = campaignDetail.SmsCurrentMonth;
//                                model.EmailToDate = campaignDetail.EmailToDate;
//                                model.EmailsLastMonth = campaignDetail.EmailsLastMonth;
//                                model.EmailsCurrentMonth = campaignDetail.EmailsCurrentMonth;
//                                model.EmailFileLocation = campaignDetail.EmailFileLocation;
//                                model.Active = campaignDetail.Active;
//                                model.NumberOfPlays = campaignDetail.NumberOfPlays;
//                                model.AverageDailyPlays = campaignDetail.AverageDailyPlays;
//                                model.SmsRequests = campaignDetail.SmsRequests;
//                                model.EmailsDelievered = campaignDetail.EmailsDelievered;
//                                model.EmailSubject = campaignDetail.EmailSubject;
//                                model.EmailBody = campaignDetail.EmailBody;
//                                model.EmailSenderAddress = campaignDetail.EmailSenderAddress;
//                                model.SmsOriginator = campaignDetail.SmsOriginator;
//                                model.SmsBody = campaignDetail.SmsBody;
//                                model.SMSFileLocation = campaignDetail.SMSFileLocation;
//                                model.CreatedDateTime = campaignDetail.CreatedDateTime;
//                                model.UpdatedDateTime = campaignDetail.UpdatedDateTime;
//                                model.Status = (int)CampaignStatus.InProgress;
//                                model.StartDate = campaignDetail.StartDate;
//                                model.EndDate = campaignDetail.EndDate;
//                                model.NumberInBatch = campaignDetail.NumberInBatch;
//                                model.CountryId = int.Parse(countryId);
//                                model.IsAdminApproval = false;
//                                model.RemainingMaxDailyBudget = campaignDetail.RemainingMaxDailyBudget;
//                                model.RemainingMaxHourlyBudget = campaignDetail.RemainingMaxHourlyBudget;
//                                model.RemainingMaxWeeklyBudget = campaignDetail.RemainingMaxWeeklyBudget;
//                                model.RemainingMaxMonthBudget = campaignDetail.RemainingMaxMonthBudget;
//                                model.ProvidendSpendAmount = campaignDetail.ProvidendSpendAmount;
//                                model.BucketCount = campaignDetail.BucketCount;
//                                model.PhoneticAlphabet = phoneticAlphabet;
//                                model.NextStatus = true;
//                                model.CurrencyCode = currencyCode;

//                                CreateOrUpdateCopyCampaignProfileCommand command =
//                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

//                                ICommandResult result = _commandBus.Submit(command);
//                                if (result.Success)
//                                {
//                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
//                                    if (CampaignData.Count() > 0 && CampaignData != null)
//                                    {
//                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
//                                    }

//                                    //Update Campaign Profile Time Setting
//                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
//                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
//                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
//                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
//                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
//                                        CampaignProfileTimeSettingModel);
//                                    ICommandResult result1 = _commandBus.Submit(command1);

//                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
//                                    if (ConnString != null && ConnString.Count() > 0)
//                                    {
//                                        UserMatchTableProcess obj = new UserMatchTableProcess();
//                                        foreach (var item in ConnString)
//                                        {
//                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
//                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
//                                            if (campaigndetails != null)
//                                            {
//                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
//                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
//                                            }
//                                        }
//                                    }

//                                    FillClient(efmvcUser.UserId);
//                                    return Json(new { success = "successjson" }, JsonRequestBehavior.AllowGet);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            IEnumerable<CampaignProfile> CampaignNameexists;
//                            if (CampaignProfileID == 0)
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
//                            }
//                            else
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
//                            }
//                            if (CampaignNameexists.Count() > 0)
//                            {
//                                FillCampaign(efmvcUser.UserId);
//                                FillCountry();

//                                return Json("Exists");
//                            }
//                            else
//                            {
//                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
//                                model.UserId = efmvcUser.UserId;
//                                model.ClientId = null;
//                                model.CampaignName = campaignName;
//                                model.CampaignDescription = campaignDescription;
//                                model.TotalBudget = decimal.Parse("0.00");
//                                model.MaxDailyBudget = float.Parse("0.00");
//                                model.MaxBid = float.Parse("0.00");
//                                model.MaxMonthBudget = float.Parse("0.00");
//                                model.MaxWeeklyBudget = float.Parse("0.00");
//                                model.MaxHourlyBudget = float.Parse("0.00");
//                                model.TotalCredit = decimal.Parse("0.00");
//                                model.SpendToDate = float.Parse("0.00");
//                                model.AvailableCredit = decimal.Parse("0.00");
//                                model.PlaysToDate = int.Parse("0");
//                                model.PlaysLastMonth = int.Parse("0");
//                                model.PlaysCurrentMonth = int.Parse("0");
//                                model.CancelledToDate = int.Parse("0");
//                                model.CancelledLastMonth = int.Parse("0");
//                                model.CancelledCurrentMonth = int.Parse("0");
//                                model.SmsToDate = int.Parse("0");
//                                model.SmsLastMonth = int.Parse("0");
//                                model.SmsCurrentMonth = int.Parse("0");
//                                model.EmailToDate = int.Parse("0");
//                                model.EmailsLastMonth = int.Parse("0");
//                                model.EmailsCurrentMonth = int.Parse("0");
//                                model.EmailFileLocation = null;
//                                model.Active = true;
//                                model.NumberOfPlays = int.Parse("0");
//                                model.AverageDailyPlays = int.Parse("0");
//                                model.SmsRequests = int.Parse("0");
//                                model.EmailsDelievered = int.Parse("0");
//                                model.EmailSubject = null;
//                                model.EmailBody = null;
//                                model.EmailSenderAddress = null;
//                                model.SmsOriginator = null;
//                                model.SmsBody = null;
//                                model.SMSFileLocation = null;
//                                model.CreatedDateTime = DateTime.Now;
//                                model.UpdatedDateTime = DateTime.Now;
//                                model.Status = (int)CampaignStatus.InProgress;
//                                model.StartDate = null;
//                                model.EndDate = null;
//                                model.NumberInBatch = 0;
//                                model.CountryId = int.Parse(countryId);
//                                model.IsAdminApproval = false;
//                                model.RemainingMaxDailyBudget = float.Parse("0.00");
//                                model.RemainingMaxHourlyBudget = float.Parse("0.00");
//                                model.RemainingMaxWeeklyBudget = float.Parse("0.00");
//                                model.RemainingMaxMonthBudget = float.Parse("0.00");
//                                model.ProvidendSpendAmount = decimal.Parse("0.00");
//                                model.BucketCount = int.Parse("0");
//                                model.PhoneticAlphabet = phoneticAlphabet;
//                                model.NextStatus = true;
//                                model.AdtoneServerCampaignProfileId = null;
//                                model.CurrencyCode = currencyCode;

//                                CreateOrUpdateCopyCampaignProfileCommand command =
//                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

//                                ICommandResult result = _commandBus.Submit(command);
//                                if (result.Success)
//                                {
//                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
//                                    if (CampaignData.Count() > 0 && CampaignData != null)
//                                    {
//                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
//                                    }

//                                    //Update Campaign Profile Time Setting
//                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
//                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
//                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
//                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
//                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
//                                        CampaignProfileTimeSettingModel);
//                                    ICommandResult result1 = _commandBus.Submit(command1);

//                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
//                                    if (ConnString != null && ConnString.Count() > 0)
//                                    {
//                                        UserMatchTableProcess obj = new UserMatchTableProcess();
//                                        foreach (var item in ConnString)
//                                        {
//                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
//                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
//                                            if (campaigndetails != null)
//                                            {
//                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
//                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
//                                            }
//                                        }
//                                    }

//                                    FillClient(efmvcUser.UserId);
//                                    return Json(new { success = "successjson" }, JsonRequestBehavior.AllowGet);
//                                }
//                            }
//                        }
//                    }
//                    else
//                    {
//                        if (campaignId != "")
//                        {
//                            IEnumerable<CampaignProfile> CampaignNameexists;
//                            if (CampaignProfileID == 0)
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
//                            }
//                            else
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
//                            }
//                            if (CampaignNameexists.Count() > 0)
//                            {
//                                FillCampaign(efmvcUser.UserId);
//                                FillCountry();

//                                return Json("Exists");
//                            }
//                            else
//                            {
//                                var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;

//                                var campaignDetail = _profileRepository.GetById(Convert.ToInt32(campaignId));
//                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
//                                model.UserId = campaignDetail.UserId;
//                                model.ClientId = campaignDetail.ClientId;
//                                model.CampaignName = campaignName;
//                                model.CampaignDescription = campaignDescription;
//                                model.TotalBudget = campaignDetail.TotalBudget;
//                                model.MaxDailyBudget = campaignDetail.MaxDailyBudget;
//                                model.MaxBid = campaignDetail.MaxBid;
//                                model.MaxMonthBudget = campaignDetail.MaxMonthBudget;
//                                model.MaxWeeklyBudget = campaignDetail.MaxWeeklyBudget;
//                                model.MaxHourlyBudget = campaignDetail.MaxHourlyBudget;
//                                model.TotalCredit = campaignDetail.TotalCredit;
//                                model.SpendToDate = campaignDetail.SpendToDate;
//                                model.AvailableCredit = campaignDetail.AvailableCredit;
//                                model.PlaysToDate = campaignDetail.PlaysToDate;
//                                model.PlaysLastMonth = campaignDetail.PlaysLastMonth;
//                                model.PlaysCurrentMonth = campaignDetail.PlaysCurrentMonth;
//                                model.CancelledToDate = campaignDetail.CancelledToDate;
//                                model.CancelledLastMonth = campaignDetail.CancelledLastMonth;
//                                model.CancelledCurrentMonth = campaignDetail.CancelledCurrentMonth;
//                                model.SmsToDate = campaignDetail.SmsToDate;
//                                model.SmsLastMonth = campaignDetail.SmsLastMonth;
//                                model.SmsCurrentMonth = campaignDetail.SmsCurrentMonth;
//                                model.EmailToDate = campaignDetail.EmailToDate;
//                                model.EmailsLastMonth = campaignDetail.EmailsLastMonth;
//                                model.EmailsCurrentMonth = campaignDetail.EmailsCurrentMonth;
//                                model.EmailFileLocation = campaignDetail.EmailFileLocation;
//                                model.Active = campaignDetail.Active;
//                                model.NumberOfPlays = campaignDetail.NumberOfPlays;
//                                model.AverageDailyPlays = campaignDetail.AverageDailyPlays;
//                                model.SmsRequests = campaignDetail.SmsRequests;
//                                model.EmailsDelievered = campaignDetail.EmailsDelievered;
//                                model.EmailSubject = campaignDetail.EmailSubject;
//                                model.EmailBody = campaignDetail.EmailBody;
//                                model.EmailSenderAddress = campaignDetail.EmailSenderAddress;
//                                model.SmsOriginator = campaignDetail.SmsOriginator;
//                                model.SmsBody = campaignDetail.SmsBody;
//                                model.SMSFileLocation = campaignDetail.SMSFileLocation;
//                                model.CreatedDateTime = campaignDetail.CreatedDateTime;
//                                model.UpdatedDateTime = campaignDetail.UpdatedDateTime;
//                                model.Status = (int)CampaignStatus.InProgress;
//                                model.StartDate = campaignDetail.StartDate;
//                                model.EndDate = campaignDetail.EndDate;
//                                model.NumberInBatch = campaignDetail.NumberInBatch;
//                                model.CountryId = int.Parse(countryId);
//                                model.IsAdminApproval = false;
//                                model.RemainingMaxDailyBudget = campaignDetail.RemainingMaxDailyBudget;
//                                model.RemainingMaxHourlyBudget = campaignDetail.RemainingMaxHourlyBudget;
//                                model.RemainingMaxWeeklyBudget = campaignDetail.RemainingMaxWeeklyBudget;
//                                model.RemainingMaxMonthBudget = campaignDetail.RemainingMaxMonthBudget;
//                                model.ProvidendSpendAmount = campaignDetail.ProvidendSpendAmount;
//                                model.BucketCount = campaignDetail.BucketCount;
//                                model.PhoneticAlphabet = phoneticAlphabet;
//                                model.NextStatus = true;
//                                model.CurrencyCode = currencyCode;

//                                CreateOrUpdateCopyCampaignProfileCommand command =
//                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

//                                ICommandResult result = _commandBus.Submit(command);
//                                if (result.Success)
//                                {
//                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
//                                    if (CampaignData.Count() > 0 && CampaignData != null)
//                                    {
//                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
//                                    }

//                                    //Update Campaign Profile Time Setting
//                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
//                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
//                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
//                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
//                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
//                                        CampaignProfileTimeSettingModel);
//                                    ICommandResult result1 = _commandBus.Submit(command1);

//                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
//                                    if (ConnString != null && ConnString.Count() > 0)
//                                    {
//                                        UserMatchTableProcess obj = new UserMatchTableProcess();
//                                        foreach (var item in ConnString)
//                                        {
//                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
//                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
//                                            if (campaigndetails != null)
//                                            {
//                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
//                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
//                                            }
//                                        }
//                                    }

//                                    return Json(new { success = "successbudget", value = currencyCode, value1 = currencyId }, JsonRequestBehavior.AllowGet);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            IEnumerable<CampaignProfile> CampaignNameexists;
//                            if (CampaignProfileID == 0)
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
//                            }
//                            else
//                            {
//                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
//                            }
//                            if (CampaignNameexists.Count() > 0)
//                            {
//                                FillCampaign(efmvcUser.UserId);
//                                FillCountry();

//                                return Json("Exists");
//                            }
//                            else
//                            {
//                                var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;

//                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
//                                model.UserId = efmvcUser.UserId;
//                                model.ClientId = null;
//                                model.CampaignName = campaignName;
//                                model.CampaignDescription = campaignDescription;
//                                model.TotalBudget = decimal.Parse("0.00");
//                                model.MaxDailyBudget = float.Parse("0.00");
//                                model.MaxBid = float.Parse("0.00");
//                                model.MaxMonthBudget = float.Parse("0.00");
//                                model.MaxWeeklyBudget = float.Parse("0.00");
//                                model.MaxHourlyBudget = float.Parse("0.00");
//                                model.TotalCredit = decimal.Parse("0.00");
//                                model.SpendToDate = float.Parse("0.00");
//                                model.AvailableCredit = decimal.Parse("0.00");
//                                model.PlaysToDate = int.Parse("0");
//                                model.PlaysLastMonth = int.Parse("0");
//                                model.PlaysCurrentMonth = int.Parse("0");
//                                model.CancelledToDate = int.Parse("0");
//                                model.CancelledLastMonth = int.Parse("0");
//                                model.CancelledCurrentMonth = int.Parse("0");
//                                model.SmsToDate = int.Parse("0");
//                                model.SmsLastMonth = int.Parse("0");
//                                model.SmsCurrentMonth = int.Parse("0");
//                                model.EmailToDate = int.Parse("0");
//                                model.EmailsLastMonth = int.Parse("0");
//                                model.EmailsCurrentMonth = int.Parse("0");
//                                model.EmailFileLocation = null;
//                                model.Active = true;
//                                model.NumberOfPlays = int.Parse("0");
//                                model.AverageDailyPlays = int.Parse("0");
//                                model.SmsRequests = int.Parse("0");
//                                model.EmailsDelievered = int.Parse("0");
//                                model.EmailSubject = null;
//                                model.EmailBody = null;
//                                model.EmailSenderAddress = null;
//                                model.SmsOriginator = null;
//                                model.SmsBody = null;
//                                model.SMSFileLocation = null;
//                                model.CreatedDateTime = DateTime.Now;
//                                model.UpdatedDateTime = DateTime.Now;
//                                model.Status = (int)CampaignStatus.InProgress;
//                                model.StartDate = null;
//                                model.EndDate = null;
//                                model.NumberInBatch = 0;
//                                model.CountryId = int.Parse(countryId);
//                                model.IsAdminApproval = false;
//                                model.RemainingMaxDailyBudget = float.Parse("0.00");
//                                model.RemainingMaxHourlyBudget = float.Parse("0.00");
//                                model.RemainingMaxWeeklyBudget = float.Parse("0.00");
//                                model.RemainingMaxMonthBudget = float.Parse("0.00");
//                                model.ProvidendSpendAmount = decimal.Parse("0.00");
//                                model.BucketCount = int.Parse("0");
//                                model.PhoneticAlphabet = phoneticAlphabet;
//                                model.NextStatus = true;
//                                model.AdtoneServerCampaignProfileId = null;
//                                model.CurrencyCode = currencyCode;

//                                CreateOrUpdateCopyCampaignProfileCommand command =
//                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

//                                ICommandResult result = _commandBus.Submit(command);
//                                if (result.Success)
//                                {
//                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
//                                    if (CampaignData.Count() > 0 && CampaignData != null)
//                                    {
//                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
//                                    }

//                                    //Update Campaign Profile Time Setting
//                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
//                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
//                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
//                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
//                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
//                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
//                                        CampaignProfileTimeSettingModel);
//                                    ICommandResult result1 = _commandBus.Submit(command1);

//                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
//                                    if (ConnString != null && ConnString.Count() > 0)
//                                    {
//                                        UserMatchTableProcess obj = new UserMatchTableProcess();
//                                        foreach (var item in ConnString)
//                                        {
//                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
//                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
//                                            if (campaigndetails != null)
//                                            {
//                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
//                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
//                                            }
//                                        }
//                                    }

//                                    return Json(new { success = "successbudget", value = currencyCode, value1 = currencyId }, JsonRequestBehavior.AllowGet);
//                                }
//                            }
//                        }
//                    }
//                    return Json("");
//                }
//                catch (Exception ex)
//                {
//                    TempData["Error"] = ex.InnerException.Message;
//                    return Json("fail");
//                }
//            }
//            return RedirectToAction("Index", "Landing");
//        }

//        [AuthorizeFilter] // Save Data
//        public ActionResult AddClientInfo(string campaignName, string clientId, string clientName, string clientDescription, string clientEmail, string clientContactPhone, string phoneticAlphabet, string countryId)
//        {
//            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

//            if (efmvcUser != null)
//            {
//                NewClientFormModel model = new NewClientFormModel();
//                // EFMVCDataContex db = new EFMVCDataContex();

//                int ClientId = Convert.ToInt32(clientId);
//                NewAdvertFormModel model1 = new NewAdvertFormModel();
//                NewAdvertRejectionFormModel model2 = new NewAdvertRejectionFormModel();

//                if (clientId != null && clientId != "0" && clientId != "")
//                {
//                    try
//                    {
//                        IEnumerable<Client> clientNameexists;
//                        if (ClientID == 0)
//                        {
//                            clientNameexists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId).ToList();
//                        }
//                        else
//                        {
//                            clientNameexists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.Id == ClientID).ToList();
//                        }
//                        if (clientNameexists.Count() > 0)
//                        {
//                            FillClient(efmvcUser.UserId);

//                            return Json("Exists");
//                        }
//                        else
//                        {
//                            var clientDetail = _clientRepository.GetById(Convert.ToInt32(clientId));
//                            model.ClientId = ClientID == 0 ? 0 : ClientID;
//                            model.UserId = efmvcUser.UserId;
//                            model.ClientName = clientName;
//                            model.ClientDescription = clientDescription;
//                            model.ClientContactInfo = "";
//                            model.ClientBudget = clientDetail.Budget;
//                            model.CreatedDate = (DateTime)clientDetail.CreatedDate;
//                            model.UpdatedDate = (DateTime)clientDetail.UpdatedDate;
//                            model.ClientStatus = (int)ClientStatus.InProgress;
//                            model.ClientEmail = clientEmail;
//                            model.ClientPhoneticAlphabet = phoneticAlphabet;
//                            model.ClientContactPhone = clientContactPhone;
//                            model.NextStatus = false;
//                            model.CountryId = Convert.ToInt32(countryId);
//                            model.AdtoneServerClientId = null;

//                            CreateOrUpdateCopyClientCommand command =
//                                Mapper.Map<NewClientFormModel, CreateOrUpdateCopyClientCommand>(model);

//                            ICommandResult result = _commandBus.Submit(command);
//                            if (result.Success)
//                            {
//                                var ClientData = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
//                                if (ClientData.Count() > 0 && ClientData != null)
//                                {
//                                    ClientID = ClientData.FirstOrDefault().Id;
//                                }

//                                var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
//                                if (campaign != null)
//                                {
//                                    campaign.ClientId = result.Id;
//                                    db.SaveChanges();
//                                }

//                                return Json("success");
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        TempData["Error"] = ex.InnerException.Message;
//                        return Json("fail");
//                    }
//                }
//                return Json("fail");
//            }
//            return RedirectToAction("Index", "Landing");
//        }

//        [AuthorizeFilter] // Next Client Data
//        public ActionResult AddBudgetData(string campaignName, string clientId, string clientName, string clientDescription, string clientEmail, string clientContactPhone, string countryId, string clientPhoneticAlphabet)
//        {
//            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
//            if (efmvcUser != null)
//            {
//                // EFMVCDataContex db = new EFMVCDataContex();
//                NewClientFormModel model = new NewClientFormModel();
//                int ClientId = Convert.ToInt32(clientId);
//                NewAdvertFormModel model1 = new NewAdvertFormModel();
//                NewAdvertRejectionFormModel model2 = new NewAdvertRejectionFormModel();

//                if (clientId != "")
//                {
//                    try
//                    {
//                        int CountryId = 0;
//                        if (countryId == "12" || countryId == "13" || countryId == "14")
//                        {
//                            CountryId = 12;
//                        }
//                        else if (countryId == "11")
//                        {
//                            CountryId = 8;
//                        }
//                        else
//                        {
//                            CountryId = Convert.ToInt32(countryId);
//                        }
//                        var currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;
//                        var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;

//                        IEnumerable<Client> clientNameExists;
//                        if (ClientID == 0)
//                        {
//                            clientNameExists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId).ToList();
//                        }
//                        else
//                        {
//                            clientNameExists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.Id != ClientID).ToList();
//                        }
//                        if (clientNameExists.Count() > 0)
//                        {
//                            FillClient(efmvcUser.UserId);

//                            return Json("Exists");
//                        }
//                        else
//                        {
//                            var clientDetail = _clientRepository.GetById(Convert.ToInt32(clientId));
//                            model.ClientId = ClientID == 0 ? 0 : ClientID;
//                            model.UserId = efmvcUser.UserId;
//                            model.ClientName = clientName;
//                            model.ClientDescription = clientDescription;
//                            model.ClientContactInfo = "";
//                            model.ClientBudget = clientDetail.Budget;
//                            model.CreatedDate = (DateTime)clientDetail.CreatedDate;
//                            model.UpdatedDate = (DateTime)clientDetail.UpdatedDate;
//                            model.ClientStatus = (int)ClientStatus.InProgress;
//                            model.ClientEmail = clientEmail;
//                            model.ClientPhoneticAlphabet = clientPhoneticAlphabet;
//                            model.ClientContactPhone = clientContactPhone;
//                            model.NextStatus = true;
//                            model.CountryId = Convert.ToInt32(countryId);
//                            model.AdtoneServerClientId = null;

//                            CreateOrUpdateCopyClientCommand command =
//                                Mapper.Map<NewClientFormModel, CreateOrUpdateCopyClientCommand>(model);

//                            ICommandResult result = _commandBus.Submit(command);
//                            if (result.Success)
//                            {
//                                var ClientData = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == clientPhoneticAlphabet).ToList();
//                                if (ClientData.Count() > 0 && ClientData != null)
//                                {
//                                    ClientID = ClientData.FirstOrDefault().Id;
//                                }

//                                var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
//                                if (campaign != null)
//                                {
//                                    campaign.ClientId = result.Id;
//                                    db.SaveChanges();
//                                }

//                                return Json(new { success = "successbudget", value = currencyCode, value1 = currencyId }, JsonRequestBehavior.AllowGet);
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        TempData["Error"] = ex.InnerException.Message;
//                        return Json("fail");
//                    }
//                }
//                return Json("fail");
//            }
//            return RedirectToAction("Index", "Landing");
//        }

//        [AuthorizeFilter]
//        public ActionResult AddBudgetInfo(string campaignName, string countryId, string currencyId, string monthlyBudget, string weeklyBudget, string dailyBudget, string hourlyBudget, string maximumBid, string clientName)
//        {
//            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
//            if (efmvcUser != null)
//            {
//                if (campaignName != "" && maximumBid != "")
//                {
//                    try
//                    {
//                        int? CountryId = null;
//                        if (countryId != "") CountryId = Convert.ToInt32(countryId);
//                        var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);
//                        NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();
//                        // EFMVCDataContex db = new EFMVCDataContex();
//                        CurrencyModel currencyModel = new CurrencyModel();
//                        var client = _clientRepository.Get(c => c.Name == clientName);
//                        var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
//                        if (campaign != null)
//                        {
//                            if (client != null) campaign.ClientId = client.Id;
//                            //Code For Currency Conversion
//                            decimal currencyRate = 0.00M;
//                            int Countryid = Convert.ToInt32(countryId);
//                            int CurrencyId = Convert.ToInt32(currencyId);
//                            var currencyData = _currencyRepository.Get(c => c.CurrencyId == CurrencyId);
//                            var currencyCountryId = currencyData.Country.Id;
//                            var fromCurrencyCode = currencyData.CurrencyCode;
//                            var toCurrencyCode = _currencyRepository.Get(c => c.CountryId == Countryid).CurrencyCode;
//                            if (currencyCountryId == Countryid)
//                            {
//                                campaign.MaxDailyBudget = float.Parse(dailyBudget);
//                                campaign.MaxBid = float.Parse(maximumBid);
//                                campaign.MaxMonthBudget = float.Parse(monthlyBudget);
//                                campaign.MaxWeeklyBudget = float.Parse(weeklyBudget);
//                                campaign.MaxHourlyBudget = float.Parse(hourlyBudget);
//                                campaign.RemainingMaxDailyBudget = float.Parse(dailyBudget);
//                                campaign.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
//                                campaign.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
//                                campaign.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
//                            }
//                            else
//                            {
//                                //passed static 1 value to avoid multiple api call
//                                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
//                                currencyRate = currencyModel.Amount;
//                                if (currencyModel.Code == "OK")
//                                {
//                                    campaign.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                    campaign.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
//                                    campaign.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                    campaign.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                    campaign.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                    campaign.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                    campaign.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                    campaign.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                    campaign.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                }
//                                else
//                                {
//                                    return Json("fail");
//                                }
//                            }
//                            campaign.CurrencyCode = toCurrencyCode;
//                            campaign.UpdatedDateTime = System.DateTime.Now;
//                            db.SaveChanges();
//                            if (ConnString != null && ConnString.Count() > 0)
//                            {
//                                UserMatchTableProcess obj = new UserMatchTableProcess();
//                                foreach (var item in ConnString)
//                                {
//                                    EFMVCDataContex db1 = new EFMVCDataContex(item);
//                                    var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
//                                    if (campaignProfileDetails != null)
//                                    {
//                                        CampaignProfileFormModel campaignProfileFormModel = new CampaignProfileFormModel();
//                                        if (client != null)
//                                        {
//                                            var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, client.Id);
//                                            int? operatorClientId;
//                                            if (externalServerClientId == 0) operatorClientId = null;
//                                            else operatorClientId = externalServerClientId;
//                                            campaignProfileDetails.ClientId = operatorClientId;
//                                            campaignProfileFormModel.ClientId = operatorClientId;
//                                        }
//                                        if (currencyCountryId == Countryid)
//                                        {
//                                            campaignProfileDetails.MaxDailyBudget = float.Parse(dailyBudget);
//                                            campaignProfileDetails.MaxBid = float.Parse(maximumBid);
//                                            campaignProfileDetails.MaxMonthBudget = float.Parse(monthlyBudget);
//                                            campaignProfileDetails.MaxWeeklyBudget = float.Parse(weeklyBudget);
//                                            campaignProfileDetails.MaxHourlyBudget = float.Parse(hourlyBudget);
//                                            campaignProfileDetails.RemainingMaxDailyBudget = float.Parse(dailyBudget);
//                                            campaignProfileDetails.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
//                                            campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
//                                            campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
//                                            campaignProfileFormModel.MaxBid = float.Parse(maximumBid);
//                                            campaignProfileFormModel.MaxMonthBudget = float.Parse(monthlyBudget);
//                                            campaignProfileFormModel.MaxWeeklyBudget = float.Parse(weeklyBudget);
//                                            campaignProfileFormModel.MaxHourlyBudget = float.Parse(hourlyBudget);
//                                            campaignProfileFormModel.MaxDailyBudget = float.Parse(dailyBudget);
//                                        }
//                                        else
//                                        {
//                                            if (currencyModel.Code == "OK")
//                                            {
//                                                campaignProfileDetails.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                                campaignProfileDetails.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
//                                                campaignProfileDetails.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                                campaignProfileDetails.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                                campaignProfileDetails.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                                campaignProfileDetails.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                                campaignProfileDetails.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                                campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                                campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                                campaignProfileFormModel.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
//                                                campaignProfileFormModel.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                                campaignProfileFormModel.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                                campaignProfileFormModel.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                                campaignProfileFormModel.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                            }
//                                        }
//                                        campaignProfileDetails.CurrencyCode = toCurrencyCode;
//                                        campaignProfileDetails.UpdatedDateTime = System.DateTime.Now;
//                                        db1.SaveChanges();
//                                        obj.UpdateCampaignBudgetInfo(campaignProfileFormModel, campaignProfileDetails, efmvcUser.UserId, db1);
//                                        PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
//                                    }
//                                }
//                            }
//                            return Json("success");
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        TempData["Error"] = ex.InnerException.Message;
//                        return Json("fail");
//                    }
//                }
//                return Json("fail");
//            }
//            return RedirectToAction("Index", "Landing");
//        }

//        [AuthorizeFilter]
//        public ActionResult AddAdvertData(string campaignName, string countryId, string currencyId, string monthlyBudget, string weeklyBudget, string dailyBudget, string hourlyBudget, string maximumBid, string clientName)
//        {
//            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
//            if (efmvcUser != null)
//            {
//                try
//                {
//                    int? CountryId = null;
//                    if (countryId != "") CountryId = Convert.ToInt32(countryId);
//                    var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);
//                    // EFMVCDataContex db = new EFMVCDataContex();
//                    CurrencyModel currencyModel = new CurrencyModel();
//                    var client = _clientRepository.Get(c => c.Name == clientName && c.UserId == efmvcUser.UserId);
//                    var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
//                    if (campaign != null)
//                    {
//                        if (client != null) campaign.ClientId = client.Id;
//                        //Code For Currency Conversion
//                        decimal currencyRate = 0.00M;
//                        int Countryid = Convert.ToInt32(countryId);
//                        int CurrencyId = Convert.ToInt32(currencyId);
//                        var currencyData = _currencyRepository.Get(c => c.CurrencyId == CurrencyId);
//                        var currencyCountryId = currencyData.Country.Id;
//                        var fromCurrencyCode = currencyData.CurrencyCode;
//                        var toCurrencyCode = _currencyRepository.Get(c => c.CountryId == Countryid).CurrencyCode;
//                        if (currencyCountryId == Countryid)
//                        {
//                            campaign.MaxDailyBudget = float.Parse(dailyBudget);
//                            campaign.MaxBid = float.Parse(maximumBid);
//                            campaign.MaxMonthBudget = float.Parse(monthlyBudget);
//                            campaign.MaxWeeklyBudget = float.Parse(weeklyBudget);
//                            campaign.MaxHourlyBudget = float.Parse(hourlyBudget);
//                            campaign.RemainingMaxDailyBudget = float.Parse(dailyBudget);
//                            campaign.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
//                            campaign.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
//                            campaign.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
//                        }
//                        else
//                        {
//                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
//                            currencyRate = currencyModel.Amount;
//                            if (currencyModel.Code == "OK")
//                            {
//                                campaign.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                campaign.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
//                                campaign.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                campaign.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                campaign.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                campaign.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                campaign.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                campaign.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                campaign.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                            }
//                            else
//                            {
//                                return Json("fail");
//                            }
//                        }
//                        campaign.CurrencyCode = toCurrencyCode;
//                        campaign.UpdatedDateTime = System.DateTime.Now;
//                        db.SaveChanges();
//                        if (ConnString != null && ConnString.Count() > 0)
//                        {
//                            UserMatchTableProcess obj = new UserMatchTableProcess();
//                            foreach (var item in ConnString)
//                            {
//                                EFMVCDataContex db1 = new EFMVCDataContex(item);
//                                var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
//                                if (campaignProfileDetails != null)
//                                {
//                                    CampaignProfileFormModel campaignProfileFormModel = new CampaignProfileFormModel();
//                                    if (client != null)
//                                    {
//                                        var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, client.Id);
//                                        int? operatorClientId;
//                                        if (externalServerClientId == 0) operatorClientId = null;
//                                        else operatorClientId = externalServerClientId;
//                                        campaignProfileDetails.ClientId = operatorClientId;
//                                        campaignProfileFormModel.ClientId = operatorClientId;
//                                    }
//                                    if (currencyCountryId == Countryid)
//                                    {
//                                        campaignProfileDetails.MaxDailyBudget = float.Parse(dailyBudget);
//                                        campaignProfileDetails.MaxBid = float.Parse(maximumBid);
//                                        campaignProfileDetails.MaxMonthBudget = float.Parse(monthlyBudget);
//                                        campaignProfileDetails.MaxWeeklyBudget = float.Parse(weeklyBudget);
//                                        campaignProfileDetails.MaxHourlyBudget = float.Parse(hourlyBudget);
//                                        campaignProfileDetails.RemainingMaxDailyBudget = float.Parse(dailyBudget);
//                                        campaignProfileDetails.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
//                                        campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
//                                        campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
//                                        campaignProfileFormModel.MaxBid = float.Parse(maximumBid);
//                                        campaignProfileFormModel.MaxMonthBudget = float.Parse(monthlyBudget);
//                                        campaignProfileFormModel.MaxWeeklyBudget = float.Parse(weeklyBudget);
//                                        campaignProfileFormModel.MaxHourlyBudget = float.Parse(hourlyBudget);
//                                        campaignProfileFormModel.MaxDailyBudget = float.Parse(dailyBudget);
//                                    }
//                                    else
//                                    {
//                                        if (currencyModel.Code == "OK")
//                                        {
//                                            campaignProfileDetails.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                            campaignProfileDetails.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
//                                            campaignProfileDetails.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                            campaignProfileDetails.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                            campaignProfileDetails.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                            campaignProfileDetails.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                            campaignProfileDetails.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                            campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                            campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                            campaignProfileFormModel.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
//                                            campaignProfileFormModel.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
//                                            campaignProfileFormModel.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
//                                            campaignProfileFormModel.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
//                                            campaignProfileFormModel.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
//                                        }
//                                    }
//                                    campaignProfileDetails.CurrencyCode = toCurrencyCode;
//                                    campaignProfileDetails.UpdatedDateTime = System.DateTime.Now;
//                                    db1.SaveChanges();
//                                    obj.UpdateCampaignBudgetInfo(campaignProfileFormModel, campaignProfileDetails, efmvcUser.UserId, db1);
//                                    PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
//                                }
//                            }
//                        }
//                        var ClientDetails = _clientRepository.GetAll().Where(s => s.UserId == efmvcUser.UserId).Select(top => new SelectListItem { Text = top.Name, Value = top.Id.ToString() }).ToList();
//                        ClientDetails.Insert(0, new SelectListItem { Text = "-- Select Client --", Value = "" });
//                        var userData = _userRepository.GetById(efmvcUser.UserId);
//                        if (userData.OperatorId != 0)
//                        {
//                            var userCountryId = _operatorRepository.GetById(userData.OperatorId).CountryId.Value;
//                            FillAdvertCategory(userCountryId);
//                        }
//                        else
//                        {
//                            FillAdvertCategory(0);
//                        }
//                        var batch = 1;
//                        var countryData = _countryRepository.GetById(Convert.ToInt32(countryId.ToString())).TermAndConditionFileName;
//                        var tnc = "";
//                        if (!string.IsNullOrEmpty(countryData))
//                        {
//                            tnc = countryData;
//                        }
//                        return Json(new { success = "success", value2 = batch, value3 = tnc, value4 = campaign.ClientId, value5 = ClientDetails }, JsonRequestBehavior.AllowGet);
//                    }
//                    return Json("fail");
//                }
//                catch (Exception ex)
//                {
//                    TempData["Error"] = ex.InnerException.Message;
//                    return Json("fail");
//                }
//            }
//            return RedirectToAction("Index", "Landing");
//        }

//        [AuthorizeFilter]
//        public ActionResult AddAdvertInfo(string campaignName, string advertPhoneticAlphabet, string advertId, string advertName, string advertClientId, string advertBrandName, string advertCategoryId, string script, string numberofadsinabatch, HttpPostedFileBase mediaFile, HttpPostedFileBase scriptFile, string countryId, string operatorId)
//        {
//            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
//            if (efmvcUser != null)
//            {
//                string countryName = _countryRepository.GetById(Convert.ToInt32(countryId)).Name;
//                string operatorName = _operatorRepository.GetById(Convert.ToInt32(operatorId)).OperatorName;

//                NewAdvertFormModel model = new NewAdvertFormModel();
//                // EFMVCDataContex db = new EFMVCDataContex();
//                AdvertEmail advertEmail = new AdvertEmail(_commandBus, _userRepository);

//                int? CountryId = null;
//                if (countryId != "")
//                {
//                    CountryId = Convert.ToInt32(countryId);
//                }
//                var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

//                if (advertId != "")
//                {
//                    advertId = advertId == "" ? "0" : advertId;
//                    int advertID = Convert.ToInt32(advertId);
//                    IEnumerable<Advert> AdvertNameexists;
//                    if (advertId == "0")
//                    {
//                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId == advertID).ToList();
//                    }
//                    else
//                    {
//                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId != advertID).ToList();
//                    }
//                    if (AdvertNameexists.Count() > 0)
//                    {
//                        FillAdvertList(efmvcUser.UserId);
//                        FillClient(efmvcUser.UserId);
//                        FillAdvertCategory(efmvcUser.UserId);
//                        FillOperator(Convert.ToInt32(countryId));

//                        return Json("Exists");
//                    }
//                    else
//                    {
//                        var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
//                        int campaignId = 0;

//                        if (campaign.CampaignProfileId != 0 && campaign.CampaignProfileId != null)
//                        {
//                            campaignId = Convert.ToInt32(campaign.CampaignProfileId);
//                        }

//                        int? clientId = null;
//                        if (advertClientId == "")
//                        {
//                            clientId = null;
//                        }
//                        else
//                        {
//                            clientId = Convert.ToInt32(advertClientId);
//                        }

//                        var advertDetail = _advertRepository.GetById(Convert.ToInt32(advertId));
//                        model.AdvertId = 0;
//                        model.UserId = efmvcUser.UserId;
//                        model.AdvertClientId = clientId;
//                        model.AdvertName = advertName;
//                        model.BrandName = advertBrandName;
//                        model.UploadedToMediaServer = false;
//                        model.CreatedDateTime = DateTime.Now;
//                        model.UpdatedDateTime = DateTime.Now;
//                        model.Status = (int)AdvertStatus.Waitingforapproval;
//                        model.Script = script;
//                        model.IsAdminApproval = false;
//                        model.AdvertCategoryId = int.Parse(advertCategoryId);
//                        model.CountryId = int.Parse(countryId);
//                        model.PhoneticAlphabet = advertPhoneticAlphabet;
//                        model.NextStatus = false;
//                        model.CampProfileId = campaignId;
//                        model.AdtoneServerAdvertId = null;
//                        model.OperatorId = Convert.ToInt32(operatorId);

//                        #region Media
//                        if (mediaFile != null)
//                        {
//                            if (mediaFile.ContentLength != 0)
//                            {
//                                var userData = _userRepository.GetById(efmvcUser.UserId);
//                                var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

//                                string fileName = firstAudioName;

//                                string fileName2 = null;
//                                if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
//                                {
//                                    var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
//                                    fileName2 = secondAudioName.ToString();
//                                }

//                                string extension = Path.GetExtension(mediaFile.FileName);

//                                var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
//                                string outputFormat = "wav";
//                                var audioFormatExtension = "." + outputFormat;

//                                if (extension != audioFormatExtension)
//                                {
//                                    string tempDirectoryName = Server.MapPath("~/Media/Temp/");
//                                    string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
//                                    mediaFile.SaveAs(tempPath);

//                                    SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

//                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                        fileName + "." + outputFormat);
//                                }
//                                else
//                                {
//                                    string directoryName = Server.MapPath("~/Media/");
//                                    directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

//                                    if (!Directory.Exists(directoryName))
//                                        Directory.CreateDirectory(directoryName);

//                                    string path = Path.Combine(directoryName, fileName + extension);
//                                    mediaFile.SaveAs(path);

//                                    StoreSecondAudioFile(directoryName, fileName2, outputFormat, path);
//                                    string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

//                                    if (!Directory.Exists(archiveDirectoryName))
//                                        Directory.CreateDirectory(archiveDirectoryName);

//                                    string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
//                                    mediaFile.SaveAs(archivePath);

//                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                            fileName + extension);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

//                            string fileName = firstAudioName;

//                            string fileName2 = null;
//                            if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
//                            {
//                                var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
//                                fileName2 = secondAudioName.ToString();
//                            }

//                            string outputFormat = "wav";
//                            var audioFormatExtension = "." + outputFormat;
//                            var extension = advertDetail.MediaFileLocation == null ? "" : advertDetail.MediaFileLocation.Split('.').LastOrDefault();
//                            if (extension != "")
//                            {
//                                extension = "." + extension;
//                                if (extension != audioFormatExtension)
//                                {
//                                    var advertmediafilename = advertDetail.MediaFileLocation.Split('/').LastOrDefault();
//                                    string fileName1 = firstAudioName;
//                                    string sourcePath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());
//                                    string targetPath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());

//                                    string sourceFile = System.IO.Path.Combine(sourcePath, advertmediafilename);
//                                    string destFile = System.IO.Path.Combine(targetPath, fileName1 + extension);

//                                    if (!Directory.Exists(targetPath))
//                                        Directory.CreateDirectory(targetPath);
//                                    if (System.IO.File.Exists(sourceFile) == true)
//                                    {
//                                        System.IO.File.Copy(sourceFile, destFile, true);

//                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                        fileName1 + "." + outputFormat);
//                                    }

//                                    string tempDirectoryName = Server.MapPath("~/Media/Temp/");
//                                    string tempPath = Path.Combine(tempDirectoryName, fileName1 + extension);

//                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                        fileName1 + "." + outputFormat);
//                                }
//                                else
//                                {
//                                    var advertmediafilename = advertDetail.MediaFileLocation.Split('/').LastOrDefault();
//                                    string fileName1 = firstAudioName;
//                                    string sourcePath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());
//                                    string targetPath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());

//                                    string sourceFile = System.IO.Path.Combine(sourcePath, advertmediafilename);
//                                    string destFile = System.IO.Path.Combine(targetPath, fileName1 + extension);

//                                    if (!Directory.Exists(targetPath))
//                                        Directory.CreateDirectory(targetPath);
//                                    if (System.IO.File.Exists(sourceFile) == true)
//                                    {
//                                        System.IO.File.Copy(sourceFile, destFile, true);
//                                    }

//                                    string directoryName1 = Server.MapPath("~/Media/");
//                                    directoryName1 = Path.Combine(directoryName1, efmvcUser.UserId.ToString());

//                                    if (!Directory.Exists(directoryName1))
//                                        Directory.CreateDirectory(directoryName1);

//                                    string path1 = Path.Combine(directoryName1, fileName + extension);

//                                    string fileName3 = firstAudioName;
//                                    string sourcePath1 = Server.MapPath("~/Media/Archive/");
//                                    string targetPath1 = Server.MapPath("~/Media/Archive/");

//                                    string sourceFile1 = System.IO.Path.Combine(sourcePath1, advertmediafilename);
//                                    string destFile1 = System.IO.Path.Combine(targetPath1, fileName3 + extension);

//                                    if (!Directory.Exists(targetPath1))
//                                        Directory.CreateDirectory(targetPath1);

//                                    if (System.IO.File.Exists(sourceFile1) == true)
//                                    {
//                                        System.IO.File.Copy(sourceFile1, destFile1, true);
//                                    }

//                                    StoreSecondAudioFile(directoryName1, fileName2, outputFormat, path1);
//                                    string archiveDirectoryName1 = Server.MapPath("~/Media/Archive/");

//                                    if (!Directory.Exists(archiveDirectoryName1))
//                                        Directory.CreateDirectory(archiveDirectoryName1);

//                                    string archivePath1 = Path.Combine(archiveDirectoryName1, fileName + extension);

//                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                        fileName + "." + outputFormat);
//                                }
//                            }
//                            else
//                            {
//                                model.MediaFileLocation = advertDetail.MediaFileLocation;
//                            }
//                        }

//                        #endregion

//                        #region Script
//                        if (scriptFile != null)
//                        {
//                            if (scriptFile.ContentLength != 0)
//                            {
//                                string fileName = Guid.NewGuid().ToString();
//                                string extension = Path.GetExtension(scriptFile.FileName);

//                                string directoryName = Server.MapPath("/Script/");
//                                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

//                                if (!Directory.Exists(directoryName))
//                                    Directory.CreateDirectory(directoryName);

//                                string path = Path.Combine(directoryName, fileName + extension);
//                                scriptFile.SaveAs(path);

//                                string archiveDirectoryName = Server.MapPath("/Script/Archive/");

//                                if (!Directory.Exists(archiveDirectoryName))
//                                    Directory.CreateDirectory(archiveDirectoryName);

//                                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
//                                scriptFile.SaveAs(archivePath);

//                                model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                        fileName + extension);
//                            }
//                            else
//                            {
//                                model.ScriptFileLocation = "";
//                            }
//                        }
//                        else
//                        {
//                            string scriptfileName = Guid.NewGuid().ToString();
//                            string extension1 = advertDetail.ScriptFileLocation == null ? "" : advertDetail.ScriptFileLocation.Split('.').LastOrDefault();
//                            if (extension1 != "")
//                            {
//                                var advertscriptfilename = advertDetail.ScriptFileLocation.Split('/').LastOrDefault();
//                                string fileName1 = scriptfileName;
//                                string sourcePath = Server.MapPath("/Script/" + efmvcUser.UserId.ToString());
//                                string targetPath = Server.MapPath("/Script/" + efmvcUser.UserId.ToString());

//                                string sourceFile = System.IO.Path.Combine(sourcePath, advertscriptfilename);
//                                string destFile = System.IO.Path.Combine(targetPath, advertscriptfilename);

//                                if (!Directory.Exists(targetPath))
//                                    Directory.CreateDirectory(targetPath);

//                                if (System.IO.File.Exists(sourceFile) == true)
//                                {
//                                    System.IO.File.Copy(sourceFile, destFile, true);

//                                    System.IO.File.Delete(fileName1 + "." + extension1); // Delete the existing file if exists
//                                    System.IO.File.Move(advertscriptfilename, fileName1 + "." + extension1);
//                                }

//                                string directoryName = Server.MapPath("/Script/");
//                                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

//                                if (!Directory.Exists(directoryName))
//                                    Directory.CreateDirectory(directoryName);

//                                string path = Path.Combine(directoryName, scriptfileName + extension1);

//                                string fileName3 = scriptfileName;
//                                string sourcePath1 = Server.MapPath("/Script/Archive/");
//                                string targetPath1 = Server.MapPath("/Script/Archive/");

//                                string sourceFile1 = System.IO.Path.Combine(sourcePath1, advertscriptfilename);
//                                string destFile1 = System.IO.Path.Combine(targetPath1, advertscriptfilename);

//                                if (!Directory.Exists(targetPath1))
//                                    Directory.CreateDirectory(targetPath1);

//                                if (System.IO.File.Exists(sourceFile1) == true)
//                                {
//                                    System.IO.File.Copy(sourceFile1, destFile1, true);

//                                    System.IO.File.Delete(fileName3 + "." + extension1); // Delete the existing file if exists
//                                    System.IO.File.Move(advertscriptfilename, fileName3 + "." + extension1);
//                                }

//                                string archiveDirectoryName = Server.MapPath("/Script/Archive/");

//                                if (!Directory.Exists(archiveDirectoryName))
//                                    Directory.CreateDirectory(archiveDirectoryName);

//                                string archivePath = Path.Combine(archiveDirectoryName, scriptfileName + extension1);

//                                model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                        scriptfileName + extension1);
//                            }
//                            else
//                            {
//                                model.ScriptFileLocation = advertDetail.ScriptFileLocation;
//                            }
//                        }

//                        #endregion

//                        CreateOrUpdateCopyAdvertCommand command =
//                            Mapper.Map<NewAdvertFormModel, CreateOrUpdateCopyAdvertCommand>(model);

//                        ICommandResult result = _commandBus.Submit(command);

//                        if (result.Success)
//                        {
//                            if (campaignId != 0)
//                            {
//                                CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
//                                _campaignAdvert.AdvertId = result.Id;
//                                _campaignAdvert.CampaignProfileId = campaignId;
//                                _campaignAdvert.NextStatus = true;
//                                CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
//                                Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

//                                ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);

//                                if (campaignAdvertcommandResult.Success)
//                                {
//                                    if (campaign != null)
//                                    {
//                                        campaign.NumberInBatch = int.Parse(numberofadsinabatch);
//                                        campaign.UpdatedDateTime = DateTime.Now;
//                                        db.SaveChanges();
//                                        if (ConnString != null && ConnString.Count() > 0)
//                                        {
//                                            UserMatchTableProcess obj = new UserMatchTableProcess();
//                                            string adName = "";
//                                            if (command.MediaFileLocation == null || command.MediaFileLocation == "")
//                                            {
//                                                adName = "";
//                                            }
//                                            else
//                                            {
//                                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
//                                                var advertOperatorId = _advertRepository.GetById(result.Id).OperatorId;
//                                                var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
//                                                adName = operatorFTPDetails.FtpRoot + "/" + command.MediaFileLocation.Split('/')[3];
//                                            }
//                                            foreach (var item in ConnString)
//                                            {
//                                                EFMVCDataContex db1 = new EFMVCDataContex(item);
//                                                var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
//                                                if (campaignProfileDetails != null)
//                                                {
//                                                    campaignProfileDetails.NumberInBatch = int.Parse(numberofadsinabatch);
//                                                    campaignProfileDetails.UpdatedDateTime = DateTime.Now;
//                                                    db1.SaveChanges();

//                                                    obj.UpdateCampaignAd(campaignProfileDetails.CampaignProfileId, adName, db1);
//                                                    PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
//                                                }
//                                            }
//                                        }
//                                        //Email Code
//                                        //advertEmail.SendMail(advertName, model.OperatorId);
//                                        advertEmail.SendMail(advertName, model.OperatorId, efmvcUser.UserId, campaignName, countryName, operatorName, DateTime.Now);
//                                        return Json("success");
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    advertId = advertId == "" ? "0" : advertId;
//                    IEnumerable<Advert> AdvertNameexists;
//                    if (advertId == "0")
//                    {
//                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId == Convert.ToInt32(advertId)).ToList();
//                    }
//                    else
//                    {
//                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId != Convert.ToInt32(advertId)).ToList();
//                    }
//                    if (AdvertNameexists.Count() > 0)
//                    {
//                        FillClient(efmvcUser.UserId);
//                        FillAdvertCategory(efmvcUser.UserId);
//                        FillOperator(Convert.ToInt32(countryId));

//                        return Json("Exists");
//                    }
//                    else
//                    {
//                        try
//                        {
//                            #region Media
//                            if (mediaFile != null)
//                            {
//                                if (mediaFile.ContentLength != 0)
//                                {
//                                    var userData = _userRepository.GetById(efmvcUser.UserId);

//                                    var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

//                                    string fileName = firstAudioName;

//                                    string fileName2 = null;
//                                    if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
//                                    {
//                                        var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
//                                        fileName2 = secondAudioName.ToString();
//                                    }

//                                    string extension = Path.GetExtension(mediaFile.FileName);

//                                    var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
//                                    string outputFormat = "wav";
//                                    var audioFormatExtension = "." + outputFormat;

//                                    if (extension != audioFormatExtension)
//                                    {
//                                        string tempDirectoryName = Server.MapPath("~/Media/Temp/");
//                                        string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
//                                        mediaFile.SaveAs(tempPath);

//                                        SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

//                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                            fileName + "." + outputFormat);
//                                    }
//                                    else
//                                    {
//                                        string directoryName = Server.MapPath("~/Media/");
//                                        directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

//                                        if (!Directory.Exists(directoryName))
//                                            Directory.CreateDirectory(directoryName);

//                                        string path = Path.Combine(directoryName, fileName + extension);
//                                        mediaFile.SaveAs(path);

//                                        StoreSecondAudioFile(directoryName, fileName2, outputFormat, path);
//                                        string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

//                                        if (!Directory.Exists(archiveDirectoryName))
//                                            Directory.CreateDirectory(archiveDirectoryName);

//                                        string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
//                                        mediaFile.SaveAs(archivePath);

//                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                                fileName + extension);
//                                    }
//                                }
//                            }
//                            #endregion

//                            #region Script

//                            if (scriptFile != null)
//                            {
//                                if (scriptFile.ContentLength != 0)
//                                {
//                                    string fileName = Guid.NewGuid().ToString();
//                                    string extension = Path.GetExtension(scriptFile.FileName);

//                                    string directoryName = Server.MapPath("/Script/");
//                                    directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

//                                    if (!Directory.Exists(directoryName))
//                                        Directory.CreateDirectory(directoryName);

//                                    string path = Path.Combine(directoryName, fileName + extension);
//                                    scriptFile.SaveAs(path);

//                                    string archiveDirectoryName = Server.MapPath("/Script/Archive/");

//                                    if (!Directory.Exists(archiveDirectoryName))
//                                        Directory.CreateDirectory(archiveDirectoryName);

//                                    string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
//                                    scriptFile.SaveAs(archivePath);

//                                    model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
//                                                                            fileName + extension);
//                                }
//                                else
//                                {
//                                    model.ScriptFileLocation = "";
//                                }
//                            }
//                            else
//                            {
//                                model.ScriptFileLocation = "";
//                            }
//                            #endregion

//                            #region Add Records

//                            var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
//                            int campaignId = 0;

//                            if (campaign.CampaignProfileId != 0 && campaign.CampaignProfileId != null)
//                            {
//                                campaignId = Convert.ToInt32(campaign.CampaignProfileId);
//                            }

//                            int? clientId = null;
//                            if (advertClientId == "")
//                            {
//                                clientId = null;
//                            }
//                            else
//                            {
//                                clientId = Convert.ToInt32(advertClientId);
//                            }

//                            if (ModelState.IsValid)
//                            {
//                                model.AdvertId = 0;
//                                model.UserId = efmvcUser.UserId;
//                                model.AdvertClientId = clientId;
//                                model.AdvertName = advertName;
//                                model.BrandName = advertBrandName;
//                                model.UploadedToMediaServer = false;
//                                model.CreatedDateTime = DateTime.Now;
//                                model.UpdatedDateTime = DateTime.Now;
//                                model.Status = (int)AdvertStatus.Waitingforapproval;
//                                model.Script = script;
//                                model.IsAdminApproval = false;
//                                model.AdvertCategoryId = int.Parse(advertCategoryId);
//                                model.CountryId = int.Parse(countryId);
//                                model.PhoneticAlphabet = advertPhoneticAlphabet;
//                                model.NextStatus = false;
//                                model.CampProfileId = campaignId;
//                                model.AdtoneServerAdvertId = null;
//                                model.OperatorId = Convert.ToInt32(operatorId);

//                                CreateOrUpdateCopyAdvertCommand command = Mapper.Map<NewAdvertFormModel, CreateOrUpdateCopyAdvertCommand>(model);

//                                ICommandResult result = _commandBus.Submit(command);

//                                if (result.Success)
//                                {
//                                    if (campaignId != 0)
//                                    {
//                                        CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
//                                        _campaignAdvert.AdvertId = result.Id;
//                                        _campaignAdvert.CampaignProfileId = campaignId;
//                                        _campaignAdvert.NextStatus = true;
//                                        CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
//                                        Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

//                                        ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);

//                                        if (campaignAdvertcommandResult.Success)
//                                        {
//                                            if (campaign != null)
//                                            {
//                                                campaign.NumberInBatch = int.Parse(numberofadsinabatch);
//                                                campaign.UpdatedDateTime = DateTime.Now;
//                                                db.SaveChanges();
//                                                if (ConnString != null && ConnString.Count() > 0)
//                                                {
//                                                    UserMatchTableProcess obj = new UserMatchTableProcess();

//                                                    string adName = "";
//                                                    if (command.MediaFileLocation == null || command.MediaFileLocation == "")
//                                                    {
//                                                        adName = "";
//                                                    }
//                                                    else
//                                                    {

//                                                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
//                                                        var advertOperatorId = _advertRepository.GetById(result.Id).OperatorId;
//                                                        var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
//                                                        adName = operatorFTPDetails.FtpRoot + "/" + command.MediaFileLocation.Split('/')[3];
//                                                    }

//                                                    foreach (var item in ConnString)
//                                                    {
//                                                        EFMVCDataContex db1 = new EFMVCDataContex(item);
//                                                        var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
//                                                        if (campaignProfileDetails != null)
//                                                        {
//                                                            campaignProfileDetails.NumberInBatch = int.Parse(numberofadsinabatch);
//                                                            campaignProfileDetails.UpdatedDateTime = DateTime.Now;
//                                                            db1.SaveChanges();

//                                                            obj.UpdateCampaignAd(campaignProfileDetails.CampaignProfileId, adName, db1);
//                                                            PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
//                                                        }
//                                                    }
//                                                }
//                                                //Email Code
//                                                //advertEmail.SendMail(advertName, model.OperatorId);
//                                                advertEmail.SendMail(advertName, model.OperatorId, efmvcUser.UserId, campaignName, countryName, operatorName, DateTime.Now);
//                                                return Json("success");
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            #endregion
//                        }
//                        catch (Exception ex)
//                        {
//                            TempData["Error"] = ex.InnerException.Message;
//                            return Json("fail");
//                        }
//                    }
//                }
//                return Json("fail");
//            }
//            return RedirectToAction("Index", "Landing");
//        }

//    }
//}
