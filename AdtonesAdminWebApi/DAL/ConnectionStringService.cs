using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;

namespace AdtonesAdminWebApi.DAL
{
    /// <summary>
    /// Used to get alternative connection strings for Provisioning or Delivery Servers based on either country or operator.
    /// Not sure why country as a bit lame if 2 operators in same country.
    /// </summary>
    public class ConnectionStringService : IConnectionStringService
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets first connection string value based on either Countryt or Operator
        /// </summary>
        /// <param name="CountryID">used as a named and default parameter</param>
        /// <param name="OperatorId">used as a named and default parameter</param>
        /// <returns>string value of a ConnectionString</returns>
        public async Task<string> GetSingleConnectionString(int CountryID = 0, int OperatorId = 0)
        {
            StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings WHERE ");
            int Id = 0;

            if (CountryID > 0)
            {
                sb.Append("CountryId=@Id");
                Id = CountryID;
            }
            else if (OperatorId > 0)
            {
                sb.Append("OperatorId=@Id");
                Id = OperatorId;
            }


            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<string>(sb.ToString(), new { Id = Id });
            }
        }

        /// <summary>
        /// Gets a list of strings by either operator or country
        /// </summary>
        /// <param name="CountryID">used as a named and default parameter</param>
        /// <param name="OperatorId">used as a named and default parameter</param>
        /// <returns>IEnumerable List of strings</returns>
        public async Task<IEnumerable<string>> GetConnectionStrings(int CountryID = 0, int OperatorId = 0)
        {
            StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings");
            int Id = 0;

            if (CountryID > 0)
            {
                sb.Append(" WHERE CountryId=@Id");
                Id = CountryID;
            }
            else if (OperatorId > 0)
            {
                sb.Append(" WHERE OperatorId=@Id");
                Id = OperatorId;
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                if(Id>0)
                    return await connection.QueryAsync<string>(sb.ToString(), new { Id = Id });
                else
                    return await connection.QueryAsync<string>(sb.ToString());
            }
        }

    }
}
