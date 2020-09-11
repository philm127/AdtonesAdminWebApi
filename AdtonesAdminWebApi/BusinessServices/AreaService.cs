using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AreaService : IAreaService
    {
        IHttpContextAccessor _httpAccessor;
        private readonly ICountryAreaDAL _areaDAL;

        ReturnResult result = new ReturnResult();


        public AreaService(ICountryAreaDAL areaDAL, IHttpContextAccessor httpAccessor)

        {
            _areaDAL = areaDAL;
            _httpAccessor = httpAccessor;
        }


        // Listing Area
        public async Task<ReturnResult> LoadDataTable()
        {

            try
            {
                result.body = await _areaDAL.LoadAreaResultSet();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "FillAreaResult"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddArea(AreaResult areamodel)
        {
            areamodel.IsActive = true;
            try
            {
                bool exists = await _areaDAL.CheckAreaExists(areamodel);
                if (exists)
                {
                    result.result = 0;
                    result.error = areamodel.AreaName + " Record Exists.";
                    return result;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "AddArea-CheckUnique"
                };
                _logging.LogError();
                result.result = 0;
            }

            try
            {
                var cnt = _areaDAL.AddArea(areamodel);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "AddArea"
                };
                _logging.LogError();
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
                result.body = await _areaDAL.GetAreaById(id);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "GetArea"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateArea(AreaResult areamodel)
        {
            try
            {
                var cnt = await _areaDAL.UpdateArea(areamodel);
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "UpdateArea"
                };
                _logging.LogError();
                result.result = 0;
                result.error = areamodel.AreaName + " Record was not updated.";
                return result;
            }
        }


        public async Task<ReturnResult> DeleteArea(int id)
        {
            try
            {
                var x = await _areaDAL.DeleteAreaById(id);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "DeleteArea"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


    }
}
