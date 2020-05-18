using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.DAL.Interfaces;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertService : IAdvertService
    {
        IHttpContextAccessor _httpAccessor;
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdvertQuery _commandText;
        
        ReturnResult result = new ReturnResult();


        public AdvertService(IAdvertDAL advertDAL, IAdvertQuery commandText, IHttpContextAccessor httpAccessor)
        {
            _advertDAL = advertDAL;
            _commandText = commandText;
            _httpAccessor = httpAccessor;
        }


        /// <summary>
        /// Populate the datatable, requires getting siteAddress to fill the Script Addresses.
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadAdvertDataTable(int id=0)
        {
            try
            {
                if(id == 0)
                    result.body = await _advertDAL.GetAdvertResultSet(_commandText.GetAdvertResultSet);
                else
                    result.body = await _advertDAL.GetAdvertResultSet(_commandText.GetAdvertDetail);

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


        public async Task<ReturnResult> RejectAdvert(UserAdvertResult model)
        {
            var x = _advertDAL.ChangeAdvertStatus(_commandText.UpdateAdvertStatus, model);
            model.Status = 5; // Rejection Status
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateAdvertRejectionCommand command2 = new CreateOrUpdateAdvertRejectionCommand();
                command2.AdvertId = advertId;
                command2.UserId = efmvcUser.UserId;
                command2.RejectionReason = rejectionReason;
                command2.CreatedDate = DateTime.Now;
                ICommandResult result2 = _commandBus.Submit(command2);

                var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == advertId);
                var campaigndetails = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);


                EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                UserMatchTableProcess obj = new UserMatchTableProcess();
                obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
                PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        SQLServerEntities = new EFMVCDataContex(item);
                        obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
                        PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, item);
                    }
                }


                TempData["status"] = "Advert is rejected successfully.";
                return Json("success");
            }
            return Json("error");
        }

        public ActionResult ApproveORRejectAdvert(int id, int status, int oldstatus)
        {

            ChangeAdvertStatusCommand command = new ChangeAdvertStatusCommand();
            //if (result.Success)
            //{
            if (status == 1) // Live
            {
                AdTransfer.CopyAdToOpeartorServer(id);
                //271191                
                var returnCode = SoapApiProcess.UploadSoapTone(id);
                //var returnCode = SoapApiProcess.UpdateToneSoapApi(id);
                if (returnCode == "000000")
                {
                    command.AdvertId = id;
                    command.Status = status;
                    ICommandResult result = _commandBus.Submit(command);
                    var mediaUrl = _advertRepository.GetById(id).MediaFileLocation;
                    var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);

                    if (oldstatus == 4 && campaignadvertId != null)
                    {
                        var camstatus = Changecampaignstatus(campaignadvertId.CampaignProfileId);
                        //if (camstatus)
                        //{
                        //    TempData["status"] = "Advert is approved successfully.";
                        //}                         

                    }

                    if (campaignadvertId != null)
                    {
                        var CampaignData = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);
                        var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        //obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, mediaUrl, SQLServerEntities);
                        //PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignadvertId.CampaignProfileId).FirstOrDefault();
                                if (campaigndetails != null)
                                {
                                    obj.UpdateMediaFileLocation(campaigndetails.CampaignProfileId, mediaUrl, SQLServerEntities);
                                    PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                }

                            }
                        }
                    }

                    var rejectionList = _advertRejectionRepository.GetMany(s => s.AdvertId == id).ToList();
                    foreach (var item in rejectionList)
                    {
                        var delteAdReasoncommand = new DeleteAdvertRejectionCommand { Id = item.AdvertRejectionId };
                        ICommandResult commandResult = _commandBus.Submit(delteAdReasoncommand);
                    }

                    TempData["status"] = "Advert is approved successfully.";
                }
                else
                {
                    //Generate Ticket
                }


            }
            else if (status == 2) // suspended
            {
                command.AdvertId = id;
                command.Status = status;
                ICommandResult result = _commandBus.Submit(command);

                var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);
                var campaigndetails = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);


                //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                //UserMatchTableProcess obj = new UserMatchTableProcess();
                //obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
                //PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
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

                TempData["status"] = "Advert is suspended successfully.";
            }
            else if (status == 3) // Archived(Deleted)
            {
                command.AdvertId = id;
                command.Status = status;
                ICommandResult result = _commandBus.Submit(command);
                EFMVCDataContex db = new EFMVCDataContex();
                var toneId = db.Adverts.Where(s => s.AdvertId == id).FirstOrDefault().SoapToneId;

                //271191
                //var returnCode = SoapApiProcess.DeleteSoapTone(toneId);
                if (toneId != null)
                {
                    var returnCode = SoapApiProcess.DeleteToneSoapApi(id);
                }
                var campaignAdvertData = db.CampaignAdverts.Where(s => s.AdvertId == id).ToList();
                if (campaignAdvertData.Count() > 0)
                {
                    var campProfileId = campaignAdvertData.FirstOrDefault().CampaignProfileId;
                    var CampaignData = _campaignprofileRepository.GetById(campProfileId);
                    var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

                    //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                    //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                    //UserMatchTableProcess obj = new UserMatchTableProcess();
                    //obj.UpdateMediaFileLocation(campProfileId, null, SQLServerEntities);
                    //PreMatchProcess.PrematchProcessForCampaign(campProfileId, conn);

                    var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                    if (ConnString != null && ConnString.Count() > 0)
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

                TempData["status"] = "Advert is archived successfully.";
            }
            else if (status == 5)
            {
                command.AdvertId = id;
                command.Status = status;
                ICommandResult result = _commandBus.Submit(command);
                TempData["status"] = "Advert is rejected successfully.";
            }
            return Json("success");
            //}
            //return Json("error");
        }

        public bool Changecampaignstatus(int campaignId)
        {
            bool status = false;
            //update campaign status from ad approval to the planned
            ChangeCampaignStatusCommand _campaignstatus = new ChangeCampaignStatusCommand();
            _campaignstatus.CampaignProfileId = campaignId;
            //check campaign details
            var campaigndetails = _campaignprofileRepository.Get(top => top.CampaignProfileId == campaignId);
            if (campaigndetails != null)
            {
                if (campaigndetails.StartDate == null && campaigndetails.EndDate == null)
                {
                    _campaignstatus.Status = (int)CampaignStatus.Play;
                }
                else
                {
                    if (campaigndetails.StartDate != null)
                    {
                        if (campaigndetails.StartDate.Value.Date == DateTime.Now.Date)
                        {
                            _campaignstatus.Status = (int)CampaignStatus.Play;
                        }
                        else
                        {
                            _campaignstatus.Status = (int)CampaignStatus.Planned;
                        }
                    }
                    else
                    {
                        _campaignstatus.Status = (int)CampaignStatus.Planned;
                    }
                }
            }
            ICommandResult campaignstatuscommandResult = _commandBus.Submit(_campaignstatus);
            if (campaignstatuscommandResult.Success)
            {
                status = true;

            }
            return status;
        }




    }
}
