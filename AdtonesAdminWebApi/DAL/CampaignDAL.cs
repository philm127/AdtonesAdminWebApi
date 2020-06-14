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
        private readonly ICampaignQuery _commandText;
        private readonly IHttpContextAccessor _httpAccessor;

        public CampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                            ICampaignQuery commandText, IHttpContextAccessor httpAccessor)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
            _commandText = commandText;
            _httpAccessor = httpAccessor;
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0)
        {
            var roleName = _httpAccessor.GetRoleFromJWT();

            var sb = new StringBuilder();
            var builder = new SqlBuilder();


            if (id == 0)
            {
                sb.Append(_commandText.GetCampaignResultSet);
                sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
            }
            else if (roleName.ToLower().Contains("operator"))
            {
                sb.Append(_commandText.GetCampaignResultSetOperator);
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


        public async Task<IEnumerable<PromotionalCampaignResult>> GetPromoCampaignResultSet()
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.GetPromoCampaignResultSet);
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<PromotionalCampaignResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<CampaignCreditResult>> GetCampaignCreditResultSet()
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.GetCampaignCreditResultSet);
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
            var select = builder.AddTemplate(_commandText.GetCampaignProfileById);
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
            sb.Append(_commandText.UpdateCampaignProfileStatus);
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
            sb.Append(_commandText.UpdateCampaignProfileStatus);
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
            sb.Append(_commandText.GetCampaignAdvertDetailsById);
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

        public async Task<int> UpdatePromotionalCampaignStatus(IdCollectionViewModel model)
        {
            var sb = new StringBuilder();
            sb.Append(_commandText.UpdatePromotionalCampaignStatus);

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.id });
                builder.AddParameters(new { Status = model.status });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }





        ////        //Model.Campaign Campaign = _CampaignRepository.GetById(command.CampaignId);
        ////        var CampaignDetail = _CampaignRepository.GetById(command.CampaignId);
        ////        CampaignDetail.Status = command.Status;
        ////            CampaignDetail.UpdatedBy = command.UpdatedBy;
        ////            _CampaignRepository.Update(CampaignDetail);
        ////            var ConnString = ConnectionString.GetConnectionStringByCountryId(CampaignDetail.CountryId);
        ////            if (ConnString != null && ConnString.Count() > 0)
        ////            {
        ////                foreach (var item in ConnString)
        ////                {
        ////                    EFMVCDataContex db = new EFMVCDataContex(item);
        ////        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UpdatedBy);
        ////        var CampaignData = db.Campaigns.Where(s => s.AdtoneServerCampaignId == command.CampaignId).FirstOrDefault();
        ////                    if (CampaignData != null)
        ////                    {
        ////                        CampaignData.Status = command.Status;
        ////                        if (externalServerUserId != 0)
        ////                        {
        ////                            CampaignData.UpdatedBy = command.UpdatedBy;
        ////                        }
        ////                        else
        ////                        {
        ////                            CampaignData.UpdatedBy = null;
        ////                        }

        ////db.SaveChanges();
        ////                    }
        ////                }
        ////            }
        ////            unitOfWork.Commit();
        ////            return new CommandResult(true);
        //        //}



    }
}
