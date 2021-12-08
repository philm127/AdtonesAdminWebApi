using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.UserMatchServices;
using System.Linq;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using System.IO;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using AdtonesAdminWebApi.Services.Mailer;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertService : IAdvertService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConnectionStringService _connService;
        private readonly IUserMatchDAL _matchDAL;
        private readonly IAdTransferService _transService;
        private readonly IGenerateTicketService _ticketService;
        private readonly ICampaignService _campService;
        private readonly ISoapApiService _soapApi;
        private readonly ISoapDAL _soapDAL;
        private readonly IAdvertDAL _advertDAL;
        private readonly ICampaignDAL _campDAL;
        private readonly IConfiguration _configuration;
        private readonly IPrematchProcess _preProcess;
        private readonly ILoggingService _logServ;
        private readonly ISaveGetFiles _saveFile;
        private readonly IConvertSaveMediaFile _convFile;
        private readonly ICampaignAdvertDAL _campAdDal;
        private readonly IAdvertEmail _adEmail;
        private readonly IPrematchProcess _matchProcess;
        private readonly ICampaignMatchDAL _campMatchDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "AdvertService";


        public AdvertService(IAdvertDAL advertDAL, IHttpContextAccessor httpAccessor, IConnectionStringService connService,
                                IUserMatchDAL matchDAL, IAdTransferService transService, IPrematchProcess preProcess,
                                IGenerateTicketService ticketService, ICampaignService campService, ISoapApiService soapApi,
                                ISoapDAL soapDAL, ICampaignDAL campDAL, IConfiguration configuration, ILoggingService logServ, 
                                ISaveGetFiles saveFile, IConvertSaveMediaFile convFile, ICampaignAdvertDAL campAdDal, ICampaignMatchDAL campMatchDAL,
                                IAdvertEmail adEmail, IPrematchProcess matchProcess)
        {
            _advertDAL = advertDAL;
            _httpAccessor = httpAccessor;
            _connService = connService;
            _matchDAL = matchDAL;
            _transService = transService;
            _ticketService = ticketService;
            _campService = campService;
            _soapApi = soapApi;
            _soapDAL = soapDAL;
            _campDAL = campDAL;
            _configuration = configuration;
            _preProcess = preProcess;
            _logServ = logServ;
            _saveFile = saveFile;
            _convFile = convFile;
            _campAdDal = campAdDal;
            _adEmail = adEmail;
            _matchProcess = matchProcess;
            _campMatchDAL = campMatchDAL;
        }


        public async Task<ReturnResult> GetAdvertDetails(int id = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertDetail(id);
                return result;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDetails";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> CreateNewCampaign_Advert(NewAdvertFormModel model)
        {
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
                model.MediaFileLocation = await CreateMediaFile(mediaFile, model.AdvertiserId, model.OperatorId);
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

            var campaign = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);
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
                var newModel = await _advertDAL.CreateNewAdvert(model);
                model.OperatorId = mainOperatorId;

                CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                _campaignAdvert.AdvertId = newModel.AdtoneServerAdvertId.Value;
                _campaignAdvert.CampaignProfileId = model.CampaignProfileId;
                _campaignAdvert.NextStatus = false;
                _campaignAdvert.AdtoneServerCampaignAdvertId = null;
                var newAdCamp = await _campAdDal.CreateNewCampaignAdvert(_campaignAdvert, model.OperatorId, newModel.AdvertId);

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
                            await _campMatchDAL.UpdateMediaLocation(ConnString, adName, campaignProfileDetails);
                            await _matchProcess.PrematchProcessForCampaign(model.CampaignProfileId, ConnString);
                        }
                        _adEmail.SendMail(model);

                        if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                        {
                            var adModel = new UserAdvertResult();

                            // Is really the Main 168 AdvertId
                            adModel.AdvertId = newModel.AdtoneServerAdvertId.Value;
                            adModel.CampaignProfileId = model.CampaignProfileId;
                            adModel.OperatorId = model.OperatorId;
                            adModel.AdvertName = model.AdvertName;
                            adModel.Brand = model.Brand;
                            adModel.ClientId = model.ClientId;
                            adModel.MediaFileLocation = model.MediaFileLocation;
                            adModel.PrevStatus = model.Status;
                            adModel.UpdatedBy = model.UpdatedBy;
                            adModel.Status = (int)Enums.AdvertStatus.Live;
                            var otraResult = await ApproveORRejectAdvert(adModel);
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
                result.error = ex.Message.ToString();
                result.result = 0;
                return result;
            }
            #endregion



            return result;
        }


        public async Task<ReturnResult> UpdateAdvert(NewAdvertFormModel model)
        {
            int mainOperatorId = model.OperatorId;
            IFormFile mediaFile = model.MediaFile;
            IFormFile scriptFile = model.ScriptFile;
            var existingAdvert = new NewAdvertFormModel();
            existingAdvert = await _advertDAL.GetAdvertForUpdateModel(model.AdvertId);

            existingAdvert.CampaignProfileId = await _campAdDal.GetCampaignIdByAdvertId(model.AdvertId);

            var adverIdFromOp = await _connService.GetAdvertIdFromAdtoneId(model.AdvertId, existingAdvert.OperatorId);

            #region Media
            if (mediaFile != null && mediaFile.Length > 0)
            {
                model.FileUpdate = true;
                existingAdvert.FileUpdate = true;
                existingAdvert.MediaFileLocation = await CreateMediaFile(mediaFile, model.AdvertiserId, model.OperatorId);
            }

            #endregion


            #region Add Records

            var campaign = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);
            int campaignId = 0;

            if (campaign != null)
            {
                campaignId = Convert.ToInt32(campaign.CampaignProfileId);
            }

            if (campaign.ClientId == 0 || campaign.ClientId != null)
            {
                 existingAdvert.ClientId = null;
            }
            if (model.FileUpdate) 
            {
                existingAdvert.UploadedToMediaServer = false;
                existingAdvert.Status = (int)Enums.AdvertStatus.Waitingforapproval;
            }
            existingAdvert.UpdatedBy = _httpAccessor.GetUserIdFromJWT();
            existingAdvert.AdvertName = model.AdvertName;
            existingAdvert.AdvertCategoryId = model.AdvertCategoryId;
            existingAdvert.Brand = model.Brand;
            try
            {
                if(adverIdFromOp == 0)
                {
                    existingAdvert.AdtoneServerAdvertId = model.AdvertId;
                    var newModel = await _advertDAL.CreateNewOperatorAdvert(existingAdvert);
                }
                var result = await _advertDAL.UpdateAdvert(existingAdvert);

                CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                _campaignAdvert.AdvertId = model.AdvertId;
                _campaignAdvert.CampaignProfileId = model.CampaignProfileId;
                _campaignAdvert.NextStatus = false;
                _campaignAdvert.AdtoneServerCampaignAdvertId = null;
                var newAdCamp = await _campAdDal.CreateOnUpdateCampaignAdvert(_campaignAdvert, model.OperatorId);

                if (campaign != null && model.FileUpdate)
                {

                    string adName = "";
                    if (existingAdvert.MediaFileLocation == null || existingAdvert.MediaFileLocation == "")
                    {
                        adName = "";
                    }
                    else
                    {

                        var operatorFTPDetails = await _advertDAL.GetFtpDetails(existingAdvert.OperatorId);
                        adName = operatorFTPDetails.FtpRoot + "/" + existingAdvert.MediaFileLocation.Split('/')[3];
                    }
                    var ConnString = await _connService.GetConnectionStringByOperator(existingAdvert.OperatorId);


                    var campaignProfileDetails = await _connService.GetCampaignProfileIdFromAdtoneId(existingAdvert.CampaignProfileId, existingAdvert.OperatorId);
                    if (campaignProfileDetails != 0)
                    {
                        await _campMatchDAL.UpdateMediaLocation(ConnString, adName, campaignProfileDetails);
                        await _matchProcess.PrematchProcessForCampaign(existingAdvert.CampaignProfileId, ConnString);
                    }
                    _adEmail.SendMail(existingAdvert);

                    if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                    {
                        var adModel = new UserAdvertResult();

                        // Is really the Main 168 AdvertId
                        adModel.AdvertId = existingAdvert.AdtoneServerAdvertId.Value;
                        adModel.CampaignProfileId = existingAdvert.CampaignProfileId;
                        adModel.OperatorId = existingAdvert.OperatorId;
                        adModel.AdvertName = existingAdvert.AdvertName;
                        adModel.Brand = existingAdvert.Brand;
                        adModel.ClientId = existingAdvert.ClientId;
                        adModel.MediaFileLocation = existingAdvert.MediaFileLocation;
                        adModel.PrevStatus = existingAdvert.Status;
                        adModel.UpdatedBy = existingAdvert.UpdatedBy;
                        adModel.Status = (int)Enums.AdvertStatus.Live;
                        var otraResult = await ApproveORRejectAdvert(adModel);
                    }
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateAdvert";
                await _logServ.LogError();
                result.error = ex.Message.ToString();
                result.result = 0;
                return result;
            }
            #endregion

            return result;

        }

        private async Task<string> CreateMediaFile(IFormFile mediaFile, int advertiserId, int operatorId)
        {
            string MediaFileLocation = string.Empty;
            var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

            string fileName = firstAudioName;

            string fileName2 = null;

            string extension = Path.GetExtension(mediaFile.FileName);
            var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
            string outputFormat = "wav";

            var audioFormatExtension = "." + outputFormat;
            //
            string actualDirectoryName = "Media";
            string directoryName = Path.Combine(actualDirectoryName, advertiserId.ToString());
            string newfile;
            //
            if (extension != audioFormatExtension)
            {
                string tempDirectoryName = @"Media\Temp\";
                string tempFile = await _saveFile.SaveFileToSite(tempDirectoryName, mediaFile);

                newfile = _convFile.ConvertAndSaveMediaFile(tempDirectoryName + tempFile, extension, outputFormat, onlyFileName, directoryName);

                MediaFileLocation = string.Format("/Media/{0}/{1}", advertiserId.ToString(),
                                                                        fileName + "." + outputFormat);
            }
            else
            {
                newfile = await _saveFile.SaveFileToSite(directoryName, mediaFile, fileName + audioFormatExtension);

                string archiveDirectoryName = @"Media\Archive";

                await _saveFile.SaveFileToSite(archiveDirectoryName, mediaFile, fileName + audioFormatExtension);


                MediaFileLocation = string.Format("/Media/{0}/{1}", advertiserId.ToString(),
                                                                            fileName + audioFormatExtension);
            }

            if (Convert.ToInt32(operatorId) == (int)Enums.OperatorTableId.Safaricom)
            {
                var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                fileName2 = secondAudioName.ToString();
                string directory2Name = Path.Combine(directoryName, "SecondAudioFile");
                newfile = await _saveFile.SaveFileToSite(directory2Name, mediaFile, secondAudioName + audioFormatExtension);
            }

            return MediaFileLocation;
        }

        public async Task<ReturnResult> ApproveORRejectAdvert(UserAdvertResult model)
        {
            try
            {
                var adModel = await _advertDAL.GetAdvertDetail(model.AdvertId);
                adModel.Status = model.Status;
                adModel.PrevStatus = model.PrevStatus;
                adModel.UpdatedBy = model.UpdatedBy;
                adModel.RejectionReason = model.RejectionReason;
                var ConnString = await _connService.GetConnectionStringByOperator(adModel.OperatorId);

                if (adModel.Status == (int)Enums.AdvertStatus.Live && adModel.PrevStatus != (int)Enums.AdvertStatus.Pending)
                {
                    _logServ.ErrorMessage = "Entered Approve OR reject 1= pending";
                    _logServ.StackTrace = "";
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = $"ApproveReject - ApproveAd AdvertId={adModel.AdvertId}";
                    await _logServ.LogInfo();

                    adModel.Status = (int)Enums.AdvertStatus.Pending;
                    string update = string.Empty;
                    update = await ApproveAd(adModel, ConnString);

                    if (update == "true")
                        result.body = $"Advert {adModel.AdvertName} is approved successfully.";
                    else
                    {
                        result.body = $"There was an issue approving the advert {adModel.AdvertName}";
                        result.result = 0;

                        _logServ.ErrorMessage = $"Approve != pending result from ApproveAd function -  {update}";
                        _logServ.StackTrace = "";
                        _logServ.PageName = PageName;
                        _logServ.ProcedureName = "ApproveRejectAdvert Approve != pending";
                        await _logServ.LogError();
                    }

                    return result;
                }
                else if (adModel.Status == (int)Enums.AdvertStatus.Live && adModel.PrevStatus == (int)Enums.AdvertStatus.Pending) // Live
                {
                    bool update = false;
                    update = await LiveFromPending(adModel, ConnString);

                    if (update)
                        result.body = "Advert " + adModel.AdvertName + " is approved successfully.";

                    return result;

                }

                else if (adModel.Status == (int)Enums.AdvertStatus.Suspended) // suspended
                {
                    bool update = false;
                    update = await Suspended(adModel, ConnString);

                    if (update)
                        result.body = "Advert " + adModel.AdvertName + " is suspended successfully.";

                    return result;
                }
                else if (adModel.Status == (int)Enums.AdvertStatus.Archived) // Archived(Deleted)
                {
                    bool update = false;
                    update = await Archived(adModel, ConnString);

                    if (update)
                        result.body = "Advert " + adModel.AdvertName + " is archived successfully.";

                    return result;
                }
                else if (adModel.Status == (int)Enums.AdvertStatus.Rejected)
                {
                    bool update = false;
                    update = await Rejected(adModel, ConnString);

                    if (update)
                        result.body = "Advert " + adModel.AdvertName + " was rejected successfully.";
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ApproveRejectAdvert";
                await _logServ.LogError();
                
                result.result = 0;

            }
            return result;
        }


        private async Task<string> ApproveAd(UserAdvertResult adModel, string ConnString)
        {
            try
            {
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                int adId = await _connService.GetAdvertIdFromAdtoneId(adModel.AdvertId, adModel.OperatorId);
                var campaignAdvert = new CampaignAdverts();
                try
                {
                    campaignAdvert = await _campDAL.GetCampaignAdvertDetailsById(adModel.AdvertId, 0);
                    //
                    _logServ.ErrorMessage = $"Entered ApproveAd Got campaignAdvert which is {campaignAdvert}";
                    _logServ.StackTrace = "";
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = $"ApproveAd AdvertId={adModel.AdvertId}";
                    await _logServ.LogInfo();
                    //
                    // campaignAdvert = await _campDAL.GetCampaignAdvertDetailsById(25000, 0);
                    if (campaignAdvert == null)
                        return "The CampaignAdverts do not contain an entry";
                    // var campaignProfile = await _campDAL.GetCampaignProfileDetail(campaignAdvert.CampaignProfileId);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = $"ApproveReject - ApproveAd - GetcampaignAdvert AdvertId={adModel.AdvertId}";
                    await _logServ.LogError();
                    
                    return ex.Message.ToString();
                }

                var updstatus = UpdateStatus(adModel,uid,adId);
                //
                _logServ.ErrorMessage = $"Entered ApproveAd Ran UpdateStatus got {updstatus}";
                _logServ.StackTrace = "";
                _logServ.PageName = PageName;
                _logServ.ProcedureName = $"ApproveAd AdvertId={adModel.AdvertId}";
                await _logServ.LogInfo();
                //

                var operatordId = adModel.OperatorId;

                if (adModel.PrevStatus == 4 && campaignAdvert != null)
                {
                    // Goes off to Campaign Service to change campaign and returns a bool
                    var camstatus = _campService.ChangeCampaignStatus(campaignAdvert.CampaignProfileId);
                }


                if (campaignAdvert != null)
                {
                    if (ConnString != null)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();


                        var campaigndetailsid = await _connService.GetCampaignProfileIdFromAdtoneId(campaignAdvert.CampaignProfileId, adModel.OperatorId);
                        if (campaigndetailsid != 0)
                        {
                            string adName = "";
                            if (adModel.MediaFileLocation == null || adModel.MediaFileLocation == "")
                            {
                                adName = "";
                            }
                            else
                            {
                                if (adModel.OperatorId != (int)Enums.OperatorTableId.Safaricom)
                                {
                                    FtpDetailsModel operatorFTPDetails = await _advertDAL.GetFtpDetails(adModel.OperatorId);
                                    if (operatorFTPDetails != null)
                                        adName = operatorFTPDetails.FtpRoot + "/" + adModel.MediaFileLocation.Split('/')[3];
                                }
                            }

                            var z = await _campMatchDAL.UpdateMediaLocation(ConnString, adName, campaigndetailsid);
                            await _matchDAL.PrematchProcessForCampaign(campaignAdvert.CampaignProfileId, ConnString);
                        }
                    }
                }

                var t = RemoveRejections(adModel, adId, ConnString);

                return "true";
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ApproveRejectApproveAd";
                await _logServ.LogError();
                
                return ex.Message.ToString();
            }
        }


        private async Task<bool> LiveFromPending(UserAdvertResult adModel, string ConnString)
        {
            try
            {
                if (adModel.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                {
                    var returnValue = await _transService.CopyAdToOperatorServer(ConnString, adModel);
                    if (returnValue != "Success")
                    {
                        string message = returnValue;
                        int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                        await _ticketService.CreateAdTicket(adModel.UpdatedBy, "Advert Error", message, subjectId, 0);
                    }
                    else
                    {
                        string crbtResponseValue = string.Empty;
                        crbtResponseValue = _soapApi.UploadToneOnCRBTServer(adModel.AdvertId);

                        if (crbtResponseValue != "Success")
                        {
                            string message = crbtResponseValue;
                            int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                            await _ticketService.CreateAdTicket(adModel.UpdatedBy, "Advert Error", message, subjectId, 0);
                        }
                        else
                        {
                            string responseCode = string.Empty;
                            responseCode = _soapApi.UploadSoapTone(adModel.AdvertId);
                            //var responseCode = "000000";
                            if (responseCode == "000000")
                            {
                                var boolRet = await ApproveAd(adModel, ConnString);
                            }
                            else
                            {
                                string message = "";
                                if (responseCode == "0" || responseCode.Contains("?"))
                                {
                                    message = responseCode + " - Unable to connect to the remote server";
                                }
                                else
                                {
                                    var responseCodeDetail = await _soapDAL.GetSoapApiResponse(responseCode);
                                    if (responseCodeDetail != null)
                                    {
                                        message = responseCode + " - " + responseCodeDetail.Description;
                                    }
                                    else
                                    {
                                        message = responseCode + " - please add this response code";
                                    }
                                }

                                int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                                await _ticketService.CreateAdTicket(adModel.UserId, "Advert Error", message, subjectId, 0);
                            }
                        }

                    }
                }
                else if (adModel.OperatorId == (int)Enums.OperatorTableId.Expresso)
                {
                    var returnValue = await _transService.CopyAdToOperatorServer(ConnString, adModel);
                    if (returnValue != "Success")
                    {
                        string message = returnValue;
                        int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                        await _ticketService.CreateAdTicket(_httpAccessor.GetUserIdFromJWT(), "Advert Error", message, subjectId, 0);
                    }

                    var boolRet = await ApproveAd(adModel, ConnString);
                }

                int adId = await _connService.GetAdvertIdFromAdtoneId(adModel.AdvertId, adModel.OperatorId);

                var t = RemoveRejections(adModel, adId, ConnString);

                return true;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ApproveRejectLiveFromPending";
                await _logServ.LogError();
                
                return false;
            }
        }


        private async Task<bool> Suspended(UserAdvertResult adModel, string ConnString)
        {
            try
            {
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                int adId = await _connService.GetAdvertIdFromAdtoneId(adModel.AdvertId, adModel.OperatorId);
                var updStatus = await UpdateStatus(adModel, uid, adId);

                var updLoc = await UpdateMediaFileLocation(adModel, ConnString);

                return true;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ApproveRejectSuspended";
                await _logServ.LogError();
                
                return false;
            }
        }


        private async Task<bool> Archived(UserAdvertResult adModel, string ConnString)
        {
            try
            {
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                int adId = await _connService.GetAdvertIdFromAdtoneId(adModel.AdvertId, adModel.OperatorId);

                var updstatus = UpdateStatus(adModel, uid, adId);

                if (adModel.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                {
                    string responseCode = string.Empty;
                    responseCode = _soapApi.DeleteSoapTone(adModel.AdvertId);

                    if (responseCode != "000000")
                    {
                        string message = "";
                        if (responseCode == "0" || responseCode.Contains("?"))
                        {
                            message = responseCode + " - Unable to connect to the remote server";
                        }
                        else
                        {
                            var responseCodeDetail = await _soapDAL.GetSoapApiResponse(responseCode);
                            if (responseCodeDetail != null)
                            {
                                message = responseCode + " - " + responseCodeDetail.Description;
                            }
                            else
                            {
                                message = responseCode + " - please add this response code";
                            }
                        }

                        int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                        await _ticketService.CreateAdTicket(adModel.UserId, "Advert Error", message, subjectId, 0);

                    }
                }

                var updLoc = UpdateMediaFileLocation(adModel, ConnString);
                return true;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ApproveRejectArchived";
                await _logServ.LogError();
                
                return false;
            }
        }


        private async Task<bool> Rejected(UserAdvertResult adModel, string ConnString)
        {
            try
            {
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                int adId = await _connService.GetAdvertIdFromAdtoneId(adModel.AdvertId, adModel.OperatorId);

                var updstatus = UpdateStatus(adModel, uid,adId);
                

                var rejId = await _advertDAL.RejectAdvertReason(adModel);
                var z = await _advertDAL.RejectAdvertReasonOperator(adModel, ConnString, uid, rejId,adId);

                var updLoc = UpdateMediaFileLocation(adModel, ConnString);

                return true;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ApproveRejectRejected";
                await _logServ.LogError();
                
                return false;
            }
        }


        private async Task<bool> UpdateStatus(UserAdvertResult adModel, int adtoneUser, int adtoneAd)
        {
            try
            {
                var x = await _advertDAL.ChangeAdvertStatus(adModel);
                var y = await _advertDAL.ChangeAdvertStatusOperator(adModel, adtoneUser, adtoneAd);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateStatus";
                await _logServ.LogError();

                return false;
            }
            return true;
        }


        private async Task<bool> RemoveRejections(UserAdvertResult adModel, int adtoneAd, string ConnString)
        {
            var x = await _advertDAL.DeleteAdvertRejection(adModel);
            var y = await _advertDAL.DeleteRejectAdvertReasonOperator(ConnString, adtoneAd);
            return true;
        }


        private async Task<bool> UpdateMediaFileLocation(UserAdvertResult adModel, string ConnString)
        {
            //ApproveRejectRejected
            var campaignAdvert = await _campDAL.GetCampaignAdvertDetailsById(adModel.AdvertId,0);
            var campaignProfile = await _campDAL.GetCampaignProfileDetail(campaignAdvert.CampaignProfileId);

            if (ConnString != null)
            {
                var campaigndetailsid = await _connService.GetCampaignProfileIdFromAdtoneId(campaignAdvert.CampaignProfileId, adModel.OperatorId);
                if (campaigndetailsid != 0)
                {
                    var f = await _campMatchDAL.UpdateMediaLocation(ConnString, null, campaigndetailsid);
                    await _preProcess.PrematchProcessForCampaign(campaignAdvert.CampaignProfileId, ConnString);
                }
            }
            return true;
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
