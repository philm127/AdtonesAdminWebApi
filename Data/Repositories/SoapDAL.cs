using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using Domain.Model;
using Domain.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{


    public class SoapDAL : ISoapDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;

        public SoapDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


        public async Task<SoapApiResponseCodes> GetSoapApiResponse(string id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(SoapQuery.GetSoapApiResponseCodes);
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
