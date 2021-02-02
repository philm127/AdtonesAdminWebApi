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
    public class CountryAreaService : ICountryAreaService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();
        private readonly ISaveGetFiles _saveFile;
        private readonly ICountryAreaDAL _caDAL;
        private readonly ILoggingService _logServ;
        const string PageName = "CountryAreaService";

        public CountryAreaService(IConfiguration configuration, ISaveGetFiles saveFile, ICountryAreaDAL caDAL, ILoggingService logServ)

        {
            _configuration = configuration;
            _saveFile = saveFile;
            _caDAL = caDAL;
            _logServ = logServ;
        }


        public async Task<ReturnResult> LoadDataTable()
        {
            try
            {
                result.body = await _caDAL.LoadCountryResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadDataTable";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCountry";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddCountry";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateCountry";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        #region Area

        // Listing Area
        public async Task<ReturnResult> LoadAreaDataTable()
        {

            try
            {
                result.body = await _caDAL.LoadAreaResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "FillAreaResult";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddArea(AreaResult areamodel)
        {
            areamodel.IsActive = true;
            try
            {
                bool exists = await _caDAL.CheckAreaExists(areamodel);
                if (exists)
                {
                    result.result = 0;
                    result.error = areamodel.AreaName + " Record Exists.";
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddArea-CheckUnique";
                await _logServ.LogError();
                
                result.result = 0;
            }

            try
            {
                var cnt = _caDAL.AddArea(areamodel);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddArea";
                await _logServ.LogError();
                
                result.result = 0;
                result.error = areamodel.AreaName + " Record was not inserted.";
                return result;
            }
            return result;
        }


        public async Task<ReturnResult> GetArea(int id)
        {
            try
            {
                result.body = await _caDAL.GetAreaById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetArea";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateArea(AreaResult areamodel)
        {
            try
            {
                var cnt = await _caDAL.UpdateArea(areamodel);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateArea";
                await _logServ.LogError();
                
                result.result = 0;
                result.error = areamodel.AreaName + " Record was not updated.";
                return result;
            }
        }


        public async Task<ReturnResult> DeleteArea(int id)
        {
            try
            {
                var x = await _caDAL.DeleteAreaById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteArea";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        #endregion


        public async Task<ReturnResult> GetMinBid(int countryId)
        {
            try
            {
                result.body = await _caDAL.GetMinBidByCountry(countryId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetMinBid";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }



    }
}
