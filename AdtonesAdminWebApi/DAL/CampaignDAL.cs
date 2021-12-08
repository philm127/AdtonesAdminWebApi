using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
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
        {
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(CampaignQuery.GetCampaignResultSet);
            if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
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


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetByAdvertiser(int id)
        {
            string selectQuery = @"SELECT camp.CampaignProfileId,camp.UserId,u.Email
                                                ,camp.ClientId, ISNULL(cl.Name,'-') AS ClientName,CampaignName,camp.CreatedDateTime AS CreatedDate,
                                                ad.AdvertName,camp.TotalBudget,camp.IsAdminApproval,
                                                ISNULL(r.UniqueListenrs,0) AS Reach,camp.Status,
												ro.Spend AS TotalSpend,ro.FundsAvailable, ro.MoreSixSecPlays AS finaltotalplays,ro.AvgBid AS AvgBidValue
                                                FROM CampaignProfile AS camp LEFT JOIN Users As u ON u.UserId=camp.UserId
                                                LEFT JOIN Client AS cl ON camp.ClientId=cl.Id
                                                LEFT JOIN CampaignAdverts AS campAd ON campAd.CampaignProfileId=camp.CampaignProfileId
												LEFT JOIN Advert AS ad ON ad.AdvertId=campAd.AdvertId
                                                LEFT JOIN 
		                                                (SELECT Id,CampaignProfileId,CurrencyCode FROM Billing WHERE Id in
			                                                (SELECT MAX(Id) FROM Billing GROUP BY CampaignProfileId,CurrencyCode)
		                                                ) AS bill 
                                                ON bill.CampaignProfileId=camp.CampaignProfileId
                                                LEFT JOIN 
		                                            ( SELECT cpi.CampaignProfileId, COUNT(DISTINCT ca.UserProfileId) AS UniqueListenrs
			                                            FROM CampaignAudit AS ca INNER JOIN CampaignProfile AS cpi ON cpi.CampaignProfileId=ca.CampaignProfileId
                                                            WHERE cpi.UserId=@Id
			                                            AND ca.Proceed = 1
			                                            GROUP BY cpi.CampaignProfileId
		                                            ) AS r ON r.CampaignProfileId = camp.CampaignProfileId
                                                LEFT JOIN RollupsCampaign AS ro ON ro.CampaignId=camp.CampaignProfileId
                                                LEFT JOIN Operators AS op ON op.CountryId=camp.CountryId
                                                LEFT JOIN Contacts AS con ON con.UserId=camp.UserId
                                                LEFT JOIN Country AS ctry ON ctry.Id=camp.CountryId 
                                                WHERE camp.Status<>5 AND camp.UserId=@Id ORDER BY camp.CampaignProfileId DESC";
            

            // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

            return await _executers.ExecuteCommand(_connStr,
                            conn => conn.Query<CampaignAdminResult>(selectQuery, new { Id = id }));
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


        public async Task<CampaignProfileDto> GetCampaignProfileDetail(int campaignId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignQuery.GetCampaignProfileById);
            try
            {
                builder.AddParameters(new { Id = campaignId, siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignProfileDto>(select.RawSql, select.Parameters));
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



        public async Task<int> ChangeCampaignProfileStatus(CampaignProfileDto model)
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
        public async Task<int> ChangeCampaignProfileStatusOperator(CampaignProfileDto model)
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


        public async Task<int> UpdateCampaignCredit(CampaignCreditCommand model, string constr)
        {
            var campModel = await GetCampaignCreditDetail(model, _connStr, false);
            string InsertMatchFinancial = @"UPDATE CampaignProfile SET TotalBudget=@TotalBudget,TotalCredit=@TotalCredit,
                                             UpdatedDateTime=GETDATE(), Status=@Status, NextStatus=0, AvailableCredit=@AvailableCredit WHERE CampaignProfileId=@Id";
            int x = 0;

            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(InsertMatchFinancial, new
                             {
                                 Id = model.CampaignProfileId,
                                 Status = (int)Enums.CampaignStatus.Play,
                                 TotalBudget = (campModel.TotalBudget + model.TotalBudget),
                                 TotalCredit = (campModel.TotalCredit + model.TotalCredit),
                                 AvailableCredit = (campModel.AvailableCredit + model.TotalCredit)
                             }));




                if (constr != null && constr.Length > 2)
                {
                    campModel = await GetCampaignCreditDetail(model, constr, true);
                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(InsertMatchFinancial, new
                                    {
                                        Id = campModel.CampaignProfileId,
                                        Status = (int)Enums.CampaignStatus.Play,
                                        TotalBudget = (campModel.TotalBudget + model.TotalBudget),
                                        TotalCredit = (campModel.TotalCredit + model.TotalCredit),
                                        AvailableCredit = (campModel.AvailableCredit + model.TotalCredit)
                                    }));
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        private async Task<CampaignCreditCommand> GetCampaignCreditDetail(CampaignCreditCommand _model, string conn, bool isOperator)
        {
            int campId = 0;
            var newModel = new CampaignCreditCommand();
            var selectSQL = "SELECT CampaignProfileId, TotalCredit, TotalBudget, AvailableCredit FROM CamapaignProfile WHERE CampaignProfileId = @Id";
            try
            {
                if (isOperator)
                    campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(_model.CampaignProfileId, conn);
                else
                    campId = _model.CampaignProfileId;
                newModel = await _executers.ExecuteCommand(conn,
                                conn => conn.QueryFirstOrDefault<CampaignCreditCommand>(selectSQL, new { Id = campId }));
            }
            catch
            {
                throw;
            }
            return newModel;
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


        
        public async Task<object> GetCampaignTableForAdvertiser(int advertiserID)//, ConsolidatedStatsDao consolidatedStats, int campaignId = 0)
        {
            //var campaign = new List<CampaignProfile>();
            //var campaignsQuery = @"SELECT cp.*,a.* FROM CampaignProfile cp LEFT JOIN CampaignAdverts AS ca ON cp.CampaignProfileId=ca.CampaignProfileId
            //                        LEFT JOIN Adverts AS a ON a.AdvertId = ca.AdvertId WHERE cp.UserId=@userId ;";

            //var campAdvQuery = @"SELECT ca.CampaignProfileId,ad.* FROM CampaignAdverts AS a INNER JOIN Advert AS a ON a.AdvertId = ca.AdvertId WHERE CampaignProfileId in @ids ;";

            //var sb = new StringBuilder();
            //var paramToUse = new { userId = advertiserID };
            //sb.Append(campaignsQuery);

            //if (campaignId > 0)
            //    sb.Append(" and cp.CampaignProfileId = @campaignId ");
            //using (var connection = new SqlConnection(_connStr))
            //{
            //    await connection.OpenAsync();
            //    if (campaignId > 0)
            //    {
            //        var camp = await connection.QueryAsync<CampaignProfile>(sb.ToString(), new { userId = advertiserID, campaignId = campaignId });
            //        campaign = camp.ToList();
            //    }
            //    else
            //    {
            //        var camp = await connection.QueryAsync<CampaignProfile>(sb.ToString(), new { userId = advertiserID });
            //        campaign = camp.ToList();
            //    }

            //    int[] Ids = campaign.Select(s => s.CampaignProfileId).ToArray();

            //    var adverts = await connection.QueryAsync<UserAdvertResult>(campAdvQuery, new { ids = new[] { Ids } });

            //    var campaigns = campaign
            //    .GroupJoin(adverts.AsQueryable(), c => c.CampaignProfileId, a => a.CampaignProfileId, (c, a) => new { Campaign = c, Advert = a })
            //    .ToList();

            //    var joined = campaigns.GroupJoin(consolidatedStats.Dashboard, s => s.Campaign.CampaignProfileId, c => c.CampaignId,
            //        (c, s) =>
            //        new
            //        {
            //            Campaign = c.Campaign,
            //            Summary = s.FirstOrDefault() ??
            //                      new DashboardSummariesDao
            //                      {
            //                          Budget = c.Campaign.TotalBudget,
            //                          FundsAvailable = c.Campaign.TotalBudget,
            //                          AvgBid = Convert.ToDecimal(c.Campaign.MaxBid)
            //                      },
            //            Advert = c.Advert.FirstOrDefault(),

            //        }).ToList();
            object bob = new object();
            return bob;

                

                //var adverts = await connection.QueryAsync<UserAdvertResult>(adQuery, new { ids = new[] { campadsId } });

                //campaign.GroupJoin

                //return campaign;

            
        }

        private IQueryable<T> Test<T>(IEnumerable<T> x)
        {
            return x.AsQueryable();
        }

    }
}
