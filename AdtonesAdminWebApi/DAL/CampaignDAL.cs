using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class CampaignDAL : BaseDAL, ICampaignDAL
    {
        public static string getCampaignResultSet => @"SELECT camp.CampaignProfileId,u.AdtoneServerUserId AS UserId,u.Email,CONCAT(u.FirstName,' ',u.LastName) AS UserName,
														'Safaricom' AS OperatorName,'Kenya' AS CountryName,
														camp.ClientId, ISNULL(cl.Name,'-') AS ClientName,CampaignName,camp.CreatedDateTime AS CreatedDate,
														ad.AdvertId, ad.AdvertName,camp.TotalBudget,u.Organisation,
														CASE WHEN bill.Id>0 THEN camp.Status ELSE 8 END AS Status,bill.CurrencyCode,
														ro.Spend AS TotalSpend,ro.FundsAvailable, ro.MoreSixSecPlays AS finaltotalplays,ro.AvgBid AS AvgBidValue,
														con.MobileNumber,u.AdtoneServerUserId,camp.AdtoneServerCampaignProfileId
                                                        ,ad.AdtoneServerAdvertId
														FROM CampaignProfile AS camp LEFT JOIN Users As u ON u.UserId=camp.UserId
														LEFT JOIN Client AS cl ON camp.ClientId=cl.Id
														INNER JOIN CampaignAdverts AS campAd ON campAd.CampaignProfileId=camp.CampaignProfileId
														INNER JOIN Advert AS ad ON ad.AdvertId=campAd.AdvertId
														LEFT JOIN 
																(SELECT Id,CampaignProfileId,CurrencyCode FROM Billing WHERE Id in
																	(SELECT MAX(Id) FROM Billing GROUP BY CampaignProfileId,CurrencyCode)
																) AS bill 
														ON bill.CampaignProfileId=camp.CampaignProfileId
														LEFT JOIN RollupsCampaign AS ro ON ro.CampaignId=camp.CampaignProfileId AND ro.DetailLevel= 'C'
														INNER JOIN Contacts AS con ON con.UserId=camp.UserId ";

        public static string getCampaignResultSetForProfile = @"SELECT camp.CampaignProfileId,op.OperatorName
                                                ,CampaignName,camp.CreatedDateTime AS CreatedDate
                                                ,camp.IsAdminApproval,ad.AdvertId, ad.AdvertName,
                                                camp.Status AS Status,
                                                ro.MoreSixSecPlays AS finaltotalplays
                                                FROM CampaignProfile AS camp
                                                LEFT JOIN CampaignAdverts AS campAd ON campAd.CampaignProfileId=camp.CampaignProfileId
												LEFT JOIN Advert AS ad ON ad.AdvertId=campAd.AdvertId
                                                LEFT JOIN RollupsCampaign AS ro ON ro.CampaignId=camp.CampaignProfileId AND ro.DetailLevel= 'C'
												LEFT JOIN CampaignProfileExt AS ext ON ext.CampaignProfileId=camp.CampaignProfileId
												LEFT JOIN Operators AS op ON op.CountryId=camp.CountryId
                                                LEFT JOIN Contacts AS con ON con.UserId=camp.UserId
                                                LEFT JOIN Country AS ctry ON ctry.Id=camp.CountryId ";

        public CampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                            IHttpContextAccessor httpAccessor) : base(configuration, executers, connService, httpAccessor)
        {
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetAdminOpAdminCampaignResultSet()
        {

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(getCampaignResultSet);
            
                // had to put this here as the check file picks up WHERE on inner queries
                // sb.Append(" WHERE 1=1 ");

            sb.Append(" WHERE camp.CountryId=10 "); // Hard coded for now

            if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
            {
                sb.Clear();
                sb.Append(getCampaignResultSetForProfile);
                sb.Append(" WHERE camp.UserId=@UserId ");
                builder.AddParameters(new { UserId = _httpAccessor.GetUserIdFromJWT() });
            }

            //var values = CheckGeneralFile(sb, builder,pais:"camp",ops:"op",advs:"camp");
            //sb = values.Item1;
            //builder = values.Item2;

            sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
            var select = builder.AddTemplate(sb.ToString());

            var str = select.RawSql;
            
            try
            {
                // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignAdminResult>(select.RawSql, select.Parameters, commandTimeout: 120));
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


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetBySalesExec(IEnumerable<int> numbers)
        {

            var sb = new StringBuilder(getCampaignResultSet);
            sb.Append(" WHERE camp.CountryId=10 "); // Hard coded for now
            var parameters = new DynamicParameters();
            if (numbers != null)
            {
                sb.Append(" AND u.AdtoneServerUserId IN @nos ");
                parameters.Add("nos", numbers);
            }
            //else
            //{
            //    sb.Append(" WHERE 1=1 ");
            //}
            //var values = CheckGeneralFile(sb, builder, pais: "con", ops: "op", advs: "camp");
            //sb = values.Item1;
            //builder = values.Item2;

            sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
            // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

            return await _executers.ExecuteCommand(_connStr,
                            conn => conn.Query<CampaignAdminResult>(sb.ToString(), parameters, commandTimeout: 120));
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetByAdvertiser(int id)
        {

            string selectQuery = @"SELECT
                                        camp.CampaignProfileId,
                                        u.AdtoneServerUserId AS UserId,
                                        camp.ClientId,
                                        ISNULL(cl.Name, '-') AS ClientName,
                                        CampaignName,
                                        camp.CreatedDateTime AS CreatedDate,
                                        ad.AdvertName,
                                        camp.TotalBudget,
                                        ISNULL(r.UniqueListeners, 0) AS Reach,
                                        camp.Status,
                                        ro.Spend AS TotalSpend,
                                        ro.FundsAvailable,
                                        ro.MoreSixSecPlays AS finaltotalplays,
                                        ro.AvgBid AS AvgBidValue,
                                        camp.CurrencyCode
                                    FROM
                                        CampaignProfile AS camp
                                    INNER JOIN
                                        Users AS u ON u.UserId = camp.UserId
                                    LEFT JOIN
                                        Client AS cl ON camp.ClientId = cl.Id
                                    INNER JOIN
                                        CampaignAdverts AS campAd ON campAd.CampaignProfileId = camp.CampaignProfileId
                                    INNER JOIN
                                        Advert AS ad ON ad.AdvertId = campAd.AdvertId
                                    LEFT JOIN
                                        (
                                            SELECT
                                                cpi.CampaignProfileId,
                                                COUNT(DISTINCT ca.UserProfileId) AS UniqueListeners
                                            FROM
                                                CampaignAudit AS ca
                                            INNER JOIN
                                                CampaignProfile AS cpi ON cpi.CampaignProfileId = ca.CampaignProfileId
                                            WHERE
                                                cpi.UserId = @Id
                                                AND ca.Proceed = 1
                                                AND cpi.CountryId = 10
                                            GROUP BY
                                                cpi.CampaignProfileId
                                        ) AS r ON r.CampaignProfileId = camp.CampaignProfileId
                                    LEFT JOIN
                                        (
                                            SELECT
                                                CampaignProfileId,
                                                CurrencyCode
                                            FROM
                                                Billing
                                            WHERE
                                                Id IN (SELECT MAX(Id) FROM Billing GROUP BY CampaignProfileId, CurrencyCode)
                                        ) AS bill ON bill.CampaignProfileId = camp.CampaignProfileId
                                    LEFT JOIN
                                        RollupsCampaign AS ro ON ro.CampaignId = camp.CampaignProfileId AND ro.DetailLevel = 'C'
                                    WHERE
                                        camp.Status <> 5
                                        AND camp.UserId = @Id
                                        AND camp.CountryId = 10
                                    ORDER BY
                                        camp.CampaignProfileId DESC;";
            

            // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

            return await _executers.ExecuteCommand(_connStr,
                            conn => conn.Query<CampaignAdminResult>(selectQuery, new { Id = id }, commandTimeout: 600));
        }


        public async Task<CampaignAdminResult> GetCampaignResultSetById(int id)
        {

            var sb = new StringBuilder();
            var builder = new SqlBuilder();

                sb.Append(getCampaignResultSet);
                sb.Append(" WHERE camp.CampaignProfileId=@Id ");
                builder.AddParameters(new { Id = id });


            var select = builder.AddTemplate(sb.ToString());

            try
            {
                // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:SiteEmailAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignAdminResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignProfileDto> GetCampaignProfileDetail(int campaignId )
        {

            string getCampaignProfileById = @"SELECT camp.CampaignProfileId,u.AdtoneServerUserId AS UserId,camp.ClientId,CampaignName,CampaignDescription,
                                                    TotalBudget,MaxBid,MaxHourlyBudget,MaxDailyBudget,MaxWeeklyBudget,MaxMonthBudget,
													TotalCredit,SpendToDate,AvailableCredit,PlaysToDate,
                                                    CancelledToDate,SmsToDate,EmailToDate,
													CASE WHEN EmailFileLocation IS NULL THEN EmailFileLocation 
														ELSE CONCAT(@siteAddress,EmailFileLocation) END AS EmailFileLocation,
													CASE WHEN SMSFileLocation IS NULL THEN SMSFileLocation 
														ELSE CONCAT(@siteAddress,SMSFileLocation) END AS SMSFileLocation,
													camp.Active,NumberOfPlays,ad.OperatorId,
                                                    AverageDailyPlays,SmsRequests,EmailsDelievered,EmailSubject,EmailBody,SmsOriginator,SmsBody,
                                                    camp.CreatedDateTime,camp.UpdatedDateTime,camp.Status,StartDate,EndDate,
                                                    camp.CountryId,camp.IsAdminApproval,ProvidendSpendAmount,AdtoneServerCampaignProfileId,
                                                    camp.CurrencyCode,cur.CurrencyId,CampaignCategoryId, ISNULL(min.MinBid,0) AS MinBid
                                                    FROM CampaignProfile AS camp
                                                    INNER JOIN Users AS u ON u.UserId=camp.UserId
													LEFT JOIN CampaignAdverts AS ca ON ca.CampaignProfileId=camp.CampaignProfileId
													LEFT JOIN Advert AS ad ON ad.AdvertId=ca.AdvertId
                                                    INNER JOIN Currencies AS cur ON cur.CurrencyCode=camp.CurrencyCode
													LEFT JOIN CampaignProfileExt as ext ON ext.CampaignProfileId=camp.CampaignProfileId
													LEFT JOIN CountryMinBid AS min ON min.CountryId=camp.CountryId
                                                    WHERE camp.CampaignProfileId=@Id";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignProfileDto>(getCampaignProfileById,
                                                                    new
                                                                    {
                                                                        Id = campaignId,
                                                                        siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress")
                                                                    }));
            }
            catch (Exception ex)
            {
                var msg = ex.Message.ToString();
                throw;
            }
        }


        public async Task<int> InsertCampaignCategory(CampaignCategoryResult model)
        {
            int x = 0;
            int y = 0;
            string addCampaignCategory = @"INSERT INTO CampaignCategory(CategoryName,Description,Active,CountryId,AdtoneServerCampaignCategoryId)
                                                        VALUES(@name,@description,@active,@CountryId,@Id);
                                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                model.AdtoneServerCampaignCategoryId = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(addCampaignCategory, new
                                    {
                                        description = model.Description,
                                        name = model.CategoryName,
                                        Id = model.AdtoneServerCampaignCategoryId,
                                        active = 1,
                                        CountryId = model.CountryId
                                    }));

                var lst = await _connService.GetConnectionStringsByCountryId(model.CountryId.GetValueOrDefault());
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    if (constr != null && constr.Length > 10)
                    {

                        y += await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(addCampaignCategory, new
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



        public async Task<int> ChangeCampaignProfileStatus(int campaignProfileId, int status)
        {
            string updateCampaignProfileStatus = @"UPDATE CampaignProfile SET Status=@Status,IsAdminApproval=1,
                                                        UpdatedDateTime = GETDATE() WHERE ";

            var sb = new StringBuilder();
            sb.Append(updateCampaignProfileStatus);
            sb.Append(" CampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = campaignProfileId });
                builder.AddParameters(new { Status = status });

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
        public async Task<int> ChangeCampaignProfileStatusOperator(int campaignProfileId, int status, int operatorId)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(operatorId);

            string updateCampaignProfileStatus = @"UPDATE CampaignProfile SET Status=@Status,IsAdminApproval=1,
                                                        UpdatedDateTime = GETDATE() WHERE ";

            var sb = new StringBuilder();
            sb.Append(updateCampaignProfileStatus);
            sb.Append(" AdtoneServerCampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = campaignProfileId });
                builder.AddParameters(new { Status = status });


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
            string getCampaignAdvertDetailsById = @"SELECT CampaignAdvertId,CampaignProfileId,AdvertId,
                                                        NextStatus,AdtoneServerCampaignAdvertId
                                                        FROM CampaignAdverts WHERE ";
            var id = adId > 0 ? adId : campId;
            var sb = new StringBuilder();

            sb.Append(getCampaignAdvertDetailsById);
            if (adId > 0)
                sb.Append(" AdvertId=@Id;");
            
            else
                sb.Append(" CampaignProfileId=@Id;");
            
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<CampaignAdverts>(sb.ToString(), new { Id = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckCampaignBillingExists(int campaignId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate("SELECT COUNT(1) FROM Billing WHERE CampaignProfileId=@Id");
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


        public async Task<int> UpdateCampaignCredit(CampaignCreditCommand model, List<string> conStrList)
        {
            string updateCampaign = @"UPDATE CampaignProfile SET TotalBudget=@TotalBudget,TotalCredit=@TotalCredit,
                                             UpdatedDateTime=GETDATE(), Status=@Status, NextStatus=0, AvailableCredit=@AvailableCredit WHERE CampaignProfileId=@Id";
            int x = 0;

            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(updateCampaign, new
                             {
                                 Id = model.CampaignProfileId,
                                 Status = model.Status,
                                 TotalBudget = model.TotalBudget,
                                 TotalCredit = model.TotalCredit,
                                 AvailableCredit = model.AvailableCredit
                             }));




                if (conStrList != null && conStrList.Count > 0)
                {
                    foreach (var constr in conStrList)
                    {
                        var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                        x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(updateCampaign, new
                                        {
                                            Id = campId,
                                            Status = model.Status,
                                            TotalBudget = model.TotalBudget,
                                            TotalCredit = model.TotalCredit,
                                            AvailableCredit = model.AvailableCredit
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

        public async Task<bool> CheckCampaignNameExists(string campaignName,int userId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate("SELECT COUNT(1) FROM CampaignProfile WHERE LOWER(CampaignName)=@Id AND UserId=@UserId");
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
