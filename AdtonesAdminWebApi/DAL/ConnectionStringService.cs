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
        /// Gets first connection string value by Operator. I have removed all CountryId as stupid
        /// </summary>
        /// <param name="OperatorId">used as a named and default parameter</param>
        /// <returns>string value of a ConnectionString</returns>
        public async Task<string> GetConnectionStringByOperator(int Id = 0)
        {

            StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings WHERE OperatorId=@Id");


            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<string>(sb.ToString(), new { Id = Id });
            }
        }


        /// <summary>
        /// Gets a list of strings by either operator or country
        /// </summary>
        /// <param name="Id">uses OperatorId</param>
        /// <returns>IEnumerable List of strings</returns>
        public async Task<IEnumerable<string>> GetConnectionStrings()
        {

            StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings");

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                    return await connection.QueryAsync<string>(sb.ToString());
            }
        }

        public async Task<IEnumerable<string>> GetConnectionStringsByCountry(int Id)
        {

            StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings WHERE CountryId=@Id");

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<string>(sb.ToString(), new { Id = Id });
            }
        }


        public async Task<string> GetConnectionStringsByCountryId(int Id)
        {

            StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings WHERE CountryId=@Id");

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<string>(sb.ToString(), new { Id = Id });
            }
        }


        public async Task<string> GetOperatorConnectionByUserId(int id)
        {

            string select_query = @"SELECT ConnectionString FROM CountryConnectionStrings AS conn 
                                        INNER JOIN Contacts AS ct ON ct.CountryId=conn.CountryId WHERE ct.UserId=@userId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<string>(select_query, new { userId = id });
            }
        }


        public async Task<int> GetUserIdFromAdtoneId(int Id, int operatorId)
        {
                var conn = await GetConnectionStringByOperator(operatorId);
            var select_string = "SELECT UserId FROM Users WHERE AdtoneServerUserId=@Id";

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<int>(select_string, new { Id = Id });
                }
        }


        public async Task<int> GetUserIdFromAdtoneIdByConnString(int Id, string conn)
        {
            var select_string = "SELECT UserId FROM Users WHERE AdtoneServerUserId=@Id";

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int>(select_string, new { Id = Id });
            }
        }


        public async Task<int> GetClientIdFromAdtoneIdByConnString(int Id, string conn)
        {
            var select_string = "SELECT Id FROM Client WHERE AdtoneServerClientId=@Id";

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int>(select_string, new { Id = Id });
            }
        }


        public async Task<int> GetCampaignProfileIdFromAdtoneId(int Id, int operatorId)
        {
            
                var conn = await GetConnectionStringByOperator(operatorId);
                StringBuilder sb = new StringBuilder("SELECT CampaignProfileId FROM CampaignProfile WHERE AdtoneServerCampaignProfileId=@Id");

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<int>(sb.ToString(), new { Id = Id });
                }
        }


        public async Task<int> GetCampaignProfileIdFromAdtoneIdByConnString(int Id, string conn)
        {
            StringBuilder sb = new StringBuilder("SELECT CampaignProfileId FROM CampaignProfile WHERE AdtoneServerCampaignProfileId=@Id");

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int>(sb.ToString(), new { Id = Id });
            }
        }


        public async Task<int> GetAdvertIdFromAdtoneId(int Id, int operatorId)
        {

            var conn = await GetConnectionStringByOperator(operatorId);
            StringBuilder sb = new StringBuilder("SELECT AdvertId FROM Advert WHERE AdtoneServerAdvertId=@Id");

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int>(sb.ToString(), new { Id = Id });
            }
        }


        public async Task<int> GetOperatorIdFromAdtoneId(int operatorId)
        {

            var conn = await GetConnectionStringByOperator(operatorId);
            var query_string = "SELECT OperatorId FROM Operators WHERE AdtoneServerOperatorId=@Id";

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int>(query_string, new { Id = operatorId });
            }
        }


        public async Task<int> GetCountryIdFromAdtoneId(int Id, string conn)
        {
            StringBuilder sb = new StringBuilder("SELECT Id FROM Country WHERE AdtoneServeCountryId=@Id");

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int>(sb.ToString(), new { Id = Id });
            }
        }

    }
}
