using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CountryService : ICountryService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();
        private readonly ISaveGetFiles _saveFile;
        private readonly ICountryAreaDAL _caDAL;

        public CountryService(IConfiguration configuration, ISaveGetFiles saveFile, ICountryAreaDAL caDAL)

        {
            _configuration = configuration;
            _saveFile = saveFile;
            _caDAL = caDAL;
        }


        public async Task<ReturnResult> LoadDataTable()
        {
            try
            {
                result.body = await _caDAL.LoadCountryResultSet();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CountryService",
                    ProcedureName = "LoadDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetCountry(int Id)
        {
            try
            {
                result.body = await _caDAL.GetCountryById(Id); 
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CountryService",
                    ProcedureName = "LoadDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddCountry(CountryResult countrymodel)
        {
            try
            {
                if (await _caDAL.CheckCountryExists(countrymodel))
                {
                    result.error = countrymodel.Name + " Already Exists.";
                    result.result = 0;
                    return result;
                }

                if (countrymodel.file != null && countrymodel.file.Length > 0)
                {
                    var extension = Path.GetExtension(countrymodel.file.FileName);
                    if (extension != ".pdf")
                    {
                        result.result = 0;
                        result.error = "Please upload pdf file only.";
                        return result;
                    }

                    /// TODO: Need to sort file saving out
                    string directoryName = "TermAndCondition";

                    string filename = await _saveFile.SaveFileToSite(directoryName, countrymodel.file);
                    countrymodel.TermAndConditionFileName = filename;
                }

                result.body = await _caDAL.AddCountry(countrymodel);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CountryService",
                    ProcedureName = "AddCountry"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateCountry(CountryResult countrymodel)
        {
            var TermAndConditionFileName = countrymodel.file;
            try
            {
                if (TermAndConditionFileName != null && TermAndConditionFileName.Length > 0)
                {
                    var extension = Path.GetExtension(TermAndConditionFileName.FileName);
                    if (extension != ".pdf")
                    {
                        result.error = "Please upload pdf file only.";
                        result.result = 0;
                        return result;
                    }
                    string directoryName = "TermAndCondition";

                    if (!string.IsNullOrEmpty(countrymodel.TermAndConditionFileName))
                    {
                        bool delfile = _saveFile.DeleteFileByName(directoryName, countrymodel.TermAndConditionFileName);
                    }

                    string filename = await _saveFile.SaveFileToSite(directoryName, countrymodel.file);
                    countrymodel.TermAndConditionFileName = filename;
                }
                
                result.body = await _caDAL.UpdateCountry(countrymodel);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CountryService",
                    ProcedureName = "UpdateCountry"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }

    }
}
