﻿using AdtonesAdminWebApi.BusinessServices.Interfaces;
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
using AdtonesAdminWebApi.DAL;

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
        private readonly IAdvertService _advertService;
        private readonly ICreateCheckSaveProfileModels _profileService;
        private readonly IUserManagementDAL _userDAL;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        const string PageName = "CreateUpdateCampaignService";

        public CreateUpdateCampaignService(IHttpContextAccessor httpAccessor, ICurrencyDAL currencyRepository, ICampaignDAL campaignDAL,
                                            ICreateUpdateCampaignDAL createDAL, IConnectionStringService connService, IPrematchProcess matchProcess,
                                            IUserMatchDAL matchDAL, IAdvertDAL advertDAL, ISaveGetFiles saveFile, IConvertSaveMediaFile convFile,
                                            IAdvertEmail adEmail, IAdvertService advertService, ICreateCheckSaveProfileModels profileService,
                                            IUserManagementDAL userDAL, ILoggingService logServ)
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
            _advertService = advertService;
            _profileService = profileService;
            _userDAL = userDAL;
            _logServ = logServ;
        }


        /// <summary>
        /// Gets initial profile models to populate the create campaign stepper.
        /// </summary>
        /// <param name="countryId">countryid</param>
        /// <returns></returns>
        public async Task<ReturnResult> GetInitialData(int countryId,int advertiserId = 0)
        {
            try
            {
                var model = new NewAdProfileMappingFormModel();
                model.CampaignProfileGeographicModel = _profileService.GetGeographicModel(countryId);
                model.CampaignProfileDemographicsmodel = _profileService.GetDemographicModel(countryId);
                model.CampaignProfileTimeSettingModel = _profileService.GetTimeSettingModel(0);
                model.CampaignProfileMobileFormModel = await _profileService.GetMobileModel(countryId);
                if(countryId == 10)
                    model.CampaignProfileSkizaFormModel = await _profileService.GetQuestionnaireModel(countryId);

                model.CampaignProfileAd = await _profileService.GetAdvertProfileModel(countryId);

                result.body = model;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetInitialData";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetProfileData(int campaignId, int advertiserId = 0)
        {
            try
            {
                var campaignDetails = await _campaignDAL.GetCampaignProfileDetail(campaignId);
                if (campaignDetails != null)
                {
                    int countryId = campaignDetails.CountryId.Value;
                    CampaignProfilePreference CampaignProfilePreferences = await _matchDAL.GetCampaignProfilePreferenceDetailsByCampaignId(campaignId);
                    var model = new NewAdProfileMappingFormModel();
                    if (CampaignProfilePreferences != null)
                    {
                        model.CampaignProfileGeographicModel = await _profileService.GetGeographicData(campaignId, CampaignProfilePreferences);
                        model.CampaignProfileDemographicsmodel = await _profileService.GetDemographicData(campaignId, CampaignProfilePreferences);
                        model.CampaignProfileMobileFormModel = await _profileService.GetMobileData(campaignId, CampaignProfilePreferences);
                        if (countryId == 10)
                            model.CampaignProfileSkizaFormModel = await _profileService.GetQuestionnaireData(campaignId, CampaignProfilePreferences);
                        model.CampaignProfileAd = await _profileService.GetAdvertProfileData(campaignId, CampaignProfilePreferences);
                    }

                    model.CampaignProfileTimeSettingModel = await _profileService.GetTimeSettingData(campaignId);
                    model.CampaignProfileId = CampaignProfilePreferences.CampaignProfileId;
                    model.CountryId = CampaignProfilePreferences.CountryId;
                    model.Id = CampaignProfilePreferences.Id;

                    result.body = model;
                }
                else
                {
                    return await GetInitialData(10);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetProfileData";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetCampaignData(int campaignId)
        {
            CampaignProfileUpdate model = await _campaignDAL.GetCampaignProfileDetailUpdate(campaignId);
            if (model.NumberInBatch == 0) model.NumberInBatch = 1;

            CampaignProfileFormModel map = new CampaignProfileFormModel();

            result.body = model;

            return result;
        }


        public async Task<ReturnResult> CreateNewCampaign(NewCampaignProfileFormModel model)
        {
            var CampaignNameexists = await _campaignDAL.CheckCampaignNameExists(model.CampaignName, model.UserId);
            if (CampaignNameexists)
            {
                result.result = 0;
                result.error = "The Campaign name already exists";
                return result;
            }

            try
            {
                if (model.CountryId == 12 || model.CountryId == 13 || model.CountryId == 14)
                    model.CountryId = 12;
                else if (model.CountryId == 11)
                    model.CountryId = 8;

                var currencyCountryData = await _currencyRepository.GetCurrencyUsingCountryIdAsync(model.CountryId);
                decimal currencyRate = 1.00M;
                if (model.CurrencyId != 0)
                {
                    var currencyData = await _currencyRepository.GetCurrencyUsingCurrencyIdAsync(model.CurrencyId);
                    var fromCurrencyCode = currencyData.CurrencyCode;
                    var toCurrencyCode = currencyCountryData.CurrencyCode;
                    if (fromCurrencyCode != toCurrencyCode)
                    {
                        currencyRate = 2;
                    }
                }

                model.CreatedDateTime = DateTime.Now;
                model.UpdatedDateTime = DateTime.Now;

                model.MaxDailyBudget = float.Parse((Convert.ToDecimal(model.MaxDailyBudget) * currencyRate).ToString());
                model.MaxBid = float.Parse((Convert.ToDecimal(model.MaxBid) * currencyRate).ToString());
                model.MaxHourlyBudget = float.Parse((Convert.ToDecimal(model.MaxHourlyBudget) * currencyRate).ToString());
                model.MaxMonthBudget = float.Parse((Convert.ToDecimal(model.MaxMonthBudget) * currencyRate).ToString());
                model.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(model.MaxWeeklyBudget) * currencyRate).ToString());

                model.EmailFileLocation = null;
                model.Active = true;
                model.IsAdminApproval = true;
                model.PhoneticAlphabet = PhoneticString();
                model.NextStatus = false;
                model.CurrencyCode = currencyCountryData.CurrencyCode;

                if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                    model.Status = (int)Enums.CampaignStatus.InProgress;
                else
                    model.Status = (int)Enums.CampaignStatus.InsufficientFunds;
                

                if (model.ClientId == 0 || model.ClientId == null)
                {
                    if (model.newClientFormModel.Name != null && model.newClientFormModel.Name.Length > 0)
                    {
                        var clientModel = new ClientViewModel()
                        {
                            Name = model.newClientFormModel.Name,
                            Description = model.newClientFormModel.Description,
                            Email = model.newClientFormModel.Email,
                            ContactPhone = model.newClientFormModel.ContactPhone,
                            CountryId = model.newClientFormModel.CountryId.Value,
                            UserId = model.newClientFormModel.UserId
                        };
                        model.ClientId = await _userDAL.InsertNewClient(clientModel);
                    }
                }


                var newModel = new NewCampaignProfileFormModel();
                try
                {
                    newModel = await _createDAL.CreateNewCampaign(model);

                    // Is actually the CampaignProfileId from 168 Main Server
                    result.body = newModel.AdtoneServerCampaignProfileId;
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "CreateNewCampaign - AddDataToMain";
                    await _logServ.LogError();
                    
                    result.result = 0;
                    return result;
                }
                try
                {
                    // var timesettings = await CampaignProfileTimeSettingMapping(newModel.AdtoneServerCampaignProfileId.Value, model.CountryId, newModel.CampaignProfileId);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "CreateNewCampaign - Update Time Settings";
                    await _logServ.LogError();
                    
                    result.result = 0;
                    return result;
                }
                var operatorString = await _connService.GetConnectionStringsByCountryId(model.CountryId);

                try
                {
                    var z = await _matchDAL.AddCampaignMatchData(newModel, operatorString);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "CreateNewCampaign - AddUserMatch Data";
                    await _logServ.LogError();
                    
                    result.result = 0;
                    return result;
                }
                try
                {
                    await _matchProcess.PrematchProcessForCampaign(newModel.AdtoneServerCampaignProfileId.Value, operatorString);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "CreateNewCampaign - PrematchProcessForCampaign";
                    await _logServ.LogError();
                    
                    result.result = 0;
                    return result;
                }

                // Supposed to send confirmation Email to Admin user but admin has no email address


            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CreateNewCampaign";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> CreateNewCampaign_Advert(NewAdvertFormModel model)
        {
            // var testgeo = GetGeographicMode();
            int mainOperatorId = model.OperatorId;
            var AdvertNameexists = await _advertDAL.CheckAdvertNameExists(model.AdvertName, model.AdvertiserId);

            if (AdvertNameexists)
            {
                result.result = 0;
                result.error = "The Advert Name already exists";
                return result;
            }
            if (model.MediaFile == null)
            {
                result.result = 0;
                result.error = "A media file is required to proceed";
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

                string extension = Path.GetExtension(mediaFile.FileName);
                var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                string outputFormat = "wav";

                var audioFormatExtension = "." + outputFormat;
                //
                string actualDirectoryName = "Media";
                string directoryName = Path.Combine(actualDirectoryName, model.AdvertiserId.ToString());
                string newfile;
                //
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
                    newfile = await _saveFile.SaveFileToSite(directoryName, mediaFile, fileName + audioFormatExtension);

                    string archiveDirectoryName = @"Media\Archive";

                    await _saveFile.SaveFileToSite(archiveDirectoryName, mediaFile, fileName + audioFormatExtension);


                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", model.AdvertiserId.ToString(),
                                                                                fileName + audioFormatExtension);
                }

                if (Convert.ToInt32(model.OperatorId) == (int)Enums.OperatorTableId.Safaricom)
                {
                    var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                    fileName2 = secondAudioName.ToString();
                    string directory2Name = Path.Combine(directoryName, "SecondAudioFile");
                    newfile = await _saveFile.SaveFileToSite(directory2Name, mediaFile, secondAudioName + audioFormatExtension);
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

            var campaign = await _campaignDAL.GetCampaignProfileDetail(model.CampaignProfileId);
            int campaignId = 0;

            if (campaign != null)
            {
                campaignId = Convert.ToInt32(campaign.CampaignProfileId);
            }

            if (campaign.ClientId == 0 || campaign.ClientId != null)
            {
                model.ClientId = null;
            }

            model.UploadedToMediaServer = false;
            model.Status = (int)Enums.AdvertStatus.Waitingforapproval;
            model.IsAdminApproval = true;
            model.CountryId = campaign.CountryId.Value;
            model.PhoneticAlphabet = PhoneticString();
            model.NextStatus = false;
            model.AdtoneServerAdvertId = null;
            model.UpdatedBy = _httpAccessor.GetUserIdFromJWT();
            try
            {
                var newModel = await _createDAL.CreateNewCampaignAdvert(model);
                model.OperatorId = mainOperatorId;

                CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                _campaignAdvert.AdvertId = newModel.AdtoneServerAdvertId.Value;
                _campaignAdvert.CampaignProfileId = model.CampaignProfileId;
                _campaignAdvert.NextStatus = false;
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


                        var campaignProfileDetails = await _connService.GetCampaignProfileIdFromAdtoneId(model.CampaignProfileId, model.OperatorId);
                        if (campaignProfileDetails != 0)
                        {
                            await _matchDAL.UpdateMediaLocation(ConnString, adName, campaignProfileDetails);
                            await _matchProcess.PrematchProcessForCampaign(model.CampaignProfileId, ConnString);
                        }
                        // _adEmail.SendMail(model);

                        if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                        {
                            var adModel = new UserAdvertResult();

                            // Is really the Main 168 AdvertId
                            adModel.AdvertId = model.AdtoneServerAdvertId.Value;
                            adModel.CampaignProfileId = model.CampaignProfileId;
                            adModel.OperatorId = model.OperatorId;
                            adModel.AdvertName = model.AdvertName;
                            adModel.Brand = model.Brand;
                            adModel.ClientId = model.ClientId;
                            adModel.MediaFileLocation = model.MediaFileLocation;
                            adModel.PrevStatus = model.Status;
                            adModel.UpdatedBy = model.UpdatedBy;
                            adModel.Status = (int)Enums.AdvertStatus.Live;
                            var otraResult = await _advertService.ApproveORRejectAdvert(adModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CreateNewCampaign_Advert - Insert CampaignAdvert";
                await _logServ.LogError();
                
                result.result = 0;
            }
            #endregion



            return result;
        }


        public async Task<ReturnResult> InsertProfileInformation(NewAdProfileMappingFormModel model)
        {
            int y = 0;
            try
            {
                var connString = await _connService.GetConnectionStringsByCountry(model.CountryId);
                y = await _matchDAL.InsertProfilePreference(model);

                var x = await _profileService.SaveGeographicWizard(model.CampaignProfileGeographicModel, connString);
                x = await _profileService.SaveDemographicsWizard(model.CampaignProfileDemographicsmodel, connString);
                x = await _profileService.SaveAdvertsWizard(model.CampaignProfileAd, connString);
                x = await _profileService.SaveMobileWizard(model.CampaignProfileMobileFormModel, connString);
                //if(model.CountryId == 10)
                // x = await _profileService.SaveQuestionnaireWizard(model.CampaignProfileSkizaFormModel, connString);
                x = await _profileService.SaveTimeSettingsWizard(model.CampaignProfileTimeSettingModel, connString);

                result.body = x;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "InsertProfileInformation";
                await _logServ.LogError();
                
                result.result = 0;
            }

            try
            {
                var campaignProfile = await _campaignDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var ConnString = await _connService.GetConnectionStringsByCountryId(campaignProfile.CountryId.Value);
                if (ConnString != null)
                {
                    if (campaignProfile.Status == (int)Enums.CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                    {
                        await _matchProcess.PrematchProcessForCampaign(model.CampaignProfileId, ConnString);
                    }
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "InsertProfileInformation - PrematchProcessForCampaign";
                await _logServ.LogError();
                
                result.result = 0;
            }

            return result;
        }


        public async Task<ReturnResult> UpdateCampaignDetails(NewCampaignProfileFormModel model)
        {
            try
            {
                int x = await _createDAL.UpdateCampaignDetails(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateCampaign";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }

            var operatorString = await _connService.GetConnectionStringsByCountryId(model.CountryId);

            try
            {
                var z = await _matchDAL.UpdateCampaignMatchData(model, operatorString);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateCampaign - UpdateMatch Data";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
            try
            {
                await _matchProcess.PrematchProcessForCampaign(model.CampaignProfileId, operatorString);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateCampaign - PrematchProcessForCampaign";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }

            return result;

        }


        public async Task<ReturnResult> CheckIfAdvertNameExists(NewAdvertFormModel model)
        {

            var AdvertNameexists = await _advertDAL.CheckAdvertNameExists(model.AdvertName, model.AdvertiserId);

            if (AdvertNameexists)
            {
                result.result = 0;
                result.error = "The Advert Name already exists";
                return result;
            }
            else
                return result;
        }


        public async Task<ReturnResult> CheckIfCampaignNameExists(NewCampaignProfileFormModel model)
        {

            var CampaignNameexists = await _campaignDAL.CheckCampaignNameExists(model.CampaignName, model.UserId);

            if (CampaignNameexists)
            {
                result.result = 0;
                result.error = "The Campaign Name already exists";
                return result;
            }
            else
                return result;
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