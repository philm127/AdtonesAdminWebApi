using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IConfiguration _configuration;
        IHttpContextAccessor _httpAccessor;
        private readonly IUserDashboardDAL _dashboardDal;


        ReturnResult result = new ReturnResult();


        public UserDashboardService(IConfiguration configuration, IHttpContextAccessor httpAccessor,IUserDashboardDAL dashboardDal)
        {
            _configuration = configuration;
            _httpAccessor = httpAccessor;
            _dashboardDal = dashboardDal;
        }


        public async Task<ReturnResult> LoadAdvertiserDataTable()
        {
            var roleName = _httpAccessor.GetRoleFromJWT();

            if (roleName.ToLower() == "OperatorAdmin".ToLower())
                return await LoadOperatorAdvertiserDataTable();


            try
            {
                result.body = await _dashboardDal.GetAdvertiserDashboard();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserDashboardService",
                    ProcedureName = "LoadAdvertiserDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadOperatorAdvertiserDataTable(int operatorId = 0)
        {
            if (operatorId == 0)
            {
                operatorId = _httpAccessor.GetOperatorFromJWT();
            }

            try
            {
                result.body = await _dashboardDal.GetAdvertiserDashboard(operatorId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserDashboardService",
                    ProcedureName = "LoadAdvertiserDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadOperatorDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetOperatorDashboard();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserDashboardService",
                    ProcedureName = "LoadOperatorDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadSubscriberDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetSubscriberDashboard();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserDashboardService",
                    ProcedureName = "LoadSubscriberDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }

    }
}
