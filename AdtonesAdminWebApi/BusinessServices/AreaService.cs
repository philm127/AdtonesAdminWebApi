using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
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
        private readonly IAreaDAL _areaDAL;
        private readonly IAreaQuery _commandText;
        private readonly ICheckExistsDAL _checkExistsDAL;
        private readonly ICheckExistsQuery _checkExistsQuery;

        ReturnResult result = new ReturnResult();


        public AreaService(IAreaDAL areaDAL, IAreaQuery commandText, IHttpContextAccessor httpAccessor, ICheckExistsDAL checkExistsDAL,
                            ICheckExistsQuery checkExistsQuery)

        {
            _areaDAL = areaDAL;
            _commandText = commandText;
            _httpAccessor = httpAccessor;
            _checkExistsQuery = checkExistsQuery;
            _checkExistsDAL = checkExistsDAL;
        }


        // Listing Area
        public async Task<ReturnResult> LoadDataTable()
        {

            try
            {
                result.body = await _areaDAL.LoadAreaResultSet(_commandText.LoadAreaDataTable);
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
                bool exists = await _checkExistsDAL.CheckAreaExists(_checkExistsQuery.CheckAreaExists, areamodel);
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
                var cnt = _areaDAL.AddArea(_commandText.AddArea, areamodel);
                return result;
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
        }


        public async Task<ReturnResult> GetArea(int id)
        {
            try
            {
                result.body = await _areaDAL.GetAreaById(_commandText.GetAreaById, id);
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
                var cnt = await _areaDAL.UpdateArea(_commandText.UpdateArea, areamodel);
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
                var x = await _areaDAL.DeleteAreaById(_commandText.DeleteArea, id);
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
