using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AdvertDAL : BaseDAL, IAdvertDAL
    {

        public AdvertDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<UserAdvertResult>> GetAdvertResultSet(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(AdvertQuery.GetAdvertResultSet);

            if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
            {
                sb.Append(" WHERE ad.UserId=@UserId ");
                builder.AddParameters(new { UserId = _httpAccessor.GetUserIdFromJWT() });
            }
            else if (id > 0)
            {
                sb.Append(" WHERE ad.UserId=@UserId AND ad.Status=4 ");
                builder.AddParameters(new { UserId = id });
            }
            var values = CheckGeneralFile(sb, builder, pais: "ad", ops: "ad", advs: "ad");
            sb = values.Item1;
            builder = values.Item2;

            sb.Append(" ORDER BY ad.CreatedDateTime DESC, ad.Status DESC;");

            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<UserAdvertResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<UserAdvertResult>> GetAdvertForSalesResultSet(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(AdvertQuery.GetAdvertSalesExecResultSet);

            if (id > 0)
            {
                sb.Append(" WHERE sales.IsActive=1 ");
                sb.Append(" AND sales.SalesExecId=@Sid ");
                builder.AddParameters(new { Sid = id });
            }
            var values = CheckGeneralFile(sb, builder, pais: "ad", ops: "ad", advs: "ad");
            sb = values.Item1;
            builder = values.Item2;

            sb.Append(" ORDER BY ad.CreatedDateTime DESC, ad.Status DESC;");

            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<UserAdvertResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdvertTableAdvertiserDto>> GetAdvertForAdvertiserResultSet(int id)
        {
            string GetAdvertSalesExecResultSet = @"SELECT ad.AdvertId,ad.UserId,ad.ClientId,ad.AdvertName,ad.Brand,
                                                ISNULL(cl.Name,'-') AS ClientName,ad.OperatorId,cad.CampaignProfileId,ad.UpdatedBy,
                                                ad.CreatedDateTime AS CreatedDate,cprof.CampaignName,
                                                ad.Script,ad.Status,ad.MediaFileLocation,cprof.CountryId,
                                                CASE WHEN ad.MediaFileLocation IS NULL THEN ad.MediaFileLocation 
                                                    ELSE CONCAT(@siteAddress,ad.MediaFileLocation) END AS MediaFile,
                                                ro.MoreSixSecPlays AS TotalPlays,CAST(ro.AvgBid AS NUMERIC(36,2)) AS AverageBid
                                                FROM Advert AS ad LEFT JOIN Client AS cl ON ad.ClientId=cl.Id
                                                LEFT JOIN Users AS usr ON usr.UserId=ad.UserId
                                                LEFT JOIN CampaignAdverts AS cad ON cad.AdvertId=ad.AdvertId
                                                LEFT JOIN CampaignProfile AS cprof ON cprof.CampaignProfileId=cad.CampaignProfileId
                                                LEFT JOIN RollupsCampaign AS ro ON ro.CampaignId=cprof.CampaignProfileId
                                                WHERE ad.UserId = @Id ORDER BY ad.CreatedDateTime DESC, ad.Status DESC;";
            
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<AdvertTableAdvertiserDto>(GetAdvertSalesExecResultSet, new { Id = id, 
                                                                                                    siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") }));
            }
            catch
            {
                throw;
            }
        }

        public async Task<AdvertiserAdvertTableDashboardDto> GetAdvertForAdvertiserDashboard(int id)
        {
            string GetAdvertSalesExecResultSet = @"select SUM(TotalSMS) AS TotalSMS,SUM(TotalEmail) AS TotalEmail,
                                                    SUM(TotalPlays) AS TotalPlays,
                                                    CAST((AVG(AvgPlayLength)/1000) AS NUMERIC(36,2)) AS AvgPlayLength
                                                    from RollupsCampaign where DetailLevel='C' and AdvertiserId=@Id;";

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<AdvertiserAdvertTableDashboardDto>(GetAdvertSalesExecResultSet, new
                                { Id = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckAdvertNameExists(string advertName, int userId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.CheckAdvertNameExists);
            builder.AddParameters(new { Id = advertName.ToLower() });
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


        /// <summary>
        /// Gets a single Advert as Result list when clicked through from Campaign Page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserAdvertResult>> GetAdvertResultSetById(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(AdvertQuery.GetAdvertResultSet);

            if (id > 0)
            {
                sb.Append(" WHERE ad.AdvertId=@Id ");
                builder.AddParameters(new { Id = id });
            }


            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<UserAdvertResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<UserAdvertResult> GetAdvertDetail(int id = 0)
        {
            var sb = new StringBuilder();
            sb.Append(AdvertQuery.GetAdvertResultSet);
            sb.Append(" WHERE ad.AdvertId=@Id");
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = id });
                builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<UserAdvertResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<NewAdvertFormModel> GetAdvertForUpdateModel(int id)
        {
            string selectQuery = @"SELECT AdvertId, UserId AS AdvertiserId,ClientId,AdvertName,Brand,MediaFileLocation,
                                    UploadedToMediaServer,CreatedDateTime,UpdatedDateTime,Status,Script,ScriptFileLocation,
                                    IsAdminApproval,AdvertCategoryId,CountryId,PhoneticAlphabet,NextStatus,CampProfileId,
                                    AdtoneServerAdvertId,UpdatedBy,OperatorId FROM Advert
                                    WHERE AdvertId=@Id;";
            return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<NewAdvertFormModel>(selectQuery, new { Id = id }));
        }

        public async Task<NewAdvertFormModel> CreateNewAdvert(NewAdvertFormModel model)
        {
            string InsertNewCampaignAdvert = @"INSERT INTO Advert(UserId,ClientId,AdvertName,Brand,MediaFileLocation,
                                                    UploadedToMediaServer,CreatedDateTime,UpdatedDateTime,Status,Script,ScriptFileLocation,
                                                    IsAdminApproval,AdvertCategoryId,CountryId,PhoneticAlphabet,NextStatus,CampProfileId,
                                                    AdtoneServerAdvertId,UpdatedBy,OperatorId)
                                                VALUES(@AdvertiserId,@ClientId,@AdvertName,@Brand,@MediaFileLocation,
                                                    @UploadedToMediaServer,GETDATE(),GETDATE(),@Status,@Script,@ScriptFileLocation,
                                                    @IsAdminApproval,@AdvertCategoryId,@CountryId,@PhoneticAlphabet,@NextStatus,@CampaignProfileId,
                                                    @AdtoneServerAdvertId,@UpdatedBy,@OperatorId);
                                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {

                model.AdtoneServerAdvertId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(InsertNewCampaignAdvert, model));

                try
                {
                    return await CreateNewOperatorAdvert(model);
                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<NewAdvertFormModel> CreateNewOperatorAdvert(NewAdvertFormModel model)
        {
            string InsertNewCampaignAdvert = @"INSERT INTO Advert(UserId,ClientId,AdvertName,Brand,MediaFileLocation,
                                                            UploadedToMediaServer,CreatedDateTime,UpdatedDateTime,Status,Script,ScriptFileLocation,
                                                            IsAdminApproval,AdvertCategoryId,CountryId,PhoneticAlphabet,NextStatus,CampProfileId,
                                                            AdtoneServerAdvertId,UpdatedBy,OperatorId)
                                                          VALUES(@AdvertiserId,@ClientId,@AdvertName,@Brand,@MediaFileLocation,
                                                            @UploadedToMediaServer,GETDATE(),GETDATE(),@Status,@Script,@ScriptFileLocation,
                                                            @IsAdminApproval,@AdvertCategoryId,@CountryId,@PhoneticAlphabet,@NextStatus,@CampaignProfileId,
                                                            @AdtoneServerAdvertId,@UpdatedBy,@OperatorId);
                                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var newModel = new NewAdvertFormModel();
            newModel.AdtoneServerAdvertId = model.AdtoneServerAdvertId;
            newModel.AdvertCategoryId = model.AdvertCategoryId;
            newModel.AdvertId = model.AdvertId;
            newModel.AdvertiserId = model.AdvertiserId;
            newModel.AdvertName = model.AdvertName;
            newModel.Brand = model.Brand;
            newModel.CampaignProfileId = model.CampaignProfileId;
            newModel.ClientId = model.ClientId;
            newModel.CountryId = model.CountryId;
            newModel.file = model.file;
            newModel.IsAdminApproval = model.IsAdminApproval;
            newModel.MediaFile = model.MediaFile;
            newModel.MediaFileLocation = model.MediaFileLocation;
            newModel.NextStatus = model.NextStatus;
            newModel.Numberofadsinabatch = model.Numberofadsinabatch;
            newModel.OperatorId = model.OperatorId;
            newModel.PhoneticAlphabet = model.PhoneticAlphabet;
            newModel.ScriptFile = model.ScriptFile;
            newModel.ScriptFileLocation = model.ScriptFileLocation;
            newModel.Script = model.Script;
            newModel.Status = model.Status;
            newModel.UpdatedBy = model.UpdatedBy;
            newModel.UploadedToMediaServer = model.UploadedToMediaServer;

            try
            {

                    var conn = await _connService.GetConnectionStringByOperator(model.OperatorId);
                    if (conn != null && conn.Length > 10)
                    {

                        newModel.AdvertiserId = await _connService.GetUserIdFromAdtoneIdByConnString(model.AdvertiserId, conn);
                        newModel.UpdatedBy = await _connService.GetUserIdFromAdtoneIdByConnString(model.UpdatedBy, conn);
                        newModel.OperatorId = await _connService.GetOperatorIdFromAdtoneId(model.OperatorId);
                        newModel.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(model.CampaignProfileId, conn);
                        if (model.ClientId != null)
                            newModel.ClientId = await _connService.GetClientIdFromAdtoneIdByConnString(model.ClientId.Value, conn);


                        newModel.AdvertCategoryId = await _executers.ExecuteCommand(conn,
                                conn => conn.ExecuteScalar<int>("SELECT AdvertCategoryId FROM AdvertCategories WHERE AdtoneServerAdvertCategoryId=@Id", new { Id = model.AdvertCategoryId }));

                        newModel.AdvertId = await _executers.ExecuteCommand(conn,
                                conn => conn.ExecuteScalar<int>(InsertNewCampaignAdvert, newModel));
                    }
            }
            catch
            {
                throw;
            }

            return newModel;
        }


        public async Task<int> UpdateAdvert(NewAdvertFormModel model)
        {
            var retVal = 0;
            var sb = new StringBuilder();
            string updateMainAdvert = @"UPDATE Advert SET ClientId=@ClientId,AdvertName=@AdvertName,Brand=@Brand,UpdatedDateTime=GETDATE(),
                                        Script=@Script,AdvertCategoryId=@AdvertCategoryId,UpdatedBy=@UpdatedBy";
            sb.Append(updateMainAdvert);
            if (model.FileUpdate)
                sb.Append(@",MediaFileLocation=@MediaFileLocation,UploadedToMediaServer=@UploadedToMediaServer, Status=@Status,
                            IsAdminApproval=@IsAdminApproval,NextStatus=@NextStatus ");
            sb.Append(" WHERE AdvertId=@AdvertId");

            try
            {

                var uid = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), model));
            }
            catch
            {
                throw;
            }

            var conn = await _connService.GetConnectionStringByOperator(model.OperatorId);
            if (conn != null && conn.Length > 10)
            {
                var newModel = new NewAdvertFormModel();
                newModel.AdtoneServerAdvertId = model.AdvertId;
                newModel.AdvertName = model.AdvertName;
                newModel.Brand = model.Brand;
                newModel.IsAdminApproval = model.IsAdminApproval;
                newModel.MediaFile = model.MediaFile;
                newModel.MediaFileLocation = model.MediaFileLocation;
                newModel.NextStatus = model.NextStatus;
                newModel.PhoneticAlphabet = model.PhoneticAlphabet;
                newModel.ScriptFile = model.ScriptFile;
                newModel.ScriptFileLocation = model.ScriptFileLocation;
                newModel.Script = model.Script;
                newModel.Status = model.Status;
                newModel.UploadedToMediaServer = model.UploadedToMediaServer;
                newModel.AdvertId = await _executers.ExecuteCommand(conn,
                                conn => conn.ExecuteScalar<int>("SELECT AdvertId FROM Advert WHERE AdtoneServerAdvertId=@Id", new { Id = model.AdvertId }));
                newModel.AdvertiserId = await _connService.GetUserIdFromAdtoneIdByConnString(model.AdvertiserId, conn);
                newModel.UpdatedBy = await _connService.GetUserIdFromAdtoneIdByConnString(model.UpdatedBy, conn);
                newModel.OperatorId = await _connService.GetOperatorIdFromAdtoneId(model.OperatorId);
                newModel.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(model.CampaignProfileId, conn);
                if (model.ClientId != null)
                    newModel.ClientId = await _connService.GetClientIdFromAdtoneIdByConnString(model.ClientId.Value, conn);

                newModel.AdvertCategoryId = await _executers.ExecuteCommand(conn,
                        conn => conn.ExecuteScalar<int>("SELECT AdvertCategoryId FROM AdvertCategories WHERE AdtoneServerAdvertCategoryId=@Id", new { Id = model.AdvertCategoryId }));

                try
                {
                    return await _executers.ExecuteCommand(conn,
                        conn => conn.ExecuteScalar<int>(sb.ToString(), newModel));

                }
                catch
                {
                    throw;
                }

            }
            return retVal;
        }



        public async Task<int> ChangeAdvertStatus(UserAdvertResult model)
        {

            var sb = new StringBuilder();
            sb.Append(AdvertQuery.UpdateAdvertStatus);
            sb.Append("AdvertId=@AdvertId;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { AdvertId = model.AdvertId });
                builder.AddParameters(new { UpdatedBy = model.UpdatedBy });
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
        /// Changes status on Operators Provisioning Server
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <param name="userId">UserId obtained from operator provisioning server using AdtonesDServerUserId</param>
        /// <returns></returns>
        public async Task<int> ChangeAdvertStatusOperator(UserAdvertResult model, int userId, int adId)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(model.OperatorId);
            var sb = new StringBuilder();
            sb.Append(AdvertQuery.UpdateAdvertStatus);
            sb.Append(" AdvertId=@AdvertId;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { AdvertId = adId });
                builder.AddParameters(new { UpdatedBy = userId });
                builder.AddParameters(new { Status = model.Status });

                return await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<FtpDetailsModel> GetFtpDetails(int operatorId)
        {

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.GetFtpDetails);
            try
            {
                builder.AddParameters(new { OperatorId = operatorId });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<FtpDetailsModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateMediaLoaded(UserAdvertResult advert)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.UpdateMediaLoaded);
            try
            {
                builder.AddParameters(new { advertId = advert.AdvertId });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> RejectAdvertReason(UserAdvertResult model)
        {

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.RejectAdvertReason);
            try
            {
                builder.AddParameters(new { AdvertId = model.AdvertId });
                builder.AddParameters(new { UserId = model.UpdatedBy });
                builder.AddParameters(new { RejectionReason = model.RejectionReason });
                builder.AddParameters(new { AdtoneServerAdvertRejectionId = 0 });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> RejectAdvertReasonOperator(UserAdvertResult model, string connString, int uid, int rejId, int adId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.RejectAdvertReason);
            try
            {
                builder.AddParameters(new { AdvertId = adId });
                builder.AddParameters(new { UserId = uid });
                builder.AddParameters(new { RejectionReason = model.RejectionReason });
                builder.AddParameters(new { AdtoneServerAdvertRejectionId = rejId });

                return await _executers.ExecuteCommand(connString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> DeleteAdvertRejection(UserAdvertResult model)
        {

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.DeleteRejectAdvertReason);
            try
            {
                builder.AddParameters(new { AdvertId = model.AdvertId });
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> DeleteRejectAdvertReasonOperator(string connString, int adId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.DeleteRejectAdvertReason);
            try
            {
                builder.AddParameters(new { AdvertId = adId });

                return await _executers.ExecuteCommand(connString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateAdvertForBilling(int advertId, string constr)
        {
            int adId = 0;
            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(AdvertQuery.UpdateAdvertFromBilling, new { Id = advertId }));


                adId = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>("SELECT AdvertId FROM Advert WHERE AdtoneServerAdvertId=@Id", new { Id = advertId }));

                return await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(AdvertQuery.UpdateAdvertFromBilling, new { Id = adId }));
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetAdvertIdByCampid(int campaignId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(AdvertQuery.GetAdvertIdByCampid, new { Id = campaignId }));
            }
            catch
            {
                throw;
            }

        }
    }
}
