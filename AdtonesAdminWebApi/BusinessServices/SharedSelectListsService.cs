using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Text;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.DAL.Queries;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class SharedSelectListsService : ISharedSelectListsService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConfiguration _configuration;
        private readonly ISharedSelectListsDAL _sharedDal;
        private readonly ILoggingService _logServ;
        const string PageName = "SharedSelectListsService";

        ReturnResult result = new ReturnResult();
        

        public SharedSelectListsService(IConfiguration configuration, ISharedSelectListsDAL sharedDal, IHttpContextAccessor httpAccessor,
                                        ILoggingService logServ)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
            _httpAccessor = httpAccessor;
            _logServ = logServ;
        }

        
        /// <summary>
        /// Get users without subscribers with roles tagged on
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> GetUsersnRoles()
        {
            List<SharedSelectListViewModel> selectedList = new List<SharedSelectListViewModel>();
            try
            {
                var dt = await _sharedDal.GetUsersnRoles();
                foreach(dynamic item in dt)
                {
                    var itemModel = new SharedSelectListViewModel();
                    itemModel.Value = item.Value.ToString();
                    itemModel.Text = item.Text + " (" + Enum.GetName(typeof(Enums.UserRole), item.RoleId).ToString()  + ")";
                    selectedList.Add(itemModel);
                }
                result.body = selectedList;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetUsersnRoles";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Retrieves a list of Users who have permissions set up and their role
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> GetUsersWPermissions()
        {
            List<SharedSelectListViewModel> selectedList = new List<SharedSelectListViewModel>();
            try
            {
                var dt = await _sharedDal.GetUserPermissionsWRoles();
                foreach (dynamic item in dt)
                {
                    var itemModel = new SharedSelectListViewModel();
                    itemModel.Value = item.Value.ToString();
                    itemModel.Text = item.Text + " (" + Enum.GetName(typeof(Enums.UserRole), item.RoleId).ToString() + ")";
                    selectedList.Add(itemModel);
                }
                result.body = selectedList;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetUsersWPermissions";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Gets the 4 potential lists to be used by update/add advertiser credit
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> GetUserDetailCreditList()
        {
            var usercreditlist = new AdvertiserCreditSharedListsModel();
            try
            {
                Task<IEnumerable<SharedSelectListViewModel>> userCL = _sharedDal.GetCreditUsers();
                Task<IEnumerable<SharedSelectListViewModel>> countryL = _sharedDal.GetCountry();
                Task<IEnumerable<SharedSelectListViewModel>> currencyL = _sharedDal.GetCurrency(0);
                Task<IEnumerable<SharedSelectListViewModel>> addUserCL = _sharedDal.AddCreditUsers();

                await Task.WhenAll(userCL, countryL, currencyL, addUserCL);

                usercreditlist.country = countryL.Result;
                usercreditlist.users = userCL.Result;
                usercreditlist.currency = currencyL.Result;
                usercreditlist.addUser = addUserCL.Result;
                result.body = usercreditlist;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetUserCreditList";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


    }
}