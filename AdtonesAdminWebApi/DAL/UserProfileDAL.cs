using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels.DTOs.UserProfile;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class UserProfileDAL : IUserProfileDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;

        public UserProfileDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


       


        public async Task<UserProfileDto> GetUserProfileByUserId(int userId)
        {
            string query = @"SELECT UserProfileId, UserId, DOB, Gender, IncomeBracket, WorkingStatus, RelationshipStatus, 
                            Education, HouseholdStatus, Location, MSISDN, AdtoneServerUserProfileId
                            FROM UserProfile WHERE UserId=@Id";
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    return await _executers.ExecuteCommand(_connStr,
                                        conn => conn.QueryFirstOrDefault<UserProfileDto>(query, new { Id = userId }));
                }
            }
            catch(Exception ex)
            {
                var msg = ex.Message.ToString();
            }
            return null;

        }

        public async Task<UserProfilePreferenceDto> GetUserProfilePreferenceByUserProfileId(int userId)
        {
            string query = @"SELECT * FROM UserProfilePreference WHERE UserProfileId=@Id";
            using (var connection = new SqlConnection(_connStr))
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<UserProfilePreferenceDto>(query, new { Id = userId }));
            }
        }
    }
}
