using AdtonesAdminWebApi.BusinessServices.Interfaces;
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


        public CountryService(IConfiguration configuration, ISaveGetFiles saveFile)

        {
            _configuration = configuration;
            _saveFile = saveFile;
        }


        public async Task<ReturnResult> LoadDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    result.body = await connection.QueryAsync<CountryResult>(@"SELECT Id,Name,ShortName,CountryCode,CreatedDate,Status,TaxPercentage 
                                                                             FROM Country ORDER BY Id DESC");
                }

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


        public async Task<ReturnResult> GetCountry(IdCollectionViewModel countrymodel)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    result.body = await connection.QueryAsync<CountryResult>(@"SELECT Id,Name,ShortName,CountryCode,CreatedDate,Status,TaxPercentage 
                                                                             FROM Country WHERE Id=@id",new { id=countrymodel.countryId});
                }

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


        public async Task<ReturnResult> AddCountry(CountryFormModel countrymodel)
        {
            try
            {
                if (CheckIfCountryExists(countrymodel))
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
                    string directoryName = "/TermAndCondition/";
                    // Put this to return either the filename or filepath from service
                    string nameOrPath = "name";
                    string path = await _saveFile.SaveFileToSite(directoryName, countrymodel.file, nameOrPath);
                    countrymodel.TermAndConditionFileName = path;

                    string insert_string = @"INSERT INTO Country(UserId,Name,ShortName,CreatedDate,UpdatedDate,Status,
                                            TermAndConditionFileName,CountryCode,TaxPercentage)
                                        VALUES(@UserId,@Name,@ShortName, GETDATE(), GETDATE(), @Status, 
                                                @TermAndConditionFileName, @CountryCode,@TaxPercentage);";

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        result.body = await connection.ExecuteAsync(insert_string, countrymodel);
                    }
                }
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


        public async Task<ReturnResult> UpdateCountry(CountryFormModel countrymodel)
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
                    string directoryName = "/TermAndCondition/";

                    if (!string.IsNullOrEmpty(countrymodel.TermAndConditionFileName))
                    {
                        bool delfile = _saveFile.DeleteFileByName(directoryName, countrymodel.TermAndConditionFileName);
                    }

                    // Put this to return either the filename or filepath from service
                    string nameOrPath = "name";
                    string path = await _saveFile.SaveFileToSite(directoryName, countrymodel.file, nameOrPath);
                    countrymodel.TermAndConditionFileName = path;
                }
                string update_string = @"UPDATE Country SET UserId = @UserId, Name = @Name, ShortName = @ShortName, 
                                                            UpdatedDate = GETDATE(),Status = @Status, CountryCode = @CountryCode,
                                                            TermAndConditionFileName = @TermAndConditionFileName,TaxPercentage=@TaxPercentage
                                                                                                                        WHERE Id=@Id;";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    result.body = await connection.ExecuteAsync(update_string,countrymodel);
                }
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


        private bool CheckIfCountryExists(CountryFormModel label)
        {
            bool countryExist = false;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                countryExist = connection.ExecuteScalar<bool>(@"SELECT COUNT(1) FROM Country 
                                                                    WHERE LOWER(Name) = @name",
                                                                    new
                                                                    {
                                                                        name = label.Name.Trim().ToLower()
                                                                    });
            }
            return countryExist;

        }


    }
}
