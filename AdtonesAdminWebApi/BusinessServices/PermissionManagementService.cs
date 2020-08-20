using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
// using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class PermisionManagementService : IPermissionManagementService
    {

        private readonly IConfiguration _configuration;
        private readonly IPermissionManagementDAL _permDAL;
        ReturnResult result = new ReturnResult();


        public PermisionManagementService(IConfiguration configuration, IPermissionManagementDAL permDAL)

        {
            _configuration = configuration;
            _permDAL = permDAL;
        }


        


        /// <summary>
        /// Gets either a list of Profile Information or a single if passed model Id is not zero
        /// </summary>
        /// <param name="model">The id is used to select a single profile</param>
        /// <returns>Either a List or single ProfileInformationResult model</returns>
        public async Task<ReturnResult> GetPermissionsByUser(int id)
        {
            try
            {
                string permPreList = string.Empty;
                var permList = new List<PermissionModel>();
                permPreList = await _permDAL.GetPermissionsByUserId(id);
                if(permPreList != null && permPreList.Length > 50)
                    permList = JsonSerializer.Deserialize<List<PermissionModel>>(permPreList);

                result.body = permList;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PermissionManagementService",
                    ProcedureName = "GetPermissionsByUser"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateUserPermissionsById(PermissionChangeModel model)
        {
            try
            {
                // PermissionList permListOfList = JsonSerializer.Deserialize<PermissionList>(model.permissions.ToString());
                List<PermissionModel> permList = JsonSerializer.Deserialize<List<PermissionModel>>(model.permissions.ToString()); ;// permListOfList.pageData;
                var str = JsonSerializer.Serialize(permList);
                result.body = await _permDAL.UpdateUserPermissions(model.UserId, str);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PermissionManagementService",
                    ProcedureName = "UpdateUserPermissionsById"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }




    }
}
