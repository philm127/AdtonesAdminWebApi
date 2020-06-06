using AdtonesAdminWebApi.DAL.Interfaces;
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

        public AdvertDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
        }


        public async Task<IEnumerable<UserAdvertResult>> GetAdvertResultSet(string command, int id=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(command);
            if (id == 0)
                sb.Append(" ORDER BY ad.AdvertId DESC;");
            else
            {
                sb.Append(" WHERE ad.UserId=@userId;");
                builder.AddParameters(new { userId = id });
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


        public async Task<UserAdvertResult> GetAdvertDetail(string command, int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            try
            {
                builder.AddParameters(new { Id = id });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<UserAdvertResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdvertCategoryResult>> GetAdvertCategoryList(string command)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertCategoryResult>(command));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> ChangeAdvertStatus(string command, UserAdvertResult model)
        {

            var sb = new StringBuilder();
            sb.Append(command);
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
        public async Task<int> ChangeAdvertStatusOperator(string command, UserAdvertResult model,int userId)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorId);
            var sb = new StringBuilder();
            sb.Append(command);
            sb.Append("AdvertId=@AdvertId;");

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


        public async Task<FtpDetailsModel> GetFtpDetails(string command, int operatorId)
        {

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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


        public async Task<int> UpdateMediaLoaded(string command, UserAdvertResult advert)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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


        //        //Model.Advert advert = _advertRepository.GetById(command.AdvertId);
        //        var advertDetail = _advertRepository.GetById(command.AdvertId);
        //        advertDetail.Status = command.Status;
        //            advertDetail.UpdatedBy = command.UpdatedBy;
        //            _advertRepository.Update(advertDetail);
        //            var ConnString = ConnectionString.GetConnectionStringByCountryId(advertDetail.CountryId);
        //            if (ConnString != null && ConnString.Count() > 0)
        //            {
        //                foreach (var item in ConnString)
        //                {
        //                    EFMVCDataContex db = new EFMVCDataContex(item);
        //        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UpdatedBy);
        //        var advertData = db.Adverts.Where(s => s.AdtoneServerAdvertId == command.AdvertId).FirstOrDefault();
        //                    if (advertData != null)
        //                    {
        //                        advertData.Status = command.Status;
        //                        if (externalServerUserId != 0)
        //                        {
        //                            advertData.UpdatedBy = command.UpdatedBy;
        //                        }
        //                        else
        //                        {
        //                            advertData.UpdatedBy = null;
        //                        }

        //db.SaveChanges();
        //                    }
        //                }
        //            }
        //            unitOfWork.Commit();
        //            return new CommandResult(true);
        //}



    }
}
