using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
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


    public class CampaignDAL : BaseDAL, ICampaignDAL
    {

        public CampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                            IHttpContextAccessor httpAccessor) : base(configuration, executers, connService, httpAccessor)
        {}


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(CampaignQuery.GetCampaignResultSet);

            if (id > 0)
            {
                sb.Append(" WHERE camp.UserId=@Id ");
                sb.Append(" AND camp.Status IN(1,2,3,4) ");
                builder.AddParameters(new { Id = id });
            }
            else
            {
                // had to put this here as the check file picks up WHERE on inner queries
                sb.Append(" WHERE 1=1 ");
            }

            var values = CheckGeneralFile(sb, builder,pais:"camp",ops:"op",advs:"camp");

            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
            var select = builder.AddTemplate(sb.ToString());
            
            try
            {
                // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignAdminResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetBySalesExec(int id = 0)
        {
            var sb = new StringBuilder(CampaignQuery.GetCampaignResultSetForSales);
            var builder = new SqlBuilder();
            if (id > 0)
            {
                sb.Append(" WHERE sales.IsActive=1 ");
                sb.Append(" AND sales.SalesExecId=@Sid ");
                builder.AddParameters(new { Sid = id });
            }
            else
            {
                sb.Append(" WHERE 1=1 ");
            }
            var values = CheckGeneralFile(sb, builder, pais: "con", ops: "op", advs: "camp");

            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
            

            var select = builder.AddTemplate(sb.ToString());
            // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

            return await _executers.ExecuteCommand(_connStr,
                            conn => conn.Query<CampaignAdminResult>(select.RawSql, select.Parameters));
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetById(int id)
        {

            var sb = new StringBuilder();
            var builder = new SqlBuilder();

                sb.Append(CampaignQuery.GetCampaignResultSet);
                sb.Append(" WHERE camp.CampaignProfileId=@Id ");
                builder.AddParameters(new { Id = id });


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


        public async Task<int> UpdateCampaignCredit(BillingPaymentModel model, int operatorId)
        {

            int x = 0;
            var sb = new StringBuilder();
            sb.Append(CampaignQuery.UpdateCampaignBilling);

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = (int)Enums.CampaignStatus.Play });
                builder.AddParameters(new { FundAmount = model.Fundamount });
                builder.AddParameters(new { Status = (int)Enums.CampaignStatus.Play });

                x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));


                var constr = await _connService.GetConnectionStringByOperator(operatorId);

                if(constr != null && constr.Length > 10)
                {
                    model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    var x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        /// <summary>
        /// Changes status on operators provisioning server
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> ChangeCampaignProfileStatusOperator(CampaignProfile model)
        {
            int country = (int)model.CountryId;
            var operatorConnectionString = await _connService.GetConnectionStringsByCountry(country);

            var sb = new StringBuilder();
            sb.Append(CampaignQuery.UpdateCampaignProfileStatus);
            sb.Append(" AdtoneServerCampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = model.Status });

                var lst = await _connService.GetConnectionStringsByCountry(country);
                List<string> conns = lst.ToList();

                int x = 0;

                foreach (string constr in conns)
                {

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
                }
                return x;
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateCampaignMatch(CampaignProfile model)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(model.OperatorId);

            var sb = new StringBuilder();
            sb.Append(CampaignQuery.UpdateCampaignMatchStatus);

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


        public async Task<CampaignAdverts> GetCampaignAdvertDetailsById(int adId = 0, int campId = 0)
        {
            var sb = new StringBuilder();
            sb.Append(CampaignQuery.GetCampaignAdvertDetailsById);
            if(adId > 0)
                sb.Append(" AdvertId=@Id;");
            else
                sb.Append(" CampaignProfileId=@Id;");
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = adId });

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


        public async Task<int> GetAdvertIdFromCampaignAdvert(int campaignId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignQuery.GetCampaignAdvertDetailsById);
            builder.AddParameters(new { Id = campaignId });

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


        public async Task<int> UpdateCampaignMatchesforBilling(int id = 0, int operatorId = 0)
        {
            int x = 0;
            try
            {
                var constr = await _connService.GetConnectionStringByOperator(operatorId);

                var campId = _connService.GetCampaignProfileIdFromAdtoneId(id, operatorId);
                   x =  await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(CampaignQuery.UpdateCampaignMatchFromBilling, new { Id = campId, Status= (int)Enums.CampaignStatus.Play }));

            }
            catch
            {
                throw;
            }
            return x;
        }

    }
}
