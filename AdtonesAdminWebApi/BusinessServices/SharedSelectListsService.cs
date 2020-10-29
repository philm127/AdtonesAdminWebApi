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

        ReturnResult result = new ReturnResult();
        

        public SharedSelectListsService(IConfiguration configuration, ISharedSelectListsDAL sharedDal, IHttpContextAccessor httpAccessor)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
            _httpAccessor = httpAccessor;
        }


        public async Task<ReturnResult> GetCountryList()
        {
            try
            {
                result.body = await _sharedDal.GetCountry();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetCountryList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetOperatorList(int countryId = 0)
        {
            try
            {
                result.body = await _sharedDal.GetOperators(countryId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetOperatorList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetOrganisationTypeDropDown()
        {
            try
            {
                result.body = await _sharedDal.GetOrganisationTypes();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetOperatorList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetCurrencyList(int currencyId=0)
        {
            var tst = _httpAccessor.GetUserIdFromJWT();
            var str = _httpAccessor.GetRoleFromJWT();
            var ytr = _httpAccessor.GetRoleIdFromJWT();
            var cvb = _httpAccessor.GetOperatorFromJWT();

            try
            {
                result.body = await _sharedDal.GetCurrency(currencyId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetCurrencyList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public ReturnResult GetUserRole()
        {
            IEnumerable<UserRole> userroleTypes = Enum.GetValues(typeof(UserRole))
                                                     .Cast<UserRole>();
            result.body = (from action in userroleTypes
                    select new SharedSelectListViewModel
                    {
                        Text = action.ToString(),
                        Value = ((int)action).ToString()
                    }).ToList();
            return result;
        }


        public ReturnResult GetUserStatus()
        {
            IEnumerable<UserStatus> userTypes = Enum.GetValues(typeof(UserStatus))
                                                     .Cast<UserStatus>();
            result.body = (from action in userTypes
                    select new SharedSelectListViewModel
                    {
                        Text = action.ToString(),
                        Value = ((int)action).ToString()
                    }).ToList();
            return result;
        }


        public async Task<ReturnResult> GetUserCreditList()
        {
            try
            {
                result.body = await _sharedDal.GetCreditUsers();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetUserCreditList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetUsersnRoles"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetUsersWPermissions"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// When Add Credit selected this populates dropdown with credit users
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> GetAddCreditUsersList()
        {
            try
            {
                result.body = await _sharedDal.AddCreditUsers();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetAddCreditUsersList"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetUserCreditList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> FillUserPaymentDropdown()
        {
            try
            {
                result.body = await _sharedDal.GetUserPaymentList(0);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "FillUserPaymentDropdown"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> FillCampaignDropdown(int id=0)
        {
            try
            {
                result.body = await _sharedDal.GetCamapignList(id);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "FillCampaignDropdown"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;

        }

    }
}