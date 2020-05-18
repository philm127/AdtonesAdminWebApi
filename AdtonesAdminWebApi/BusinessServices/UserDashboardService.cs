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
        private readonly IUserDashboardQuery _commandText;

        ReturnResult result = new ReturnResult();


        public UserDashboardService(IConfiguration configuration, IHttpContextAccessor httpAccessor,IUserDashboardDAL dashboardDal,
                                        IUserDashboardQuery commandText)
        {
            _configuration = configuration;
            _httpAccessor = httpAccessor;
            _dashboardDal = dashboardDal;
            _commandText = commandText;
        }


        public async Task<ReturnResult> LoadAdvertiserDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetAdvertiserDashboard(_commandText.AdvertiserResultQuery);
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
                result.body = await _dashboardDal.GetOperatorDashboard(_commandText.OperatorResultQuery);
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
                result.body = await _dashboardDal.GetSubscriberDashboard(_commandText.SubscriberResultQuery);
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
