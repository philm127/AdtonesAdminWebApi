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

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertService : IAdvertService
    {
        IHttpContextAccessor _httpAccessor;
        private IConnectionStringService _connService;
        private readonly IUserMatchDAL _matchDAL;
        private readonly IUserMatchQuery _matchText;
        private readonly IAdTransferService _transService;
        private readonly IGenerateTicketService _ticketService;

        // private IUserMatchInterface _matchInterface;
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdvertQuery _commandText;
        
        ReturnResult result = new ReturnResult();


        public AdvertService(IAdvertDAL advertDAL, IAdvertQuery commandText, IHttpContextAccessor httpAccessor, IConnectionStringService connService,
                                IUserMatchDAL matchDAL, IUserMatchQuery matchText, IAdTransferService transService, 
                                IGenerateTicketService ticketService)//IUserMatchInterface matchInterface
        {
            _advertDAL = advertDAL;
            _commandText = commandText;
            _httpAccessor = httpAccessor;
            _connService = connService;
            _matchDAL = matchDAL;
            _matchText = matchText;
            _transService = transService;
            _ticketService = ticketService;
            // _matchInterface = matchInterface;
        }


        /// <summary>
        /// Populate the datatable, requires getting siteAddress to fill the Script Addresses.
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadAdvertDataTable(int id=0)
        {
            try
            {
                    result.body = await _advertDAL.GetAdvertResultSet(_commandText.GetAdvertResultSet,id);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertService",
                    ProcedureName = "LoadAdvertDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadAdvertDetails(int id = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertResultSet(_commandText.GetAdvertDetail,id);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertService",
                    ProcedureName = "LoadAdvertDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadAdvertCategoryDataTable()
        {
            try
            {
                result.body = await _advertDAL.GetAdvertCategoryList(_commandText.GetAdvertCategoryDataTable);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertService",
                    ProcedureName = "LoadAdvertCategoryDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> ApproveORRejectAdvert(UserAdvertResult adModel)
        {
            var ConnString = await _connService.GetSingleConnectionString(adModel.OperatorId);

            if (adModel.Status == (int)Enums.AdvertStatus.Live && adModel.PrevStatus != (int)Enums.AdvertStatus.Pending)
            {

                var x = await _advertDAL.ChangeAdvertStatus(_commandText.UpdateAdvertStatus,adModel);
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                var y = await _advertDAL.ChangeAdvertStatusOperator(_commandText.UpdateAdvertStatus, adModel, uid);

                if (adModel.PrevStatus == 4 && adModel.CampaignProfileId > 0)
                {
                    var camstatus = Changecampaignstatus(campaignadvertId.CampaignProfileId);
                }

                if (adModel.CampaignProfileId > 0)
                {
                    if (ConnString != null)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();


                        string select_string = @"SELECT * FROM CampaignProfiles WHERE AdtoneServerCampaignProfileId=@CampaignProfileId";//.FirstOrDefault();
                        var campaigndetails = select_string;
                            if (campaigndetails != null)
                            {
                                //Add 08-08-2019
                                string adName = "";
                                if (adModel.MediaFile == null || adModel.MediaFile == "")
                                {
                                    adName = "";
                                }
                                else
                                {
                                    if (adModel.OperatorId != (int)Enums.OperatorTableId.Safaricom)
                                    {
                                    
                                    FtpDetailsModel operatorFTPDetails = await _advertDAL.GetFtpDetails(_commandText.GetFtpDetails, adModel.OperatorId);
                                        if (operatorFTPDetails != null) 
                                            adName = operatorFTPDetails.FtpRoot + "/" + adModel.MediaFile.Split('/')[3];
                                    }
                                }

                            var z = await _matchDAL.UpdateMediaLocation(_matchText.UpdateMediaLocation, ConnString, adName, adModel.CampaignProfileId);
                            await _matchDAL.PrematchProcessForCampaign(adModel.CampaignProfileId, ConnString);
                            }
                    }
                }

                result.body = "Advert " + adModel.AdvertName + " is approved successfully.";
                return result;
            }
            else if (adModel.Status == (int)Enums.AdvertStatus.Live && adModel.PrevStatus == (int)Enums.AdvertStatus.Pending) // Live
            {

                if (adModel.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                {
                    var returnValue = await _transService.CopyAdToOpeartorServer(ConnString, adModel);
                    if (returnValue != "Success")
                    {
                        string message = returnValue;
                        int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                        await _ticketService.CreateAdTicket(_httpAccessor.GetUserIdFromJWT(), "Advert Error", message, subjectId, 0);
                    }
                    else
                    {
                        var crbtResponseValue = SoapApiProcess.UploadToneOnCRBTServer(id);
                        //var crbtResponseValue = "Success";
                        if (crbtResponseValue != "Success")
                        {
                            string message = crbtResponseValue;
                            int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                            await _ticketService.CreateAdTicket(_httpAccessor.GetUserIdFromJWT(), "Advert Error", message, subjectId, 0);
                        }
                        else
                        {
                            var responseCode = SoapApiProcess.UploadSoapTone(id);
                            //var responseCode = "000000";
                            if (responseCode == "000000")
                            {
                                ApproveAd(id, command, efmvcUser.UserId, status, oldstatus);
                            }
                            else
                            {
                                var advertData = _advertRepository.GetById(id);
                                string message = "";
                                if (responseCode == "0" || responseCode.Contains("?"))
                                {
                                    message = responseCode + " - Unable to connect to the remote server";
                                }
                                else
                                {
                                    var responseCodeDetail = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == responseCode).FirstOrDefault();
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
                    var returnValue = await _transService.CopyAdToOpeartorServer(ConnString,adModel);
                    if (returnValue != "Success")
                    {
                        string message = returnValue;
                        int subjectId = (int)Enums.QuestionSubjectStatus.AdvertError;
                        await _ticketService.CreateAdTicket(_httpAccessor.GetUserIdFromJWT(), "Advert Error", message, subjectId, 0);
                    }

                    ApproveAd(id, command, efmvcUser.UserId, status, oldstatus);
                }


            }
            else if (adModel.Status == (int)Enums.AdvertStatus.Suspended) // suspended
            {
                var x = await _advertDAL.ChangeAdvertStatus(_commandText.UpdateAdvertStatus, adModel);
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                var y = await _advertDAL.ChangeAdvertStatusOperator(_commandText.UpdateAdvertStatus, adModel, uid);

                var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);
                var campaigndetails = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);

                if (ConnString != null)
                {
                    UserMatchTableProcess obj = new UserMatchTableProcess();
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                        var OperatorCampaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignadvertId.CampaignProfileId).FirstOrDefault();
                        if (OperatorCampaigndetails != null)
                        {
                            obj.UpdateMediaFileLocation(OperatorCampaigndetails.CampaignProfileId, null, SQLServerEntities);
                            PreMatchProcess.PrematchProcessForCampaign(OperatorCampaigndetails.CampaignProfileId, item);
                        }

                    }
                }

                result.body = "Advert " + adModel.AdvertName + " is suspended successfully.";
                return result;
            }
            else if (adModel.Status == (int)Enums.AdvertStatus.Archived) // Archived(Deleted)
            {
                var x = await _advertDAL.ChangeAdvertStatus(_commandText.UpdateAdvertStatus, adModel);
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                var y = await _advertDAL.ChangeAdvertStatusOperator(_commandText.UpdateAdvertStatus, adModel, uid);

                EFMVCDataContex db = new EFMVCDataContex();
                var advertData = db.Adverts.Where(s => s.AdvertId == id).FirstOrDefault();

                if (advertData.OperatorId == (int)OperatorTableId.Safaricom)
                {

                    //if (advertData.SoapToneId != null)
                    //{
                    //var responseCode = SoapApiProcess.DeleteToneSoapApi(id);
                    //271191
                    var responseCode = SoapApiProcess.DeleteSoapTone(id);
                    if (responseCode != "000000")
                    {
                        string message = "";
                        if (responseCode == "0" || responseCode.Contains("?"))
                        {
                            message = responseCode + " - Unable to connect to the remote server";
                        }
                        else
                        {
                            var responseCodeDetail = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == responseCode).FirstOrDefault();
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

                var campaignAdvertData = db.CampaignAdverts.Where(s => s.AdvertId == id).ToList();
                if (campaignAdvertData.Count() > 0)
                {
                    var campProfileId = campaignAdvertData.FirstOrDefault().CampaignProfileId;
                    var CampaignData = _campaignprofileRepository.GetById(campProfileId);
                    var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

                    if (ConnString != null)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                            var OperatorCampaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campProfileId).FirstOrDefault();
                            if (OperatorCampaigndetails != null)
                            {
                                obj.UpdateMediaFileLocation(OperatorCampaigndetails.CampaignProfileId, null, SQLServerEntities);
                                PreMatchProcess.PrematchProcessForCampaign(OperatorCampaigndetails.CampaignProfileId, item);
                            }

                        }
                    }

                }
                result.body = "Advert " + adModel.AdvertName + " is archived successfully.";
            }
            else if (adModel.Status == (int)Enums.AdvertStatus.Rejected)
            {

                var x = await _advertDAL.ChangeAdvertStatus(_commandText.UpdateAdvertStatus, adModel);
                int uid = await _connService.GetUserIdFromAdtoneId(adModel.UpdatedBy, adModel.OperatorId);
                var y = await _advertDAL.ChangeAdvertStatusOperator(_commandText.UpdateAdvertStatus, adModel, uid);


                result.body = "Advert " + adModel.AdvertName + " is rejected successfully.";
            }
            return result;
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
        //        EFMVCDataContex db = new EFMVCDataContex();
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

        //public bool Changecampaignstatus(int campaignId)
        //{
        //    bool status = false;
        //    //update campaign status from ad approval to the planned
        //    ChangeCampaignStatusCommand _campaignstatus = new ChangeCampaignStatusCommand();
        //    _campaignstatus.CampaignProfileId = campaignId;
        //    //check campaign details
        //    var campaigndetails = _campaignprofileRepository.Get(top => top.CampaignProfileId == campaignId);
        //    if (campaigndetails != null)
        //    {
        //        if (campaigndetails.StartDate == null && campaigndetails.EndDate == null)
        //        {
        //            _campaignstatus.Status = (int)CampaignStatus.Play;
        //        }
        //        else
        //        {
        //            if (campaigndetails.StartDate != null)
        //            {
        //                if (campaigndetails.StartDate.Value.Date == DateTime.Now.Date)
        //                {
        //                    _campaignstatus.Status = (int)CampaignStatus.Play;
        //                }
        //                else
        //                {
        //                    _campaignstatus.Status = (int)CampaignStatus.Planned;
        //                }
        //            }
        //            else
        //            {
        //                _campaignstatus.Status = (int)CampaignStatus.Planned;
        //            }
        //        }
        //    }
        //    ICommandResult campaignstatuscommandResult = _commandBus.Submit(_campaignstatus);
        //    if (campaignstatuscommandResult.Success)
        //    {
        //        status = true;

        //    }
        //    return status;
        //}




    }
}
