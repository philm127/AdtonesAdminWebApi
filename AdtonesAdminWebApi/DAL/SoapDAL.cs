using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class SoapDAL : ISoapDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly ISoapQuery _commandText;

        public SoapDAL(IConfiguration configuration, IExecutionCommand executers, ISoapQuery commandText)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _commandText = commandText;
        }


        public async Task<SoapApiResponseCodes> GetSoapApiResponse(string id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.GetSoapApiResponseCodes);
            builder.AddParameters(new { returnCode = id });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<SoapApiResponseCodes>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


    }
}
