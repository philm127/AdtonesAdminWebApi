using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
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

        public CampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(string command, int id=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(command);
            if (id == 0)
                sb.Append(" ORDER BY camp.CampaignProfileId DESC;");
            else
            {
                sb.Append(" WHERE camp.UserId=@userId;");
                builder.AddParameters(new { userId = id });
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


        public async Task<IEnumerable<PromotionalCampaignResult>> GetPromoCampaignResultSet(string command)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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


        public async Task<IEnumerable<CampaignCreditResult>> GetCampaignCreditResultSet(string command)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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


        public async Task<CampaignProfile> GetCampaignProfileDetail(string command, int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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


        public async Task<int> ChangeCampaignProfileStatus(string command, CampaignProfile model)
        {

            var sb = new StringBuilder();
            sb.Append(command);
            sb.Append(" CampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = model.Status });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(command));
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
        public async Task<int> ChangeCampaignProfileStatusOperator(string command, CampaignProfile model)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorId);

            var sb = new StringBuilder();
            sb.Append(command);
            sb.Append(" AdtoneServerCampaignProfileId=@Id;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { Id = model.CampaignProfileId });
                builder.AddParameters(new { Status = model.Status });

                return await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(command));
            }
            catch
            {
                throw;
            }
        }


        //        public async Task<IEnumerable<CampaignCategoryResult>> GetCampaignCategoryList(string command)
        //        {
        //            try
        //            {
        //                return await _executers.ExecuteCommand(_connStr,
        //                                    conn => conn.Query<CampaignCategoryResult>(command));
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //        }





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
