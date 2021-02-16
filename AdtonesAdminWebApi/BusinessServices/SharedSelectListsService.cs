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


        public async Task<ReturnResult> GetCountryList()
        {
            try
            {
                result.body = await _sharedDal.GetCountry();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCountryList";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOperatorList";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOrganisationTypeDropDown";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetAdvertCategoryDropDown(int countryId)
        {
            try
            {
                result.body = await _sharedDal.GetAdvertCategory(countryId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertCategoryDropDown";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetCampaignCategoryDropDown(int countryId = 0)
        {
            try
            {
                result.body = await _sharedDal.GetCampaignCategory(countryId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCampaignCategoryDropDown";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCurrencyList";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetClientList(int userId = 0)
        {

            try
            {
                result.body = await _sharedDal.GetClientList(userId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetClientList";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetTicketSubjectList()
        {

            try
            {
                result.body = await _sharedDal.GetTicketSubjectList();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetTicketSubjectList";
                await _logServ.LogError();

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


        public ReturnResult GetTicketStatus()
        {
            IEnumerable<QuestionStatus> userTypes = Enum.GetValues(typeof(QuestionStatus))
                                                     .Cast<QuestionStatus>();
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetUserCreditList";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAddCreditUsersList";
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


        public async Task<ReturnResult> FillUserPaymentDropdown()
        {
            try
            {
                result.body = await _sharedDal.GetUserPaymentList(0);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "FillUserPaymentDropdown";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> FillPaymentTypeDropdown()
        {
            try
            {
                result.body = await _sharedDal.GetPaymentTypeList();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "FillPaymentTypeDropdown";
                await _logServ.LogError();

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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "FillCampaignDropdown";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;

        }

    }
}