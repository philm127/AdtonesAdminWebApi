
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CampaignService : ICampaignService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public CampaignService(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        /// <summary>
        /// Populate the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadCampaignDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<CampaignAdminResult>(GetCampaignResultSet());

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
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<PromotionalCampaignResult>(GetPromoCampaignResultSet());

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


        public async Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                if (model.status == 2)
                {
                    bool exists = false;
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        exists = await connection.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM Billing WHERE CampaignProfileId=@Id;",
                                                                      new { Id = model.id });
                    }
                    if (!exists)
                        model.status = (int)Enums.CampaignStatus.CampaignPausedDueToInsufficientFunds;
                }
                var update_query = @"UPDATE CampaignProfile SET Status@Status,IsAdminApproval=true WHERE CampaignProfileId=@Id; ";


                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query.ToString(), new { Status = model.status, Id = model.id });
                }
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


        public async Task<ReturnResult> UpdatePromotionalCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                var update_query = @"UPDATE PromotionalCampaigns SET Status@Status,IsAdminApproval=true WHERE CampaignProfileId=@Id; ";


                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query.ToString(), new { Status = model.status, Id = model.id });
                }
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


        public async Task<ReturnResult> AddPromotionalCampaign(PromotionalCampaignResult model)
        {

                    if (model.Files != null)
                    {
                        /// TODO: How to do this for a) completion, b) for testing        
                        string operatorName = _operatorRepository.GetById(model.OperatorId).OperatorName.ToString();

                        var operatorConnectionString = ConnectionString.GetSingleConnectionStringByOperatorId(model.OperatorId);

                        
                        if (!string.IsNullOrEmpty(operatorConnectionString))
                        {
                            EFMVCDataContex db = new EFMVCDataContex(operatorConnectionString);
                            var isBatchIDExist = _promotionalCampaignRepository.GetMany(top => top.BatchID.Equals(model.BatchID)).ToList();
                            if (isBatchIDExist.Count() > 0)
                            {
                                FillCountry(model.CountryId, model.OperatorId);
                                TempData["Error"] = "BatchID exists to " + operatorName + " operator.";
                                return View(model);
                            }
                            else
                            {
                                if (model.OperatorId == (int)OperatorTableId.Expresso)
                                {
                                    #region Media
                                    if (mediaFile.ContentLength != 0)
                                    {
                                        var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();
                                        string fileName = firstAudioName;

                                        string extension = Path.GetExtension(mediaFile.FileName);
                                        var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                        //string outputFormat = model.OperatorId == 1 ? "wav" : model.OperatorId == 2 ? "mp3" : "wav";
                                        string outputFormat = "wav";
                                        var audioFormatExtension = "." + outputFormat;

                                        if (extension != audioFormatExtension)
                                        {
                                            string tempDirectoryName = Server.MapPath("~/PromotionalMedia/Temp/");
                                            string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(tempPath);

                                            SaveConvertedFile(tempPath, extension, model.OperatorId.ToString(), fileName, outputFormat);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                fileName + "." + outputFormat);
                                        }
                                        else
                                        {
                                            string directoryName = Server.MapPath("~/PromotionalMedia/");
                                            directoryName = Path.Combine(directoryName, model.OperatorId.ToString());

                                            if (!Directory.Exists(directoryName))
                                                Directory.CreateDirectory(directoryName);

                                            string path = Path.Combine(directoryName, fileName + extension);
                                            mediaFile.SaveAs(path);

                                            string archiveDirectoryName = Server.MapPath("~/PromotionalMedia/Archive/");

                                            if (!Directory.Exists(archiveDirectoryName))
                                                Directory.CreateDirectory(archiveDirectoryName);

                                            string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(archivePath);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                    fileName + extension);
                                        }

                                        //Add Promotional Campaign Data on DB Server
                                        CreateOrUpdatePromotionalCampaignCommand promotionalCampaignCommand =
                                            Mapper.Map<PromotionalCampaignFormModel, CreateOrUpdatePromotionalCampaignCommand>(model);
                                        ICommandResult promotionalCampaignResult = _commandBus.Submit(promotionalCampaignCommand);
                                        if (promotionalCampaignResult.Success)
                                        {
                                            int promotionalCampaignId = promotionalCampaignResult.Id;
                                            string adName = "";
                                            if (model.AdvertLocation == null || model.AdvertLocation == "")
                                            {
                                                adName = "";
                                            }
                                            else
                                            {
                                                if (model.OperatorId == (int)OperatorTableId.Expresso)
                                                {
                                                    var operatorFTPDetails = _operatorFTPDetailsRepository.Get(top => top.OperatorId == model.OperatorId);

                                                    //Transfer Advert File From Operator Server to Linux Server
                                                    var returnValue = CopyAdToOpeartorServer(model.AdvertLocation, model.OperatorId, operatorFTPDetails);
                                                    if (returnValue == "Success")
                                                    {
                                                        if (operatorFTPDetails != null) adName = operatorFTPDetails.FtpRoot + "/" + model.AdvertLocation.Split('/')[3];

                                                        //Get and Update Promotional Campaign Data on DB Server
                                                        var promotionalCampaignData = db.PromotionalCampaigns.Where(top => top.AdtoneServerPromotionalCampaignId == promotionalCampaignId && top.BatchID == model.BatchID && top.CampaignName.ToLower().Equals(model.CampaignName.ToLower())).FirstOrDefault();
                                                        if (promotionalCampaignData != null)
                                                        {
                                                            promotionalCampaignData.AdvertLocation = adName;
                                                            db.SaveChanges();
                                                        }
                                                    }
                                                }
                                            }

                                            //Add Promotional Advert Data on DB Server
                                            PromotionalAdvertFormModel promotionalAdvertModel = new PromotionalAdvertFormModel();
                                            promotionalAdvertModel.ID = model.ID;
                                            promotionalAdvertModel.CampaignID = promotionalCampaignId;
                                            promotionalAdvertModel.OperatorID = model.OperatorId;
                                            promotionalAdvertModel.AdvertName = model.AdvertName;
                                            promotionalAdvertModel.AdvertLocation = model.AdvertLocation;

                                            CreateOrUpdatePromotionalAdvertCommand promotionalAdvertCommand =
                                                Mapper.Map<PromotionalAdvertFormModel, CreateOrUpdatePromotionalAdvertCommand>(promotionalAdvertModel);
                                            ICommandResult promotionalAdvertResult = _commandBus.Submit(promotionalAdvertCommand);
                                            if (promotionalAdvertResult.Success)
                                            {
                                                int promotionalAdvertId = promotionalAdvertResult.Id;
                                                //Get and Update Promotional Advert Data on DB Server
                                                var promotionalAdvertData = db.PromotionalAdverts.Where(top => top.AdtoneServerPromotionalAdvertId == promotionalAdvertId).FirstOrDefault();
                                                if (promotionalAdvertData != null)
                                                {
                                                    promotionalAdvertData.AdvertLocation = adName;
                                                    db.SaveChanges();
                                                }
                                            }

                                            await OnCampaignCreated(model.BatchID);
                                        }
                                    }
                                    #endregion

                                    TempData["success"] = "Campaign and Advert added successfully for operator " + operatorName + ".";
                                    return RedirectToAction("Index");
                                }
                                else if (model.OperatorId == (int)OperatorTableId.Safaricom)
                                {
                                    #region Media
                                    if (mediaFile.ContentLength != 0)
                                    {
                                        var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();
                                        string fileName = firstAudioName;

                                        string extension = Path.GetExtension(mediaFile.FileName);
                                        var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                        string outputFormat = model.OperatorId == 1 ? "wav" : model.OperatorId == 2 ? "mp3" : "wav";
                                        var audioFormatExtension = "." + outputFormat;

                                        if (extension != audioFormatExtension)
                                        {
                                            string tempDirectoryName = Server.MapPath("~/PromotionalMedia/Temp/");
                                            if (!Directory.Exists(tempDirectoryName))
                                                Directory.CreateDirectory(tempDirectoryName);
                                            string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(tempPath);

                                            SaveConvertedFile(tempPath, extension, model.OperatorId.ToString(), fileName, outputFormat);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                fileName + "." + outputFormat);
                                        }
                                        else
                                        {
                                            string directoryName = Server.MapPath("~/PromotionalMedia/");
                                            directoryName = Path.Combine(directoryName, model.OperatorId.ToString());

                                            if (!Directory.Exists(directoryName))
                                                Directory.CreateDirectory(directoryName);

                                            string path = Path.Combine(directoryName, fileName + extension);
                                            mediaFile.SaveAs(path);

                                            string archiveDirectoryName = Server.MapPath("~/PromotionalMedia/Archive/");

                                            if (!Directory.Exists(archiveDirectoryName))
                                                Directory.CreateDirectory(archiveDirectoryName);

                                            string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(archivePath);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                    fileName + extension);
                                        }

                                        //Add Promotional Campaign Data on DB Server
                                        CreateOrUpdatePromotionalCampaignCommand promotionalCampaignCommand =
                                            Mapper.Map<PromotionalCampaignFormModel, CreateOrUpdatePromotionalCampaignCommand>(model);
                                        ICommandResult promotionalCampaignResult = _commandBus.Submit(promotionalCampaignCommand);
                                        if (promotionalCampaignResult.Success)
                                        {
                                            int promotionalCampaignId = promotionalCampaignResult.Id;
                                            string adName = "";
                                            if (model.AdvertLocation == null || model.AdvertLocation == "")
                                            {
                                                adName = "";
                                            }
                                            else
                                            {
                                                if (model.OperatorId == (int)OperatorTableId.Safaricom)
                                                {
                                                    var operatorFTPDetails = _operatorFTPDetailsRepository.Get(top => top.OperatorId == model.OperatorId);

                                                    //Transfer Advert File From Operator Server to Linux Server
                                                    var returnValue = CopyAdToOpeartorServer(model.AdvertLocation, model.OperatorId, operatorFTPDetails);
                                                    if (returnValue == "Success")
                                                    {
                                                        if (operatorFTPDetails != null) adName = operatorFTPDetails.FtpRoot + "/" + model.AdvertLocation.Split('/')[3];

                                                        //Get and Update Promotional Campaign Data on DB Server
                                                        var promotionalCampaignData = db.PromotionalCampaigns.Where(top => top.AdtoneServerPromotionalCampaignId == promotionalCampaignId && top.BatchID == model.BatchID && top.CampaignName.ToLower().Equals(model.CampaignName.ToLower())).FirstOrDefault();
                                                        if (promotionalCampaignData != null)
                                                        {
                                                            promotionalCampaignData.AdvertLocation = adName;
                                                            db.SaveChanges();
                                                        }
                                                    }
                                                }
                                            }

                                            //Add Promotional Advert Data on DB Server
                                            PromotionalAdvertFormModel promotionalAdvertModel = new PromotionalAdvertFormModel();
                                            promotionalAdvertModel.ID = model.ID;
                                            promotionalAdvertModel.CampaignID = promotionalCampaignId;
                                            promotionalAdvertModel.OperatorID = model.OperatorId;
                                            promotionalAdvertModel.AdvertName = model.AdvertName;
                                            promotionalAdvertModel.AdvertLocation = model.AdvertLocation;

                                            CreateOrUpdatePromotionalAdvertCommand promotionalAdvertCommand =
                                                Mapper.Map<PromotionalAdvertFormModel, CreateOrUpdatePromotionalAdvertCommand>(promotionalAdvertModel);
                                            ICommandResult promotionalAdvertResult = _commandBus.Submit(promotionalAdvertCommand);
                                            if (promotionalAdvertResult.Success)
                                            {
                                                int promotionalAdvertId = promotionalAdvertResult.Id;
                                                //Get and Update Promotional Advert Data on DB Server
                                                var promotionalAdvertData = db.PromotionalAdverts.Where(top => top.AdtoneServerPromotionalAdvertId == promotionalAdvertId).FirstOrDefault();
                                                if (promotionalAdvertData != null)
                                                {
                                                    promotionalAdvertData.AdvertLocation = adName;
                                                    db.SaveChanges();
                                                }
                                            }

                                            await OnCampaignCreated(model.BatchID);
                                        }
                                    }
                                    #endregion

                                    TempData["success"] = "Campaign and Advert added successfully for operator " + operatorName + ".";
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Error"] = operatorName + " operator implementation is under process.";
                                }
                            }
                        }
                        else TempData["Error"] = operatorName + " operator implementation is under process.";
                    }

        }



        #region Longer SQL Query


        private string GetCampaignResultSet()
        {
            return @"SELECT u.Email,CONCAT(u.FirstName, ' ',u.LastName) AS FullName,
                    ISNULL(cl.Name,'-') AS ClientName,camp.CampaignName,
                    (SELECT TOP 1 ad.AdvertName FROM Advert AS ad WHERE ad.CampProfileId=camp.CampaignProfileId) AS AdvertName,
                    cp.finaltotalplays,
                    (SELECT SUM(ISNULL(TotalBudget,0)) FROM CampaignProfile WHERE CampaignProfileId=camp.CampaignProfileId 
                        GROUP BY CampaignProfileId) AS TotalBudget,
                    cp.TotalSpend,
                        (
	                    (SELECT SUM(ISNULL(TotalBudget,0)) 
		                    FROM CampaignProfile 
		                    WHERE CampaignProfileId=camp.CampaignProfileId 
		                    GROUP BY CampaignProfileId
                     /* For hard of vision is a minus sign below */
	                    ) - cp.TotalSpend) AS FundsAvailable, cp.ABidValue, camp.CreatedDateTime,
                    (SELECT TOP 1 AdvertId FROM Advert AS ad WHERE ad.CampProfileId=camp.CampaignProfileId) AS AdvertId,
                    (SELECT TOP 1 OperatorId FROM Advert WHERE CampProfileId=camp.CampaignProfileId) AS OperatorId,
                    (SELECT COUNT(Id) FROM Question WHERE CampaignProfileId=camp.CampaignProfileId GROUP BY CampaignProfileId) AS TicketCount,
                    (CASE 
	                    WHEN 
		                    ISNULL((SELECT COUNT(CampaignProfileId) FROM Billing WHERE CampaignProfileId=camp.CampaignProfileId 
                            GROUP BY CampaignProfileId ),0)=0 
	                    THEN 8 
                        ELSE camp.Status END) AS Status,
	                camp.CampaignProfileId,camp.UserId,camp.IsAdminApproval,camp.CountryId
                    FROM CampaignProfile AS camp INNER JOIN Users AS u ON camp.UserId=u.UserId
                    LEFT JOIN Client AS cl ON camp.ClientId=cl.Id
                    LEFT JOIN
                        (SELECT CampaignProfileId,COUNT(CampaignProfileId) AS finaltotalplays,
	                    CAST(AVG(ISNULL(BidValue,0)) AS decimal(18,4)) AS ABidValue,
	                    CAST((Sum(ISNULL(BidValue,0)) + Sum(ISNULL(SMSCost,0)) + Sum(ISNULL(EmailCost,0))) AS decimal(18,4)) AS TotalSpend,
	                    SUM(ISNULL(SMSCost,0)) AS SMSCost,Sum(ISNULL(EmailCost,0)) AS EmailCost
	                    FROM CampaignAudit
	                    WHERE LOWER(Status)='played' AND PlayLengthTicks > 6000 
	                    GROUP BY CampaignProfileId
	                    HAVING COUNT(CampaignProfileId)>=1) AS cp
                    ON cp.CampaignProfileId=camp.CampaignProfileId
                    ORDER BY camp.CreatedDateTime DESC;";
        }

        
        private string GetPromoCampaignResultSet()
        {
            return @"SELECT promo.ID,promo.OperatorID,op.OperatorName,promo.CampaignName,promo.BatchID,MaxDaily,MaxWeekly,
                        promo.AdvertLocation,promo.Status,pa.AdvertName,
                        CASE WHEN promo.Status=1 THEN 'Play' ELSE 'Stop' END AS rStatus
                        FROM PromotionalCampaigns AS promo 
                        LEFT JOIN PromotionalAdverts AS pa ON pa.CampaignID=promo.ID
                        LEFT JOIN Operators AS op ON op.OperatorId=promo.OperatorID;";
        }


        #endregion
    }
}
