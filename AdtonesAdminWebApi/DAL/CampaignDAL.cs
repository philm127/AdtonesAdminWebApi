using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class CampaignDAL : ICampaignDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;
        private readonly IHttpContextAccessor _httpAccessor;

        public CampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                            IHttpContextAccessor httpAccessor)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
            _httpAccessor = httpAccessor;
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0)
        {
            var roleName = _httpAccessor.GetRoleFromJWT();

            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            if (roleName.ToLower().Contains("operator"))
            {
                sb.Append(CampaignQuery.GetCampaignResultSet);
                sb.Append(" WHERE u.OperatorId=@Id ");
                sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
                builder.AddParameters(new { Id = id });
            }
            else if (id == 0)
            {
                sb.Append(CampaignQuery.GetCampaignResultSet);
                sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
            }
            else if ( id > 0 )
            {
                sb.Append(CampaignQuery.GetCampaignResultSet);
                sb.Append(" WHERE camp.UserId=@Id ");
                sb.Append(" AND camp.Status IN(1,2,3,4) ");
                sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
                builder.AddParameters(new { Id = id });
            }


                var select = builder.AddTemplate(sb.ToString());
            
            try
            {
                // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:SiteEmailAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignAdminResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<CampaignCreditResult>> GetCampaignCreditResultSet()
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignQuery.GetCampaignCreditResultSet);
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignCreditResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignProfile> GetCampaignProfileDetail(int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignQuery.GetCampaignProfileById);
            try
            {
                builder.AddParameters(new { Id = id });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignProfile>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> ChangeCampaignProfileStatus(CampaignProfile model)
        {

            var sb = new StringBuilder();
            sb.Append(CampaignQuery.UpdateCampaignProfileStatus);
            sb.Append(" CampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = model.Status });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Changes status on operators provisioning server
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> ChangeCampaignProfileStatusOperator(CampaignProfile model)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorId);

            var sb = new StringBuilder();
            sb.Append(CampaignQuery.UpdateCampaignProfileStatus);
            sb.Append(" AdtoneServerCampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = model.Status });

                return await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignAdverts> GetCampaignAdvertDetailsByAdvertId(int Id)
        {
            var sb = new StringBuilder();
            sb.Append(CampaignQuery.GetCampaignAdvertDetailsById);
            sb.Append(" AdvertId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = Id });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<CampaignAdverts>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckCampaignBillingExists(int campaignId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignQuery.CheckCampaignBillingExists);
            builder.AddParameters(new { Id = campaignId });

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
