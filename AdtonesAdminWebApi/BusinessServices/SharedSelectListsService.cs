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
using AdtonesAdminWebApi.DAL.Shared;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class SharedSelectListsService : ISharedSelectListsService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConfiguration _configuration;
        private readonly ISharedSelectListsDAL _sharedDal;
        private readonly ISharedListQuery _commandText;

        ReturnResult result = new ReturnResult();
        

        public SharedSelectListsService(IConfiguration configuration, ISharedSelectListsDAL sharedDal, IHttpContextAccessor httpAccessor, 
                                        ISharedListQuery commandText)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
            _httpAccessor = httpAccessor;
            _commandText = commandText;
        }


        public async Task<ReturnResult> GetCountryList()
        {
            try
            {
                var select_query = @"SELECT Id AS Value,Name AS Text FROM Country";
                result.body = await _sharedDal.GetSelectList(select_query);
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
            StringBuilder sb = new StringBuilder("SELECT OperatorId AS Value,OperatorName AS Text FROM Operators WHERE IsActive=1");
            if (countryId > 0)
                sb.Append(" AND CountryId = @countryId");

            try
            {
                result.body = await _sharedDal.GetSelectList(sb.ToString(),countryId);
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

            var sb = new StringBuilder(_commandText.GetCurrencyList);
            
            if (currencyId > 0) 
                sb.Append(" WHERE CurrencyId = @Id");

            try
            {
                result.body = await _sharedDal.TESTGetSelectListById(sb.ToString(), currencyId);
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


        public async Task<ReturnResult> GetUserById(int userId)
        {
            string query = @"SELECT UserId,OperatorId,Email,FirstName,LastName,DateCreated,Organisation,
                                        Activated,RoleId,OrganisationTypeId,AdtoneServerUserId 
                                        FROM Users WHERE UserId=@UserId";

            try
            {
                    result.body = await _sharedDal.GetUserById(query, userId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetUserById"
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


        

    }
}