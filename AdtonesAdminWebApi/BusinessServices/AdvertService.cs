using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.UserMatchServices;
using System.Linq;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertService : IAdvertService
    {
        IHttpContextAccessor _httpAccessor;
        private IConnectionStringService _connService;
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
        ReturnResult result = new ReturnResult();
        const string PageName = "AdvertService";


        public AdvertService(IAdvertDAL advertDAL, IHttpContextAccessor httpAccessor, IConnectionStringService connService,
                                IUserMatchDAL matchDAL, IAdTransferService transService, IPrematchProcess preProcess,
                                IGenerateTicketService ticketService, ICampaignService campService, ISoapApiService soapApi,
                                ISoapDAL soapDAL, ICampaignDAL campDAL, IConfiguration configuration, ILoggingService logServ)
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
        }


        /// <summary>
        /// Populate the datatable, requires getting siteAddress to fill the Script Addresses.
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadAdvertDataTable(int id=0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertResultSet(id);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdvertDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadAdvertDataTableForSales(int id = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertForSalesResultSet(id);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdvertDataTableForSales";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Gets a single Advert as Result list when clicked through from Campaign Page.
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadAdvertDataTableById(int id = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertResultSetById(id);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdvertDataTable";
                await _logServ.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadAdvertDetails(int id = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertDetail(id);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdvertDetails";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadAdvertCategoryDataTable()
        {
            try
            {
                result.body = await _advertDAL.GetAdvertCategoryList();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdvertCategoryDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> DeleteAdvertCategory(IdCollectionViewModel model)
        {
            try
            {
                result.body = await _advertDAL.RemoveAdvertCategory(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteAdvertCategory";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model)
        {
            try
            {
                result.body = await _advertDAL.UpdateAdvertCategory(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateAdvertCategory";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetAdvertCategoryDetails(int id)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertCategoryDetails(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertCategoryDetails";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model)
        {
            try
            {
                result.body = await _advertDAL.InsertAdvertCategory(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddAdvertCategory";
                await _logServ.LogError();
                
            }
            return result;
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
                    adModel.Status = (int)Enums.AdvertStatus.Pending;
                    bool update = false;
                    update = await ApproveAd(adModel, ConnString);

                    if (update)
                        result.body = "Advert " + adModel.AdvertName + " is approved successfully.";

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


        private async Task<bool> ApproveAd(UserAdvertResult adModel, string ConnString)
        {
            try
            {
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                int adId = await _connService.GetAdvertIdFromAdtoneId(adModel.AdvertId, adModel.OperatorId);
                var campaignAdvert = new CampaignAdverts();
                try
                {
                    campaignAdvert = await _campDAL.GetCampaignAdvertDetailsById(adModel.AdvertId, 0);
                    var campaignProfile = await _campDAL.GetCampaignProfileDetail(campaignAdvert.CampaignProfileId);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = $"ApproveReject - ApproveAd - GetcampaignAdvert AdvertId={adModel.AdvertId}";
                    await _logServ.LogError();
                    
                    return false;
                }

                var updstatus = UpdateStatus(adModel,uid,adId);

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

                            var z = await _matchDAL.UpdateMediaLocation(ConnString, adName, campaigndetailsid);
                            await _matchDAL.PrematchProcessForCampaign(campaignAdvert.CampaignProfileId, ConnString);
                        }
                    }
                }

                var t = RemoveRejections(adModel, adId, ConnString);

                return true;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ApproveRejectApproveAd";
                await _logServ.LogError();
                
                return false;
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
            var x = await _advertDAL.ChangeAdvertStatus(adModel);
            var y = await _advertDAL.ChangeAdvertStatusOperator(adModel, adtoneUser, adtoneAd);
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
                    var f = await _matchDAL.UpdateMediaLocation(ConnString, null, campaigndetailsid);
                    await _preProcess.PrematchProcessForCampaign(campaignAdvert.CampaignProfileId, ConnString);
                }
            }
            return true;
        }


        

        //public async Task<ReturnResult> RejectAdvert(UserAdvertResult model)
        //{
        //    var x = _advertDAL.ChangeAdvertStatus(_commandText.UpdateAdvertStatus, model);
        //    model.Status = 5; // Rejection Status
        //    ICommandResult result = _commandBus.Submit(command);
        //    if (result.Success)
        //    {
        //        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

        //        CreateOrUpdateAdvertRejectionCommand command2 = new CreateOrUpdateAdvertRejectionCommand();
        //        command2.AdvertId = advertId;
        //        command2.UserId = efmvcUser.UserId;
        //        command2.RejectionReason = rejectionReason;
        //        command2.CreatedDate = DateTime.Now;
        //        ICommandResult result2 = _commandBus.Submit(command2);

        //        var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == advertId);
        //        var campaigndetails = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);


        //        EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
        //        var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
        //        UserMatchTableProcess obj = new UserMatchTableProcess();
        //        obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
        //        PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

        //        var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
        //        if (ConnString != null && ConnString.Count() > 0)
        //        {
        //            foreach (var item in ConnString)
        //            {
        //                SQLServerEntities = new EFMVCDataContex(item);
        //                obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
        //                PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, item);
        //            }
        //        }


        //        TempData["status"] = "Advert is rejected successfully.";
        //        return Json("success");
        //    }
        //    return Json("error");
        //}

        //public ActionResult ApproveORRejectAdvert(int id, int status, int oldstatus)
        //{

        //    ChangeAdvertStatusCommand command = new ChangeAdvertStatusCommand();
        //    //if (result.Success)
        //    //{
        //    if (status == 1) // Live
        //    {
        //        AdTransfer.CopyAdToOpeartorServer(id);
        //        //271191                
        //        var returnCode = SoapApiProcess.UploadSoapTone(id);
        //        //var returnCode = SoapApiProcess.UpdateToneSoapApi(id);
        //        if (returnCode == "000000")
        //        {
        //            command.AdvertId = id;
        //            command.Status = status;
        //            ICommandResult result = _commandBus.Submit(command);
        //            var mediaUrl = _advertRepository.GetById(id).MediaFileLocation;
        //            var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);

        //            if (oldstatus == 4 && campaignadvertId != null)
        //            {
        //                var camstatus = Changecampaignstatus(campaignadvertId.CampaignProfileId);
        //                //if (camstatus)
        //                //{
        //                //    TempData["status"] = "Advert is approved successfully.";
        //                //}                         

        //            }

        //            if (campaignadvertId != null)
        //            {
        //                var CampaignData = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);
        //                var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

        //                //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
        //                //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
        //                //UserMatchTableProcess obj = new UserMatchTableProcess();
        //                //obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, mediaUrl, SQLServerEntities);
        //                //PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

        //                var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
        //                if (ConnString != null && ConnString.Count() > 0)
        //                {
        //                    UserMatchTableProcess obj = new UserMatchTableProcess();
        //                    foreach (var item in ConnString)
        //                    {
        //                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
        //                        var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignadvertId.CampaignProfileId).FirstOrDefault();
        //                        if (campaigndetails != null)
        //                        {
        //                            obj.UpdateMediaFileLocation(campaigndetails.CampaignProfileId, mediaUrl, SQLServerEntities);
        //                            PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
        //                        }

        //                    }
        //                }
        //            }

        //            var rejectionList = _advertRejectionRepository.GetMany(s => s.AdvertId == id).ToList();
        //            foreach (var item in rejectionList)
        //            {
        //                var delteAdReasoncommand = new DeleteAdvertRejectionCommand { Id = item.AdvertRejectionId };
        //                ICommandResult commandResult = _commandBus.Submit(delteAdReasoncommand);
        //            }

        //            TempData["status"] = "Advert is approved successfully.";
        //        }
        //        else
        //        {
        //            //Generate Ticket
        //        }


        //    }
        //    else if (status == 2) // suspended
        //    {
        //        command.AdvertId = id;
        //        command.Status = status;
        //        ICommandResult result = _commandBus.Submit(command);

        //        var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);
        //        var campaigndetails = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);


        //        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
        //        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
        //        //UserMatchTableProcess obj = new UserMatchTableProcess();
        //        //obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
        //        //PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

        //        var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
        //        if (ConnString != null && ConnString.Count() > 0)
        //        {
        //            UserMatchTableProcess obj = new UserMatchTableProcess();
        //            foreach (var item in ConnString)
        //            {
        //                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
        //                var OperatorCampaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignadvertId.CampaignProfileId).FirstOrDefault();
        //                if (OperatorCampaigndetails != null)
        //                {
        //                    obj.UpdateMediaFileLocation(OperatorCampaigndetails.CampaignProfileId, null, SQLServerEntities);
        //                    PreMatchProcess.PrematchProcessForCampaign(OperatorCampaigndetails.CampaignProfileId, item);
        //                }

        //            }
        //        }

        //        TempData["status"] = "Advert is suspended successfully.";
        //    }
        //    else if (status == 3) // Archived(Deleted)
        //    {
        //        command.AdvertId = id;
        //        command.Status = status;
        //        ICommandResult result = _commandBus.Submit(command);
        //        // EFMVCDataContex db = new EFMVCDataContex();
        //        var toneId = db.Adverts.Where(s => s.AdvertId == id).FirstOrDefault().SoapToneId;

        //        //271191
        //        //var returnCode = SoapApiProcess.DeleteSoapTone(toneId);
        //        if (toneId != null)
        //        {
        //            var returnCode = SoapApiProcess.DeleteToneSoapApi(id);
        //        }
        //        var campaignAdvertData = db.CampaignAdverts.Where(s => s.AdvertId == id).ToList();
        //        if (campaignAdvertData.Count() > 0)
        //        {
        //            var campProfileId = campaignAdvertData.FirstOrDefault().CampaignProfileId;
        //            var CampaignData = _campaignprofileRepository.GetById(campProfileId);
        //            var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

        //            //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
        //            //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
        //            //UserMatchTableProcess obj = new UserMatchTableProcess();
        //            //obj.UpdateMediaFileLocation(campProfileId, null, SQLServerEntities);
        //            //PreMatchProcess.PrematchProcessForCampaign(campProfileId, conn);

        //            var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
        //            if (ConnString != null && ConnString.Count() > 0)
        //            {
        //                UserMatchTableProcess obj = new UserMatchTableProcess();
        //                foreach (var item in ConnString)
        //                {
        //                    EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
        //                    var OperatorCampaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campProfileId).FirstOrDefault();
        //                    if (OperatorCampaigndetails != null)
        //                    {
        //                        obj.UpdateMediaFileLocation(OperatorCampaigndetails.CampaignProfileId, null, SQLServerEntities);
        //                        PreMatchProcess.PrematchProcessForCampaign(OperatorCampaigndetails.CampaignProfileId, item);
        //                    }

        //                }
        //            }

        //        }

        //        TempData["status"] = "Advert is archived successfully.";
        //    }
        //    else if (status == 5)
        //    {
        //        command.AdvertId = id;
        //        command.Status = status;
        //        ICommandResult result = _commandBus.Submit(command);
        //        TempData["status"] = "Advert is rejected successfully.";
        //    }
        //    return Json("success");
        //    //}
        //    //return Json("error");
        //}

        

    }
}
