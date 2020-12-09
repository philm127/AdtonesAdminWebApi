using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Services.Mailer;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CreateUpdateCampaignService : ICreateUpdateCampaignService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ICurrencyDAL _currencyRepository;
        private readonly ICampaignDAL _campaignDAL;
        private readonly ICreateUpdateCampaignDAL _createDAL;
        private readonly IConnectionStringService _connService;
        private readonly IPrematchProcess _matchProcess;
        private readonly IUserMatchDAL _matchDAL;
        private readonly IAdvertDAL _advertDAL;
        private readonly ISaveGetFiles _saveFile;
        private readonly IConvertSaveMediaFile _convFile;
        private readonly IAdvertEmail _adEmail;
        ReturnResult result = new ReturnResult();


        public CreateUpdateCampaignService(IHttpContextAccessor httpAccessor, ICurrencyDAL currencyRepository, ICampaignDAL campaignDAL,
                                            ICreateUpdateCampaignDAL createDAL, IConnectionStringService connService, IPrematchProcess matchProcess,
                                            IUserMatchDAL matchDAL, IAdvertDAL advertDAL, ISaveGetFiles saveFile, IConvertSaveMediaFile convFile,
                                            IAdvertEmail adEmail)
        {
            _httpAccessor = httpAccessor;
            _currencyRepository = currencyRepository;
            _campaignDAL = campaignDAL;
            _createDAL = createDAL;
            _connService = connService;
            _matchProcess = matchProcess;
            _matchDAL = matchDAL;
            _advertDAL = advertDAL;
            _saveFile = saveFile;
            _convFile = convFile;
            _adEmail = adEmail;
        }


        /// <summary>
        /// Gets initial data to populate the create campaign stepper.
        /// </summary>
        /// <returns></returns>
        //public async Task<ReturnResult> GetInitialData(int advertiserId = 0)
        //{
        //    try
        //    {
        //        result.body = await _advertDAL.GetAdvertResultSet(advertiserId);

        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "CreateUpdateCampaignService",
        //            ProcedureName = "GetInitialData"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //    }
        //    return result;
        //}


        public async Task<ReturnResult> CreateNewCampaign(NewCampaignProfileFormModel model)
        {
            var CampaignNameexists = await _campaignDAL.CheckCampaignNameExists(model.CampaignName, model.UserId);
            if (CampaignNameexists)
            {
                result.result = 0;
                result.body = "The Campaign name already exists";
                return result;
            }

            try
            {

                var currencyData = await _currencyRepository.GetCurrencyUsingCurrencyIdAsync(model.CurrencyId);
                var currencyCountryData = await _currencyRepository.GetCurrencyUsingCountryIdAsync(model.CountryId);
                decimal currencyRate = 1.00M;
                var fromCurrencyCode = currencyData.CurrencyCode;
                var toCurrencyCode = currencyCountryData.CurrencyCode;
                if (fromCurrencyCode != toCurrencyCode)
                {
                    currencyRate = 2;
                }

                model.CreatedDateTime = DateTime.Now;
                model.UpdatedDateTime = DateTime.Now;

                if (model.CountryId == 12 || model.CountryId == 13 || model.CountryId == 14)
                    model.CountryId = 12;
                else if (model.CountryId == 11)
                    model.CountryId = 8;

                

                model.MaxDailyBudget = float.Parse((Convert.ToDecimal(model.MaxDailyBudget) * currencyRate).ToString());
                model.MaxBid = float.Parse((Convert.ToDecimal(model.MaxBid) * currencyRate).ToString());
                model.MaxHourlyBudget = float.Parse((Convert.ToDecimal(model.MaxHourlyBudget) * currencyRate).ToString());
                model.MaxMonthBudget = float.Parse((Convert.ToDecimal(model.MaxMonthBudget) * currencyRate).ToString());
                model.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(model.MaxWeeklyBudget) * currencyRate).ToString());

                model.EmailFileLocation = null;
                model.Active = true;
                model.Status = (int)Enums.CampaignStatus.InProgress;
                model.IsAdminApproval = false;
                model.PhoneticAlphabet = PhoneticString();
                model.NextStatus = true;
                model.CurrencyCode = currencyCountryData.CurrencyCode;

                var newModel = new NewCampaignProfileFormModel();
                try
                {
                    newModel = await _createDAL.CreateNewCampaign(model);

                    // Is actually the CampaignProfileId from 168 Main Server
                    result.body = newModel.AdtoneServerCampaignProfileId;
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "CreateUpdateCampaignService",
                        ProcedureName = "CreateNewCampaign - AddDataToMain"
                    };
                    _logging.LogError();
                    result.result = 0;
                    return result;
                }
                try
                {
                    var timesettings = await CampaignProfileTimeSettingMapping(newModel.AdtoneServerCampaignProfileId.Value, model.CountryId, newModel.CampaignProfileId);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "CreateUpdateCampaignService",
                        ProcedureName = "CreateNewCampaign - Update Time Settings"
                    };
                    _logging.LogError();
                    result.result = 0;
                    return result;
                }
                var operatorString = await _connService.GetConnectionStringsByCountryId(model.CountryId);

                try
                {
                    var z = await _matchDAL.AddCampaignData(newModel, operatorString);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "CreateUpdateCampaignService",
                        ProcedureName = "CreateNewCampaign - AddUserMatch Data"
                    };
                    _logging.LogError();
                    result.result = 0;
                    return result;
                }
                try
                {
                    await _matchProcess.PrematchProcessForCampaign(newModel.CampaignProfileId, operatorString);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "CreateUpdateCampaignService",
                        ProcedureName = "CreateNewCampaign - PrematchProcessForCampaign"
                    };
                    _logging.LogError();
                    result.result = 0;
                    return result;
                }

                // Supposed to send confirmation Email to Admin user but admin has no email address


            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CreateUpdateCampaignService",
                    ProcedureName = "CreateNewCampaign"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> CreateNewCampaign_Advert(NewAdvertFormModel model)
        {

            var AdvertNameexists = await _advertDAL.CheckAdvertNameExists(model.AdvertName, model.AdvertiserId);

            if (AdvertNameexists)
            {
                result.result = 0;
                result.body = "The Advert Name already exists";
                return result;
            }
            if (model.file.Count == 0)
            {
                result.result = 0;
                result.body = "A media file is required to proceed";
                return result;
            }

            IFormFile mediaFile = model.MediaFile;
            IFormFile scriptFile = model.ScriptFile;

            #region Media
            if (mediaFile.Length > 0)
            {

                var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                string fileName = firstAudioName;

                string fileName2 = null;
                if (Convert.ToInt32(model.OperatorId) == (int)Enums.OperatorTableId.Safaricom)
                {
                    var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                    fileName2 = secondAudioName.ToString();
                }

                string extension = Path.GetExtension(mediaFile.FileName);
                var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                string outputFormat = "wav";

                var audioFormatExtension = "." + outputFormat;
                string actualDirectoryName = "Media";
                string directoryName = Path.Combine(actualDirectoryName, model.OperatorId.ToString());
                string newfile = string.Empty;

                if (extension != audioFormatExtension)
                {
                    string tempDirectoryName = @"Media\Temp\";
                    string tempFile = await _saveFile.SaveFileToSite(tempDirectoryName, mediaFile);

                    newfile = _convFile.ConvertAndSaveMediaFile(tempDirectoryName + tempFile, extension, outputFormat, onlyFileName, directoryName);

                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", model.AdvertiserId.ToString(),
                                                                            fileName + "." + outputFormat);
                }
                else
                {
                    newfile = await _saveFile.SaveFileToSite(directoryName, mediaFile);

                    string archiveDirectoryName = "Media//Archive";

                    string apath = await _saveFile.SaveOriginalFileToSite(archiveDirectoryName, mediaFile);


                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", model.AdvertiserId.ToString(),
                                                                                fileName + extension);
                }
            }
            else
            {
                result.result = 0;
                result.error = "Advert media file must be included please update the campaign";
                return result;
            }

            #endregion

            #region Script

            if (scriptFile != null && scriptFile.Length != 0)
            {

                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(scriptFile.FileName);

                string directoryName = "Script";
                directoryName = Path.Combine(directoryName, model.AdvertiserId.ToString());

                string path = Path.Combine(directoryName, fileName + extension);
                await _saveFile.SaveFileToSite(directoryName, mediaFile);

                string archiveDirectoryName = "Script/Archive/";


                await _saveFile.SaveFileToSite(archiveDirectoryName, mediaFile);

                model.ScriptFileLocation = string.Format("/Script/{0}/{1}", model.AdvertiserId.ToString(),
                                                                        fileName + extension);

            }
            else
            {
                model.ScriptFileLocation = "";
            }
            #endregion

            #region Add Records

            var campaign = await _campaignDAL.GetCampaignProfileDetail(model.CampaignProfileId.Value);
            int campaignId = 0;

            if (campaign != null)
            {
                campaignId = Convert.ToInt32(campaign.CampaignProfileId);
            }

            int? clientId = null;
            if (campaign.ClientId == 0 || campaign.ClientId != null)
            {
                model.ClientId = null;
            }

            model.UploadedToMediaServer = false;
            model.Status = (int)Enums.AdvertStatus.Waitingforapproval;
            model.IsAdminApproval = false;
            model.CountryId = campaign.CountryId.Value;
            model.PhoneticAlphabet = PhoneticString();
            model.NextStatus = false;
            model.AdtoneServerAdvertId = null;

            var newModel = await _createDAL.CreateNewCampaignAdvert(model);

            CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
            _campaignAdvert.AdvertId = newModel.AdtoneServerAdvertId.Value;
            _campaignAdvert.CampaignProfileId = model.CampaignProfileId.Value;
            _campaignAdvert.NextStatus = true;
            _campaignAdvert.AdtoneServerCampaignAdvertId = null;
            var newAdCamp = await _createDAL.CreateNewIntoCampaignAdverts(_campaignAdvert, model.OperatorId, newModel.AdvertId);

            if (newAdCamp != null)
            {
                if (campaign != null)
                {

                    string adName = "";
                    if (model.MediaFileLocation == null || model.MediaFileLocation == "")
                    {
                        adName = "";
                    }
                    else
                    {

                        var operatorFTPDetails = await _advertDAL.GetFtpDetails(model.OperatorId);
                        adName = operatorFTPDetails.FtpRoot + "/" + model.MediaFileLocation.Split('/')[3];
                    }
                    var ConnString = await _connService.GetConnectionStringByOperator(model.OperatorId);


                    var campaignProfileDetails = await _connService.GetCampaignProfileIdFromAdtoneId(model.CampaignProfileId.Value, model.OperatorId);
                    if (campaignProfileDetails != 0)
                    {
                        await _matchDAL.UpdateMediaLocation(ConnString, adName, campaignProfileDetails);
                        await _matchProcess.PrematchProcessForCampaign(campaignProfileDetails, ConnString);
                    }
                    _adEmail.SendMail(model.AdvertName, model.OperatorId, model.AdvertiserId, "campaignName", "countryName", "operatorName", DateTime.Now);
                }
            }
            #endregion

            return result;
        }

        


        private async Task<bool> CampaignProfileTimeSettingMapping(int campaignId, int countryId, int provCampId)
        {
            PostedTimesModel postedTimesModel = new PostedTimesModel();
            postedTimesModel.DayIds = new string[] { "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "24:00" };

            CampaignProfileTimeSetting timeSettings = new CampaignProfileTimeSetting();
            var model = new CampaignProfileTimeSettingFormModel{ CampaignProfileId = campaignId, AvailableTimes = WizardGetTimes() };

            //Update Campaign Profile Time Setting

            model.MondayPostedTimes = postedTimesModel;
            model.TuesdayPostedTimes = postedTimesModel;
            model.WednesdayPostedTimes = postedTimesModel;
            model.ThursdayPostedTimes = postedTimesModel;
            model.FridayPostedTimes = postedTimesModel;
            model.SaturdayPostedTimes = postedTimesModel;
            model.SundayPostedTimes = postedTimesModel;
            

                timeSettings = new CampaignProfileTimeSetting();
                timeSettings.Monday = string.Empty;
                foreach (string s in model.MondayPostedTimes.DayIds)
                    timeSettings.Monday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Monday))
                    timeSettings.Monday = timeSettings.Monday.Substring(0, timeSettings.Monday.Length - 1);

                timeSettings.Tuesday = string.Empty;
                foreach (string s in model.TuesdayPostedTimes.DayIds)
                    timeSettings.Tuesday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Tuesday))
                    timeSettings.Tuesday = timeSettings.Tuesday.Substring(0, timeSettings.Tuesday.Length - 1);

                timeSettings.Wednesday = string.Empty;
                foreach (string s in model.WednesdayPostedTimes.DayIds)
                    timeSettings.Wednesday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Wednesday))
                    timeSettings.Wednesday = timeSettings.Wednesday.Substring(0, timeSettings.Wednesday.Length - 1);

                timeSettings.Thursday = string.Empty;
                foreach (string s in model.ThursdayPostedTimes.DayIds)
                    timeSettings.Thursday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Thursday))
                    timeSettings.Thursday = timeSettings.Thursday.Substring(0, timeSettings.Thursday.Length - 1);

                timeSettings.Friday = string.Empty;
                foreach (string s in model.FridayPostedTimes.DayIds)
                    timeSettings.Friday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Friday))
                    timeSettings.Friday = timeSettings.Friday.Substring(0, timeSettings.Friday.Length - 1);

                timeSettings.Saturday = string.Empty;
                foreach (string s in model.SaturdayPostedTimes.DayIds)
                    timeSettings.Saturday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Saturday))
                    timeSettings.Saturday = timeSettings.Saturday.Substring(0, timeSettings.Saturday.Length - 1);

                timeSettings.Sunday = string.Empty;
                foreach (string s in model.SundayPostedTimes.DayIds)
                    timeSettings.Sunday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Sunday))
                    timeSettings.Sunday = timeSettings.Sunday.Substring(0, timeSettings.Sunday.Length - 1);

                timeSettings.CampaignProfileId = model.CampaignProfileId;
                timeSettings.CampaignProfileTimeSettingsId = model.CampaignProfileTimeSettingsId;

            var x = await _createDAL.AddProfileTimeSettings(timeSettings, countryId, provCampId);

            return true;

            }

        public IList<TimeOfDay> WizardGetTimes()
        {
            IList<TimeOfDay> times = new List<TimeOfDay>();
            times.Add(new TimeOfDay { Id = "01:00", Name = "01:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "02:00", Name = "02:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "03:00", Name = "03:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "04:00", Name = "04:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "05:00", Name = "05:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "06:00", Name = "06:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "07:00", Name = "07:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "08:00", Name = "08:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "09:00", Name = "09:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "10:00", Name = "10:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "11:00", Name = "11:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "12:00", Name = "12:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "13:00", Name = "13:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "14:00", Name = "14:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "15:00", Name = "15:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "16:00", Name = "16:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "17:00", Name = "17:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "18:00", Name = "18:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "19:00", Name = "19:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "20:00", Name = "20:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "21:00", Name = "21:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "22:00", Name = "22:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "23:00", Name = "23:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "24:00", Name = "24:00", IsSelected = true });

            return times;
        }


        private static string PhoneticString()
        {
            var str1 = RandomString(3);
            var str2 = RandomString(3);
            var str3 = RandomString(3);

            string phonetic = phonetic = str1.ToLower() + "-" + str2.ToLower() + "-" + str3.ToLower();

            return phonetic;
        }


        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
