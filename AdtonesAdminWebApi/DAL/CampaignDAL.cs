using AdtonesAdminWebApi.DAL.Interfaces;
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


        public CampaignDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(string command)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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


        //        public async Task<UserCampaignResult> GetCampaignDetail(string command, int id = 0)
        //        {
        //            var builder = new SqlBuilder();
        //            var select = builder.AddTemplate(command);
        //            try
        //            {
        //                builder.AddParameters(new { Id = id });

        //                return await _executers.ExecuteCommand(_connStr,
        //                                conn => conn.QueryFirstOrDefault<UserCampaignResult>(select.RawSql, select.Parameters));
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //        }


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


        //        public async Task<int> ChangeCampaignStatus(string command, UserCampaignResult model)
        //        {

        //            var sb = new StringBuilder();
        //            sb.Append(command);
        //            sb.Append("CampaignId=@CampaignId;");

        //            var builder = new SqlBuilder();
        //            var select = builder.AddTemplate(command);
        //            try
        //            {
        //                builder.AddParameters(new { CampaignId = model.CampaignId });
        //                builder.AddParameters(new { UpdatedBy = model.UpdatedBy });
        //                builder.AddParameters(new { CampaignId = model.CampaignId });

        //                return await _executers.ExecuteCommand(_connStr,
        //                                    conn => conn.ExecuteScalar<int>(command));
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
