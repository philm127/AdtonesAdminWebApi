using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using ClosedXML.Excel;
using AdtonesAdminWebApi.OperatorSpecific;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class PromotionalUsersService : IPromotionalUsersService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IConnectionStringService _connService { get; }
        private readonly IConfiguration _configuration;
        private readonly IExpresso _operatorExpresso;
        private readonly ISafaricom _operatorSafaricom;

        ReturnResult result = new ReturnResult();
        private const string DestinationTableName = "dbo.PromotionalUsers";

        public PromotionalUsersService(IWebHostEnvironment webHostEnvironment, IConnectionStringService connService,
                                        IConfiguration configuration, IExpresso operatorExpresso, ISafaricom operatorSafaricom)
        {
            _webHostEnvironment = webHostEnvironment;
            _connService = connService;
            _configuration = configuration;
            _operatorExpresso = operatorExpresso;
            _operatorSafaricom = operatorSafaricom;
        }


        public async Task<ReturnResult> SavePromotionalUser(PromotionalUserFormModel model)
        {
            string operatorName;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    operatorName = await connection.QueryFirstOrDefaultAsync<string>(@"SELECT OperatorName FROM Operators WHERE 
                                                                                          OperatorId=@id", new { id = model.OperatorId });
                }

                var operatorConnectionString = await _connService.GetSingleConnectionString(OperatorId: model.OperatorId);

                if (!string.IsNullOrEmpty(operatorConnectionString))
                {
                    using (var db = new SqlConnection(_configuration.GetConnectionString(operatorConnectionString)))
                    {
                        var batchIDExist = await db.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM PromotionalUsers WHERE 
                                                                                          BatchId=@id", new { id = model.BatchID });

                        if (batchIDExist)
                        {
                            result.result = 0;
                            result.body = "Batch Id Exist to " + operatorName + " Operator.";
                            return result;
                        }
                    }

                    string countryCode = string.Empty;
                    if (model.OperatorId == (int)OperatorTableId.Expresso)
                        countryCode = "221";
                    else if (model.OperatorId == (int)OperatorTableId.Safaricom)
                        countryCode = "254";

                    HashSet<string> promoMsisdns = ImportExcel(countryCode, model.Files);
                    var existingUsers = new HashSet<string>();

                    using (var db = new SqlConnection(_configuration.GetConnectionString("operatorConnectionString")))
                    {
                        await db.OpenAsync();
                        var existingUserList = await db.QueryAsync<string>(@"SELECT DISTINCT(msisdn) FROM
                                                                                    (SELECT DISTINCT(msisdn) AS msisdn FROM PromotionalUsers
                                                                                    UNION ALL
                                                                                    SELECT DISTINCT(msisdn) AS msisdn FROM UserProfile) t");
                        existingUsers = new HashSet<string>(existingUserList);
                    }

                    promoMsisdns.ExceptWith(existingUsers);

                    if (model.OperatorId == (int)OperatorTableId.Expresso)
                    {
                        var res = await _operatorExpresso.ProcPromotionalUser(promoMsisdns, DestinationTableName,
                                                                                                operatorConnectionString, model);
                    }
                    else if (model.OperatorId == (int)OperatorTableId.Safaricom)
                    {
                        var res = await _operatorSafaricom.ProcPromotionalUser(promoMsisdns, DestinationTableName,
                                                                                                operatorConnectionString, model);
                    }
                    else
                    {
                        result.error = operatorName + " operator implementation is under process.";
                        result.result = 0;
                        return result;
                    }
                    return result;
                }
                else
                {
                    result.error = operatorName + " operator implementation is under process.";
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