﻿using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using Domain.Model;
using AdtonesAdminWebApi.Services;
using Domain.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{


    public class CampaignDAL : BaseDAL, ICampaignDAL
    {
        private readonly IAdvertiserFinancialDAL _invDAL;

        public CampaignDAL(IAdvertiserFinancialDAL invDAL, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                            IHttpContextAccessor httpAccessor) : base(configuration, executers, connService, httpAccessor)
        {
            _invDAL = invDAL;
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(CampaignQuery.GetCampaignResultSet);
            if (_httpAccessor.GetRoleIdFromJWT() == (int)Domain.Enums.UserRole.ProfileAdmin)
            {
                sb.Clear();
                sb.Append(CampaignQuery.GetCampaignResultSetForProfile);
                sb.Append(" WHERE camp.UserId=@UserId ");
                builder.AddParameters(new { UserId = _httpAccessor.GetUserIdFromJWT() });
            }
            else if (id > 0)
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


        public int[] GetOperatorFromPermissionForProv()
        {
            return GetOperatorFromPermission();
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetProv(int operatorId, int id = 0)
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

            var values = CheckGeneralFile(sb, builder, pais: "camp");

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
                builder.AddParameters(new { Id = id, siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignProfile>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignProfileUpdate> GetCampaignProfileDetailUpdate(int campaignId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignQuery.GetCampaignProfileById);
            try
            {
                builder.AddParameters(new { Id = campaignId, siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignProfileUpdate>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertCampaignCategory(CampaignCategoryResult model)
        {
            int x = 0;
            int y = 0;
            
            try
            {
                model.AdtoneServerCampaignCategoryId = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(CampaignQuery.AddCampaignCategory, new
                                    {
                                        description = model.Description,
                                        name = model.CategoryName,
                                        Id = model.AdtoneServerCampaignCategoryId,
                                        active = 1,
                                        CountryId = model.CountryId
                                    }));

                var lst = await _connService.GetConnectionStringsByCountry(model.CountryId.GetValueOrDefault());
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    if (constr != null && constr.Length > 10)
                    {

                        y += await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(CampaignQuery.AddCampaignCategory, new
                                        {
                                            description = model.Description,
                                            name = model.CategoryName,
                                            Id = model.AdtoneServerCampaignCategoryId,
                                            active = 1,
                                            CountryId = model.CountryId
                                        }));
                    }
                }

            }
            catch
            {
                throw;
            }
            return x;
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


        public async Task<int> UpdateCampaignCredit(BillingPaymentModel model, string constr)
        {
            var campModel = await GetCampaignProfileDetail(model.CampaignProfileId);
            var available = await _invDAL.GetAvailableCredit(model.AdvertiserId);
            int x = 0;

            var sb = new StringBuilder();
            sb.Append(CampaignQuery.UpdateCampaignBilling);

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = (int)Domain.Enums.CampaignStatus.Play });
                builder.AddParameters(new { TotalBudget = (campModel.TotalBudget + model.Fundamount) });
                builder.AddParameters(new { TotalCredit = (campModel.TotalCredit + model.TotalAmount) });
                builder.AddParameters(new { AvailableCredit = available });

                x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));


                if (constr != null && constr.Length > 10)
                {
                    var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(sb.ToString(), new
                                    {
                                        Id = campId,
                                        Status = (int)Domain.Enums.CampaignStatus.Play,
                                        TotalBudget = (campModel.TotalBudget + model.Fundamount),
                                        TotalCredit = (campModel.TotalCredit + model.TotalAmount),
                                        AvailableCredit = available
                                    }));

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMatchFinancial, new
                                    {
                                        Id = campId,
                                        Status = (int)Domain.Enums.CampaignStatus.Play,
                                        TotalBudget = (campModel.TotalBudget + model.Fundamount),
                                        TotalCredit = (campModel.TotalCredit + model.TotalAmount),
                                        AvailableCredit = available
                                    }));
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
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(model.OperatorId);

            var sb = new StringBuilder();
            sb.Append(CampaignQuery.UpdateCampaignProfileStatus);
            sb.Append(" AdtoneServerCampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = model.Status });


                int x = 0;


                    x = await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
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


        public async Task<bool> CheckCampaignNameExists(string campaignName,int userId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignQuery.CheckCampaignNameExists);
            builder.AddParameters(new { Id = campaignName.ToLower() });
            builder.AddParameters(new { UserId = userId });

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


        public async Task<int> UpdateCampaignMatchesforBilling(int id = 0, string constr = null)
        {
            int x = 0;
            try
            {

                var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(id, constr);
                   x =  await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(CampaignQuery.UpdateCampaignMatchFromBilling, new { Id = campId, Status= (int)Domain.Enums.CampaignStatus.Play }));

            }
            catch
            {
                throw;
            }
            return x;
        }

    }
}
