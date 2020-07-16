using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
                PermissionList listStr = new PermissionList();
                var str =  JsonSerializer.Deserialize<List<PermissionModel>>(await _permDAL.GetPermissionsByUserId(id));
                listStr.pages = str.ToList();
                // str.ToList().ForEach(l => listStr.Add(l));
                result.body = listStr;// await _permDAL.GetPermissionsByUserId(id);
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


        

    }
}
