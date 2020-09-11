using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public abstract class BaseDAL
    {
        public readonly IConfiguration _configuration;
        public readonly string _connStr;
        public readonly IExecutionCommand _executers;
        public readonly IConnectionStringService _connService;
        public readonly IHttpContextAccessor _httpAccessor;

        public BaseDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
            _httpAccessor = httpAccessor;
        }

        


        public async Task<string> GetPermissionsByUserId()
        {
            return await _executers.ExecuteCommand(_connStr,
                             conn => conn.QueryFirstOrDefault<string>(PermissionManagementQuery.GetPermissionById, 
                                                                                                        new { UserId = _httpAccessor.GetUserIdFromJWT() }));
        }


        /// <summary>
        /// Takes the arrays from general permissions to allow access by country, operator, advertisers or all 3.
        /// </summary>
        /// <param name="sb">Stringbuilder of query so far</param>
        /// <param name="builder">A dapper builder holding parameters submitted so far</param>
        /// <param name="pais">This is to allow flexibility to the virtual table name when using country array</param>
        /// <param name="ops">This is to allow flexibility to the virtual table name when using operator array</param>
        /// <param name="advs">This is to allow flexibility to the virtual table name when using advertiser array</param>
        /// <param name="test">This will take op or null, op is for testing local json file for operator before we take data from table</param>
        /// <returns>A Tuple of an updated StringBuilder and an updated Dapper Query Builder</returns>
        public (StringBuilder sbuild, SqlBuilder build) CheckGeneralFile(StringBuilder sb, SqlBuilder builder, string pais = null, string ops = null, 
                                string advs = null, string test = null)
        {
            var genFile = string.Empty;
            genFile = GetPermissionsByUserId().Result;
           
            List<PermissionModel> gen = JsonSerializer.Deserialize<List<PermissionModel>>(genFile);

            var page = gen.Find(u => u.pageName == "GeneralAccess");
            
            var els = page.elements.ToList();

            bool queryUsed = false;

            
            // operators plural as operator is a key word

            var testcty = els.Find(x => x.name == "country").arrayId;

            if (testcty != null)
            {
                int[] country = els.Find(x => x.name == "country").arrayId.ToArray();
                if (country.Length > 0 && pais != null)
                {
                    sb.Append($" AND {pais}.CountryId IN @country ");
                    builder.AddParameters(new { country = country.ToArray() });
                    queryUsed = true;
                }
            }

            var testopo = els.Find(x => x.name == "operator").arrayId;
            if (testopo != null)
            {
                int[] operators = els.Find(x => x.name == "operator").arrayId.ToArray();
                if (operators.Length > 0 && ops != null)
                {
                    sb.Append($" AND {ops}.OperatorId IN @operators ");
                    builder.AddParameters(new { operators = operators.ToArray() });
                    queryUsed = true;
                }
            }

            var testads = els.Find(x => x.name == "advertiser").arrayId;
            if (testads != null)
            {
                int[] advertiser = els.Find(x => x.name == "advertiser").arrayId.ToArray();
                if (advertiser.Length > 0 && advs != null)
                {
                    sb.Append($" AND {advs}.UserId IN @advertiser ");
                    builder.AddParameters(new { advertiser = advertiser.ToArray() });
                    queryUsed = true;
                }
            }

            // If original string does not have Where clause this will add it.
            if(!sb.ToString().ToLower().Contains(" where ") && queryUsed)
            {
                var find = " and ";
                var replace = " WHERE ";
                int Place = sb.ToString().ToLower().IndexOf(find);
                string result = sb.ToString().Remove(Place, find.Length).Insert(Place, replace);
                sb.Clear();
                sb.Append(result);
            }

            return (sb, builder);
        }

        //        public async Task<ReturnResult> GetAll(string sql_query,QModel<T> model)
        //        {
        //            try
        //            {
        //                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //                {
        //                    connection.Open();
        //                    result.body = await connection.QueryAsync<SharedSelectListViewModel>(@"SELECT Id AS Value,Name AS Text FROM Country");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                var _logging = new ErrorLogging()
        //                {
        //                    ErrorMessage = ex.Message.ToString(),
        //                    StackTrace = ex.StackTrace.ToString(),
        //                    PageName = "SharedSelectListsService",
        //                    ProcedureName = "GetCountryList"
        //                };
        //                _logging.LogError();
        //                result.result = 0;
        //            }
        //            return result;
        //        }


        //        public async Task<object> GetAll<T>(string sql_query, object model, dynamic retModel, object clause=null)
        //        {
        ////            var shop = GetAll(typeof(T),retModel);
        //            try
        //            {
        //                var getType = retModel.GetType();
        //                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //                {
        //                    connection.Open();
        //                    return = await connection.QueryAsync<getType>(sql_query);

        //                    {
        //                    }
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //        }
    }
}