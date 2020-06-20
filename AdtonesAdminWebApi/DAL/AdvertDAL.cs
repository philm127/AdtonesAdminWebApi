using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AdvertDAL : IAdvertDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;
        private readonly IAdvertQuery _commandText;

        public AdvertDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                            IAdvertQuery commandText)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
            _commandText = commandText;
        }


        public async Task<IEnumerable<UserAdvertResult>> GetAdvertResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(_commandText.GetAdvertResultSet);
            sb.Append(" ORDER BY ad.AdvertId DESC;");

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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.GetAdvertDetail);
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


        public async Task<IEnumerable<AdvertCategoryResult>> GetAdvertCategoryList()
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertCategoryResult>(_commandText.GetAdvertCategoryDataTable));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> ChangeAdvertStatus(UserAdvertResult model)
        {

            var sb = new StringBuilder();
            sb.Append(_commandText.UpdateAdvertStatus);
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
        public async Task<int> ChangeAdvertStatusOperator(UserAdvertResult model,int userId)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorId);
            var sb = new StringBuilder();
            sb.Append(_commandText.UpdateAdvertStatus);
            sb.Append(" AdtoneServerAdvertId=@AdvertId;");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                builder.AddParameters(new { AdvertId = model.AdvertId });
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
            var select = builder.AddTemplate(_commandText.GetFtpDetails);
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
            var select = builder.AddTemplate(_commandText.UpdateMediaLoaded);
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
            var select = builder.AddTemplate(_commandText.RejectAdvertReason);
            try
            {
                builder.AddParameters(new { AdvertId = model.AdvertId });
                builder.AddParameters(new { UserId = model.UpdatedBy });
                builder.AddParameters(new { RejectReason = model.RejectionReason });
                builder.AddParameters(new { AdtoneServerAdvertRejectionI = 0});

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> RejectAdvertReasonOperator(UserAdvertResult model,string connString,int uid, int rejId, int adId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.RejectAdvertReason);
            try
            {
                builder.AddParameters(new { AdvertId = adId });
                builder.AddParameters(new { UserId = uid });
                builder.AddParameters(new { RejectReason = model.RejectionReason });
                builder.AddParameters(new { AdtoneServerAdvertRejectionI = rejId });

                return await _executers.ExecuteCommand(connString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


    }
}
