using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ClosedXML.Excel;
using AdtonesAdminWebApi.OperatorSpecific;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;


namespace AdtonesAdminWebApi.BusinessServices
{
    public class PromotionalCampaignService : IPromotionalCampaignService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IConnectionStringService _connService { get; }
        private readonly IConfiguration _configuration;
        private readonly IExpressoProcessPromoUser _operatorExpresso;
        // private readonly ISafaricomProcessPromoUser _operatorSafaricom;
        private readonly ISaveGetFiles _saveFile;
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdTransferService _adTransfer;
        private readonly IConvertSaveMediaFile _convFile;
        private readonly IPromotionalCampaignDAL _provisionServer;

        ReturnResult result = new ReturnResult();

        public PromotionalCampaignService(IWebHostEnvironment webHostEnvironment, IConnectionStringService connService, IPromotionalCampaignDAL provisionServer,
                                        IConfiguration configuration, IExpressoProcessPromoUser operatorExpresso, IConvertSaveMediaFile convFile,
                                        ISaveGetFiles saveFile, IAdvertDAL advertDAL, IAdTransferService adTransfer) // ISafaricomProcessPromoUser operatorSafaricom,
        {
            _webHostEnvironment = webHostEnvironment;
            _provisionServer = provisionServer;
            _connService = connService;
            _configuration = configuration;
            _operatorExpresso = operatorExpresso;
            // _operatorSafaricom = operatorSafaricom;
            _saveFile = saveFile;
            _advertDAL = advertDAL;
            _adTransfer = adTransfer;
            _convFile = convFile;
        }


        public async Task<ReturnResult> SavePromotionalUser(PromotionalUserFormModel model)
        {

            try
            {
                    var batchIDExist = await _provisionServer.GetPromoUserBatchIdCheckForExisting(model);
                    if (batchIDExist)
                    {
                        result.result = 0;
                        result.body = "A Batch Id Exists For the Operator ";
                        return result;
                    }

                    string countryCode = string.Empty;
                    if (model.OperatorId == (int)Enums.OperatorTableId.Expresso)
                        countryCode = "221";
                    else if (model.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                        countryCode = "254";

                    HashSet<string> promoMsisdns = ImportExcel(countryCode, model.Files);
                    
                    var existingUserList = await _provisionServer.GetMsisdnCheckForExisting(model.OperatorId);

                    var existingUsers = new HashSet<string>();
                    existingUsers = new HashSet<string>(existingUserList);

                    promoMsisdns.ExceptWith(existingUsers);

                    if (model.OperatorId == (int)Enums.OperatorTableId.Expresso)
                    {
                        var res = await _operatorExpresso.ProcPromotionalUser(promoMsisdns, model);
                    }
                    //else if (model.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                    //{
                    //    var res = await _operatorSafaricom.ProcPromotionalUser(promoMsisdns, model);
                    //}
                    else
                    {
                        result.error = " Operator implementation is under process.";
                        result.result = 0;
                        return result;
                    }
                    return result;

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PromotionalUserService",
                    ProcedureName = "SavePromotionalUser"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "failed to process users";
                return result;
            }
        }


        public async Task<ReturnResult> AddPromotionalCampaign(PromotionalCampaignResult model)
        {
            model.Status = 1;
            try
            {
                bool exists = await _provisionServer.GetPromoCampaignBatchIdCheckForExisting(model);

                if (exists)
                {
                    result.error = "BatchID exists for this operator.";
                    result.result = 0;
                    return result;
                }

                var mediaFile = model.Files;
                if (mediaFile != null && mediaFile.Length != 0)
                {

                    string extension = Path.GetExtension(mediaFile.FileName);
                    var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                    // string outputFormat = model.OperatorId == 1 ? "wav" : model.OperatorId == 2 ? "mp3" : "wav";
                    string outputFormat = "wav";
                    var audioFormatExtension = "." + outputFormat;

                    string fileName = Path.GetFileName(mediaFile.FileName);
                    string actualDirectoryName = "PromotionalMedia";
                    string directoryName = Path.Combine(actualDirectoryName, model.OperatorId.ToString());
                    string newfile = string.Empty;


                    if (extension != audioFormatExtension)
                    {
                        // Put this to return either the filename or filepath from service
                        string tempDirectoryName = @"PromotionalMedia\Temp\";
                        string tempFile = await _saveFile.SaveFileToSite(tempDirectoryName, mediaFile);

                        newfile = _convFile.ConvertAndSaveMediaFile(tempDirectoryName + tempFile, extension, outputFormat, onlyFileName, directoryName);
                        model.AdvertLocation = $"/{actualDirectoryName}/{model.OperatorId.ToString()}/{ newfile}";
                    }
                    else
                    {

                        newfile = await _saveFile.SaveFileToSite(directoryName, mediaFile);

                        string archiveDirectoryName = "PromotionalMedia//Archive";

                        string apath = await _saveFile.SaveOriginalFileToSite(archiveDirectoryName, mediaFile);

                        model.AdvertLocation = $"/{actualDirectoryName}/{model.OperatorId.ToString()}/{ newfile}";
                    }

                    int promoId = 0;
                    int opPromoId = 0;
                    int advertId = 0;
                    try
                    {
                        // Updates both main DB returns id for the promo server.
                        promoId = await _provisionServer.AddPromotionalCampaign(model);

                        // Adds Promo Advert
                        model.ID = promoId;
                        model.AdtoneServerPromotionalCampaignId = null;
                        advertId = await _provisionServer.AddPromotionalAdvert(model);

                        model.AdtoneServerPromotionalCampaignId = promoId;

                    }
                    catch (Exception ex)
                    {
                        var _logging = new ErrorLogging()
                        {
                            ErrorMessage = ex.Message.ToString(),
                            StackTrace = ex.StackTrace.ToString(),
                            PageName = "PromotionalCampaignService",
                            ProcedureName = "AddPromotionalCampaign - Add To Db"
                        };
                        _logging.LogError();
                        result.result = 0;
                        result.error = "Failed to insert into Database.";
                        return result;
                    }
                    try
                    {

                        var operatorFTPDetails = await _advertDAL.GetFtpDetails(model.OperatorId);

                        // Transfer Advert File From Operator Server to Linux Server
                        var returnValue = await _adTransfer.CopyPromoAdToOperatorServer(model, newfile);
                        if (returnValue)
                        {
                            if (operatorFTPDetails != null)
                                model.AdvertLocation = operatorFTPDetails.FtpRoot + "/" + model.AdvertLocation.Split('/')[3];

                            opPromoId = await _provisionServer.AddPromotionalCampaignToOperator(model);

                            model.ID = opPromoId;
                            model.AdtoneServerPromotionalCampaignId = advertId;
                            var opAdvertId = _provisionServer.AddPromotionalAdvertToOperator(model);
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        var _logging = new ErrorLogging()
                        {
                            ErrorMessage = ex.Message.ToString(),
                            StackTrace = ex.StackTrace.ToString(),
                            PageName = "PromotionalCampaignService",
                            ProcedureName = "AddPromotionalCampaign - Add Advert"
                        };
                        _logging.LogError();
                        result.result = 0;
                        result.error = ex.Message.ToString();
                        return result;
                    }
                }
                else
                {
                    result.error = " No valid Media file loaded.";
                    result.result = 0;
                    return result;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PromotionalCampaignService",
                    ProcedureName = "AddPromotionalCampaign - Whole process"
                };
                _logging.LogError();
                result.result = 0;
                result.error = ex.Message.ToString();
                return result;
            }
        }


        public async Task<ReturnResult> UpdatePromotionalCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                // Updates both main and provisioning server
                result.body = await _provisionServer.UpdatePromotionalCampaignStatus(model);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PromotionalCampaignService",
                    ProcedureName = "UpdatePromotionalCampaignStatus"
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

                    result.body = await _provisionServer.GetPromoCampaignResultSet();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PromotionalCampaignService",
                    ProcedureName = "LoadPromoCampaignDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetPromoBatchIdList(int operatorId)
        {
            try
            {
                result.body = await _provisionServer.GetPromoBatchIdList(operatorId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PromotionalCampaignService",
                    ProcedureName = "GetPromoBatchIdList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Method to get data from csv,xls,xlsx file
        /// </summary>
        /// <param name="countryPrefix">Depending on Country for the country dialling code</param>
        /// <param name="file">The excel file</param>
        /// <returns></returns>
        private HashSet<string> ImportExcel(string countryPrefix, IFormFile file)
        {
            var otherpath = _webHostEnvironment.ContentRootPath;
            //Save the uploaded Excel file.
            var fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(file.FileName);
            var filePath = Path.Combine(otherpath + "/PromotionalUserCSV/", file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            HashSet<string> strList = new HashSet<string>();

            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                foreach (IXLRow row in workSheet.Rows())
                {
                    int i = 0;
                    foreach (IXLCell cell in row.Cells())
                    {
                        string MSISDN = "";
                        if (!String.IsNullOrEmpty(cell.Value.ToString()))
                        {
                            string countryCode = cell.Value.ToString().Substring(0, 3);
                            if (countryCode == countryPrefix) MSISDN = cell.Value.ToString();
                            else MSISDN = string.Concat(countryPrefix, cell.Value.ToString());
                            strList.Add(MSISDN);
                            i++;
                        }
                    }
                }
            }
            return strList;
        }

    }
}