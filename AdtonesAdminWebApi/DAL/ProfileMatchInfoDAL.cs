using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
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
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(ProfileMatchInfoQuery.LoadDataTable);
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.GetProfileInfo);
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


        public async Task<IEnumerable<ProfileMatchLabelFormModel>> GetProfileLabelById(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.GetProfileInfoLabels);
            builder.AddParameters(new { id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<ProfileMatchLabelFormModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateProfileInfo(ProfileMatchInformationFormModel model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.UpdateProfileInfo);
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.AddProfileInfo);
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.InsertProfileInfoLabel);
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.UpdateProfileInfoLabel);
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.DeleteMatchLabel);
            builder.AddParameters(new { id = id });

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


        public async Task<bool> CheckIfProfileInfoExists(ProfileMatchInformationFormModel model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(ProfileMatchInfoQuery.CheckIfProfileExists);
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
