using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class ProfileMatchInfoDAL : BaseDAL, IProfileMatchInfoDAL
    {

        public ProfileMatchInfoDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor) 
            : base(configuration, executers, connService, httpAccessor)
        {
        }


        public async Task<IEnumerable<ProfileMatchInformationFormModel>> LoadProfileResultSet()
        {
            string loadDataTable = @"SELECT prof.Id,prof.ProfileName,
                                                prof.ProfileType,prof.CountryId, c.Name AS CountryName,prof.IsActive 
                                                FROM ProfileMatchInformations AS prof 
                                                LEFT JOIN Country AS c ON prof.CountryId=c.Id 
                                                LEFT JOIN Operators AS op ON op.CountryId=prof.CountryId ";
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(loadDataTable);
            var values = CheckGeneralFile(sb, builder,pais:"op",ops:"op");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY prof.UpdatedDate DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<ProfileMatchInformationFormModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<ProfileMatchInformationFormModel> GetProfileById(int id)
        {
            string getProfileInfo = @"SELECT p.Id, p.ProfileName,p.IsActive,
                                                p.ProfileType,p.CountryId
                                                FROM ProfileMatchInformations p
                                                WHERE p.Id=@id";

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(getProfileInfo);
            builder.AddParameters(new { id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<ProfileMatchInformationFormModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProfileMatchInformationFormModel>> GetProfileMatchInformation(int countryId)
        {
            string getProfileMatchInformation = @"SELECT Id,ProfileName,ProfileType FROM ProfileMatchInformations 
                                                        WHERE LOWER(ProfileType)<>'adtypes'
                                                                AND IsActive=1 AND CountryId=@Id";
            using (var connection = new SqlConnection(_connStr))
            {
                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.Query<ProfileMatchInformationFormModel>(getProfileMatchInformation,
                                                            new { Id = countryId }));
            }
        }


        public async Task<IEnumerable<ProfileMatchLabelFormModel>> GetListProfileLabelById(int id)
        {
            string getProfileInfoLabels = @"SELECT Id,ProfileLabel,ProfileMatchInformationId,
                                                    FORMAT(CreatedDate, 'd', 'en-gb') as CreatedDate
                                                    FROM ProfileMatchLabels
                                                    WHERE ProfileMatchInformationId=@id";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<ProfileMatchLabelFormModel>(getProfileInfoLabels, new { id = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<ProfileMatchLabelFormModel> GetProfileLabelById(int id)
        {
            string getProfileInfoLabels = @"SELECT Id,ProfileLabel,ProfileMatchInformationId,
                                                    FORMAT(CreatedDate, 'd', 'en-gb') as CreatedDate
                                                    FROM ProfileMatchLabels
                                                    WHERE ProfileMatchInformationId=@id";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<ProfileMatchLabelFormModel>(getProfileInfoLabels, new { id = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateProfileInfo(ProfileMatchInformationFormModel model)
        {
            string updateProfileInfo = @"UPDATE ProfileMatchInformations SET IsActive=@IsActive,
                                                    UpdatedDate=GETDATE(),ProfileType=@ProfileType
                                                    WHERE Id=@Id";

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(updateProfileInfo);
            builder.AddParameters(new { IsActive = model.IsActive });
            builder.AddParameters(new { profileType = model.ProfileType.Trim() });
            builder.AddParameters(new { Id = model.Id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }

        }


        public async Task<int> AddProfileInfo(ProfileMatchInformationFormModel model)
        {
            string addProfileInfo = @"INSERT INTO ProfileMatchInformations(ProfileName,IsActive,CountryId,
                                                            CreatedDate,UpdatedDate,ProfileType)
                                                 VALUES(@ProfileName,@IsActive,@CountryId,GETDATE(),GETDATE(),@ProfileType);
                                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(addProfileInfo);
            builder.AddParameters(new { ProfileName = model.ProfileName.Trim() });
            builder.AddParameters(new { IsActive = model.IsActive });
            builder.AddParameters(new { CountryId = model.CountryId });
            builder.AddParameters(new { ProfileType = model.ProfileType.Trim() });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddProfileInfoLabel(ProfileMatchLabelFormModel model)
        {
            string insertProfileInfoLabel = @"INSERT INTO ProfileMatchLabels(ProfileLabel,ProfileMatchInformationId,CreatedDate,UpdatedDate) 
                                                        VALUES(@ProfileLabel,@ProfileMatchInformationId,GETDATE(),GETDATE());";

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(insertProfileInfoLabel);
            builder.AddParameters(new { ProfileLabel = model.ProfileLabel.Trim() });
            builder.AddParameters(new { ProfileMatchInformationId = model.ProfileMatchInformationId });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateProfileInfoLabel(ProfileMatchLabelFormModel model)
        {
            string updateProfileInfoLabel = @"UPDATE ProfileMatchLabels SET ProfileLabel=@ProfileLabel,
                                                        UpdatedDate=GETDATE() WHERE Id=@Id";
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(updateProfileInfoLabel);
            builder.AddParameters(new { ProfileLabel = model.ProfileLabel.Trim() });
            builder.AddParameters(new { Id = model.Id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> DeleteProfileLabelById(int id)
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>("DELETE FROM ProfileMatchLabels WHERE Id=@id", new { id = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckIfProfileInfoExists(ProfileMatchInformationFormModel model)
        {
            string checkIfProfileExists = @"SELECT COUNT(1) FROM ProfileMatchInformations 
                            WHERE LOWER(ProfileName) = @profilename
                            AND CountryId=@countryId AND LOWER(ProfileType)=@profileType";

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(checkIfProfileExists);
            builder.AddParameters(new { profilename = model.ProfileName.Trim().ToLower() });
            builder.AddParameters(new { countryId = model.CountryId });
            builder.AddParameters(new { profileType = model.ProfileType.Trim().ToLower() });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<bool>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }

    }
}
