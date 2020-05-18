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
using AdtonesAdminWebApi.DAL.Queries;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class PromotionalUsersService : IPromotionalUsersService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IConnectionStringService _connService { get; }
        private readonly IConfiguration _configuration;
        private readonly IExpressoProcessPromoUser _operatorExpresso;
        private readonly ISafaricomProcessPromoUser _operatorSafaricom;
        private readonly IProvisionServerDAL _provisionServer;
        private readonly IProvisionServerQuery  _provisionQuery;

        ReturnResult result = new ReturnResult();
        private const string DestinationTableName = "dbo.PromotionalUsers";

        public PromotionalUsersService(IWebHostEnvironment webHostEnvironment, IConnectionStringService connService, IProvisionServerDAL provisionServer,
                                        IConfiguration configuration, IExpressoProcessPromoUser operatorExpresso, ISafaricomProcessPromoUser operatorSafaricom,
                                        IProvisionServerQuery provisionQuery)
        {
            _webHostEnvironment = webHostEnvironment;
            _provisionServer = provisionServer;
            _connService = connService;
            _configuration = configuration;
            _operatorExpresso = operatorExpresso;
            _operatorSafaricom = operatorSafaricom;
            _provisionQuery = provisionQuery;
        }


        public async Task<ReturnResult> SavePromotionalUser(PromotionalUserFormModel model)
        {

            try
            {
                /// TODO: How to do this for testing?
                var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorId);

                if (!string.IsNullOrEmpty(operatorConnectionString))
                {
                    var batchIDExist = await _provisionServer.GetPromoUserBatchIdCheckForExisting(_provisionQuery.CheckIfBatchExists, operatorConnectionString);
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
                    
                    var existingUserList = await _provisionServer.GetMsisdnCheckForExisting(_provisionQuery.CheckExistingMSISDN, _configuration.GetConnectionString("operatorConnectionString"));

                    var existingUsers = new HashSet<string>();
                    existingUsers = new HashSet<string>(existingUserList);

                    promoMsisdns.ExceptWith(existingUsers);

                    if (model.OperatorId == (int)Enums.OperatorTableId.Expresso)
                    {
                        var res = await _operatorExpresso.ProcPromotionalUser(promoMsisdns, DestinationTableName,
                                                                                                operatorConnectionString, model);
                    }
                    else if (model.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                    {
                        var res = await _operatorSafaricom.ProcPromotionalUser(promoMsisdns, DestinationTableName,
                                                                                                operatorConnectionString, model);
                    }
                    else
                    {
                        result.error = " Operator implementation is under process.";
                        result.result = 0;
                        return result;
                    }
                    return result;
                }
                else
                {
                    result.error = " Operator implementation is under process.";
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
                    PageName = "PromotionalUserService",
                    ProcedureName = "SavePromotionalUser"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "failed to process users";
                return result;
            }
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