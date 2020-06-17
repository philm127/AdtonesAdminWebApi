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


        /// TODO: When not test set test to false. have this here so don't send shit to real servers.
        /// <summary>
        /// Gets first connection string value by Operator. I have removed all CountryId as stupid
        /// </summary>
        /// <param name="OperatorId">used as a named and default parameter</param>
        /// <returns>string value of a ConnectionString</returns>
        public async Task<string> GetSingleConnectionString(int Id = 0)
        {
            /// TODO: This needs to be sorted in a better manner
            var test = _configuration.GetValue<bool>("Environment:Test");
            if (test)
                return _configuration.GetConnectionString("TestProvoConnection");
            else
            {
                StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings WHERE OperatorId=@Id");


                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<string>(sb.ToString(), new { Id = Id });
                }
            }
        }


        /// TODO: When not test set test to false. have this here so don't send shit to real servers.
        /// <summary>
        /// Gets a list of strings by either operator or country
        /// </summary>
        /// <param name="Id">uses OperatorId</param>
        /// <returns>IEnumerable List of strings</returns>
        public async Task<IEnumerable<string>> GetConnectionStrings(int Id=0)
        {
            var test = true;
            if (test)
            {
                List<string> str = null;
                str.Add(_configuration.GetConnectionString("TestProvoConnection"));
                return str;
            }
            else
            {
                StringBuilder sb = new StringBuilder("SELECT ConnectionString FROM CountryConnectionStrings WHERE OperatorId=@Id");

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    if (Id > 0)
                        return await connection.QueryAsync<string>(sb.ToString(), new { Id = Id });
                    else
                        return await connection.QueryAsync<string>(sb.ToString());
                }
            }
        }


        public async Task<int> GetUserIdFromAdtoneId(int Id, int operatorId)
        {
                var conn = await GetSingleConnectionString(operatorId);
            var select_string = "SELECT UserId FROM Users WHERE AdtoneServerUserId=@Id";

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<int>(select_string, new { Id = Id });
                }
        }


        public async Task<int> GetCampaignProfileIdFromAdtoneId(int Id, int operatorId)
        {
            
                var conn = await GetSingleConnectionString(operatorId);
                StringBuilder sb = new StringBuilder("SELECT CampaignProfileId FROM CampaignProfile WHERE AdtoneServerCampaignProfileId=@Id");

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<int>(sb.ToString(), new { Id = Id });
                }
        }


        public async Task<int> GetAdvertIdFromAdtoneId(int Id, int operatorId)
        {

            var conn = await GetSingleConnectionString(operatorId);
            StringBuilder sb = new StringBuilder("SELECT AdvertId FROM Advert WHERE AdtoneServerAdvertId=@Id");

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int>(sb.ToString(), new { Id = Id });
            }
        }

    }
}
