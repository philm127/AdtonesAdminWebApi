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
        private readonly ILoggingService _logServ;
        const string PageName = "UserDashboardService";


        ReturnResult result = new ReturnResult();


        public UserDashboardService(IConfiguration configuration, IHttpContextAccessor httpAccessor, IUserDashboardDAL dashboardDal,
                                    ILoggingService logServ)
        {
            _configuration = configuration;
            _httpAccessor = httpAccessor;
            _dashboardDal = dashboardDal;
            _logServ = logServ;
        }


        public async Task<ReturnResult> LoadAdvertiserDataTable()
        {
            var roleName = _httpAccessor.GetRoleFromJWT();
            var operatorId = 0;

            if (roleName.ToLower() == "OperatorAdmin".ToLower())
                operatorId = _httpAccessor.GetOperatorFromJWT();
                

            try
            {
                result.body = await _dashboardDal.GetAdvertiserDashboard(operatorId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdvertiserDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Present sales Exec data list for Sales Manager principally
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadSalesExecDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetSalesExecDashboard();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadSalesExecDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadSalesExecForAdminDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetSalesExecForAdminDashboard();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadSalesExecForAdminDataTable";
                await _logServ.LogError();

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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadOperatorDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadAdminDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetAdminDashboard();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdminDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadAdvertisersForSales(int userId)
        {
            try
            {
                result.body = await _dashboardDal.GetAdvertiserDashboardForSales(userId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadAdvertisersForSales";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadSubscriberDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }

    }
}
