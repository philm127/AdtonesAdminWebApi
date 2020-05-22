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


        public AdvertDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


        public async Task<IEnumerable<UserAdvertResult>> GetAdvertResultSet(string command)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            try
            {
                builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:SiteEmailAddress") });

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
            var select = builder.AddTemplate(command);
            try
            {
                builder.AddParameters(new { AdvertId = model.AdvertId });
                builder.AddParameters(new { UpdatedBy = model.UpdatedBy });
                builder.AddParameters(new { AdvertId = model.AdvertId });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(command));
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
