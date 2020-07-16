using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class ManagementReportDAL : IManagementReportDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;

        public ManagementReportDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
        }


        public async Task<int> GetreportInts(ManagementReportsSearch search, string query)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<int>(query, new
                                {
                                    operators = search.operators.ToArray(),
                                    start = search.DateFrom,
                                    end = search.DateTo
                                } ));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SpendCredit>> GetTotalCreditCost(ManagementReportsSearch search, string query)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<SpendCredit>(query, new
                                {
                                    operators = search.operators.ToArray(),
                                    start = search.DateFrom,
                                    end = search.DateTo
                                }));
            }
            catch
            {
                throw;
            }
        }

    }
}
