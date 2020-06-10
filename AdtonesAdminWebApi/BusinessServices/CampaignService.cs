
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;


namespace AdtonesAdminWebApi.BusinessServices
{
    public class CampaignService : ICampaignService
    {
        private readonly IConfiguration _configuration;
        public IConnectionStringService _connService { get; }
        // private readonly ISaveFiles _saveFile;
        ReturnResult result = new ReturnResult();

        IHttpContextAccessor _httpAccessor;
        private readonly ICampaignDAL _campDAL;
        private readonly ICheckExistsDAL _existDAL;

        public CampaignService(IConfiguration configuration, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                ICampaignDAL campDAL, ICheckExistsDAL existDAL) //ISaveFiles saveFile)

        {
            _configuration = configuration;
            _connService = connService;
           // _saveFile = saveFile;
            _httpAccessor = httpAccessor;
            _campDAL = campDAL;
            _existDAL = existDAL;
        }


        /// <summary>
        /// Populate the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadCampaignDataTable(int id=0)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    result.body = await _campDAL.GetCampaignResultSet(id);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "LoadCampaignDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadPromoCampaignDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    result.body = await _campDAL.GetPromoCampaignResultSet();
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "LoadPromoCampaignDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadCampaignCreditDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    result.body = await _campDAL.GetCampaignCreditResultSet();
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "LoadCampaignCreditDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Changed for the actual campaign directly
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                // Need to do this to get OperatorId
                CampaignProfile _campProfile = await _campDAL.GetCampaignProfileDetail(model.id);
                bool exists = false;

                exists = await _existDAL.CheckCampaignBillingExists(model.id);

                if (!exists)
                    _campProfile.Status = (int)Enums.CampaignStatus.InsufficientFunds;
                else
                    _campProfile.Status = model.status;

                result.body = await _campDAL.ChangeCampaignProfileStatus(_campProfile);
                var x = await _campDAL.ChangeCampaignProfileStatusOperator(_campProfile);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "UpdateStatus"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Changed when advert status changed, called by Adverts changed status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ChangeCampaignStatus(int campaignId)
        {
            try
            {
                CampaignProfile _campProfile = await _campDAL.GetCampaignProfileDetail(campaignId);

                if (_campProfile != null)
                {
                    bool exists = false;

                    exists = await _existDAL.CheckCampaignBillingExists(campaignId);

                    if (exists)
                    {

                        if (_campProfile.StartDate == null && _campProfile.EndDate == null)
                        {
                            _campProfile.Status = (int)Enums.CampaignStatus.Play;
                        }
                        else
                        {
                            if (_campProfile.StartDate != null)
                            {
                                if (_campProfile.StartDate == DateTime.Now.Date)
                                {
                                    _campProfile.Status = (int)Enums.CampaignStatus.Play;
                                }
                                else
                                {
                                    _campProfile.Status = (int)Enums.CampaignStatus.Planned;
                                }
                            }
                            else
                            {
                                _campProfile.Status = (int)Enums.CampaignStatus.Planned;
                            }
                        }
                    }
                    else
                    {
                        _campProfile.Status = (int)Enums.CampaignStatus.InsufficientFunds;
                    }

                    var y = await _campDAL.ChangeCampaignProfileStatus( _campProfile);
                    var x = await _campDAL.ChangeCampaignProfileStatusOperator( _campProfile);

                    return true;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "ChangeCampaignStatus"
                };
                _logging.LogError();
                return false;
            }
            return true;
        }


        public async Task<ReturnResult> UpdatePromotionalCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                result.body = await _campDAL.UpdatePromotionalCampaignStatus(model);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "UpdatePromotionalCampaignStatus"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        //public async Task<ReturnResult> AddPromotionalCampaign(PromotionalCampaignResult model)
        //{

        //    if (model.Files != null)
        //    {
        //        /// TODO: How to do this for testing?
        //        var operatorConnectionString = await _connService.GetSingleConnectionString(OperatorId: model.OperatorID);

        //        if (!string.IsNullOrEmpty(operatorConnectionString))
        //        {
        //            bool exists = false;
        //            using (var connection = new SqlConnection(_configuration.GetConnectionString("operatorConnectionString")))
        //            {
        //                exists = await connection.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM PromotionalCamappaigns WHERE BatchID=@batch;",
        //                                                              new { batch = model.BatchID });
        //            }

        //            if (exists)
        //            {
        //                result.error = "BatchID exists for this operator.";
        //                result.result = 0;
        //                return result;
        //            }
        //            else
        //            {
        //                var mediaFile = model.Files;
        //                string fileName = string.Empty;

        //                string extension = Path.GetExtension(mediaFile.FileName);
        //                var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);

        //                string outputFormat = "wav";
        //                var audioFormatExtension = "." + outputFormat;

        //                if (extension != audioFormatExtension)
        //                {
        //                    // Put this to return either the filename or filepath from service
        //                    string tempDirectoryName = "/PromotionalMedia/Temp/";
        //                    string nameOrPath = "path";
        //                    string tempPath = await _saveFile.SaveFileToSite(tempDirectoryName, mediaFile, nameOrPath);

        //                    var convert = new ConvertSaveMediaFile();
        //                    var success = convert.ConvertAndSaveMediaFile(tempPath, extension, model.OperatorID.ToString(), fileName, outputFormat);
        //                    model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}",
        //                                                  model.OperatorID.ToString(), fileName + "." + outputFormat);
        //                }
        //                else
        //                {
        //                    string directoryName = "/PromotionalMedia/";
        //                    directoryName = Path.Combine(directoryName, model.OperatorID.ToString());
        //                    string path = Path.Combine(directoryName, fileName + extension);
        //                    string mpath = await _saveFile.SaveFileToSite(path, mediaFile, "path");

        //                    string archiveDirectoryName = "/PromotionalMedia/Archive/";

        //                    string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
        //                    string apath = await _saveFile.SaveFileToSite(archivePath, mediaFile, "path");

        //                    model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorID.ToString(),
        //                                                            fileName + extension);
        //                }
        //            }
        //            if (model.OperatorID == (int)Enums.OperatorTableId.Expresso)
        //            {


        //            }
        //            else if (model.OperatorID == (int)Enums.OperatorTableId.Safaricom)
        //            {
        //                #region Media
        //                if (mediaFile.ContentLength != 0)
        //                {
        //                    var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();
        //                    string fileName = firstAudioName;

        //                    string extension = Path.GetExtension(mediaFile.FileName);
        //                    var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
        //                    string outputFormat = model.OperatorId == 1 ? "wav" : model.OperatorId == 2 ? "mp3" : "wav";
        //                    var audioFormatExtension = "." + outputFormat;

        //                    if (extension != audioFormatExtension)
        //                    {
        //                        string tempDirectoryName = Server.MapPath("~/PromotionalMedia/Temp/");
        //                        if (!Directory.Exists(tempDirectoryName))
        //                            Directory.CreateDirectory(tempDirectoryName);
        //                        string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
        //                        mediaFile.SaveAs(tempPath);

        //                        SaveConvertedFile(tempPath, extension, model.OperatorId.ToString(), fileName, outputFormat);

        //                        model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
        //                                                            fileName + "." + outputFormat);
        //                    }
        //                    else
        //                    {
        //                        string directoryName = Server.MapPath("~/PromotionalMedia/");
        //                        directoryName = Path.Combine(directoryName, model.OperatorId.ToString());

        //                        if (!Directory.Exists(directoryName))
        //                            Directory.CreateDirectory(directoryName);

        //                        string path = Path.Combine(directoryName, fileName + extension);
        //                        mediaFile.SaveAs(path);

        //                        string archiveDirectoryName = Server.MapPath("~/PromotionalMedia/Archive/");

        //                        if (!Directory.Exists(archiveDirectoryName))
        //                            Directory.CreateDirectory(archiveDirectoryName);

        //                        string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
        //                        mediaFile.SaveAs(archivePath);

        //                        model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
        //                                                                fileName + extension);
        //                    }

        //                    //Add Promotional Campaign Data on DB Server
        //                    CreateOrUpdatePromotionalCampaignCommand promotionalCampaignCommand =
        //                        Mapper.Map<PromotionalCampaignFormModel, CreateOrUpdatePromotionalCampaignCommand>(model);
        //                    ICommandResult promotionalCampaignResult = _commandBus.Submit(promotionalCampaignCommand);
        //                    if (promotionalCampaignResult.Success)
        //                    {
        //                        int promotionalCampaignId = promotionalCampaignResult.Id;
        //                        string adName = "";
        //                        if (model.AdvertLocation == null || model.AdvertLocation == "")
        //                        {
        //                            adName = "";
        //                        }
        //                        else
        //                        {
        //                            if (model.OperatorId == (int)OperatorTableId.Safaricom)
        //                            {
        //                                var operatorFTPDetails = _operatorFTPDetailsRepository.Get(top => top.OperatorId == model.OperatorId);

        //                                //Transfer Advert File From Operator Server to Linux Server
        //                                var returnValue = CopyAdToOpeartorServer(model.AdvertLocation, model.OperatorId, operatorFTPDetails);
        //                                if (returnValue == "Success")
        //                                {
        //                                    if (operatorFTPDetails != null) adName = operatorFTPDetails.FtpRoot + "/" + model.AdvertLocation.Split('/')[3];

        //                                    //Get and Update Promotional Campaign Data on DB Server
        //                                    var promotionalCampaignData = db.PromotionalCampaigns.Where(top => top.AdtoneServerPromotionalCampaignId == promotionalCampaignId && top.BatchID == model.BatchID && top.CampaignName.ToLower().Equals(model.CampaignName.ToLower())).FirstOrDefault();
        //                                    if (promotionalCampaignData != null)
        //                                    {
        //                                        promotionalCampaignData.AdvertLocation = adName;
        //                                        db.SaveChanges();
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        //Add Promotional Advert Data on DB Server
        //                        PromotionalAdvertFormModel promotionalAdvertModel = new PromotionalAdvertFormModel();
        //                        promotionalAdvertModel.ID = model.ID;
        //                        promotionalAdvertModel.CampaignID = promotionalCampaignId;
        //                        promotionalAdvertModel.OperatorID = model.OperatorID;
        //                        promotionalAdvertModel.AdvertName = model.AdvertName;
        //                        promotionalAdvertModel.AdvertLocation = model.AdvertLocation;

        //                        CreateOrUpdatePromotionalAdvertCommand promotionalAdvertCommand =
        //                            Mapper.Map<PromotionalAdvertFormModel, CreateOrUpdatePromotionalAdvertCommand>(promotionalAdvertModel);
        //                        ICommandResult promotionalAdvertResult = _commandBus.Submit(promotionalAdvertCommand);
        //                        if (promotionalAdvertResult.Success)
        //                        {
        //                            int promotionalAdvertId = promotionalAdvertResult.Id;
        //                            //Get and Update Promotional Advert Data on DB Server
        //                            var promotionalAdvertData = db.PromotionalAdverts.Where(top => top.AdtoneServerPromotionalAdvertId == promotionalAdvertId).FirstOrDefault();
        //                            if (promotionalAdvertData != null)
        //                            {
        //                                promotionalAdvertData.AdvertLocation = adName;
        //                                db.SaveChanges();
        //                            }
        //                        }

        //                        await OnCampaignCreated(model.BatchID);
        //                    }
        //                }
        //                #endregion

        //                result.body = "Campaign and Advert added successfully for operator. ";
        //            }
        //            else
        //            {
        //                result.error = " Operator implementation is under process.";
        //                result.result = 0;
        //                return result;
        //            }
        //        }
        //    }
        //    result.error = " Operator implementation is under process.";
        //    result.result = 0;
        //    return result;
        //}


    }
}
