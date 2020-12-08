using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
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

            if (id > 0)
            {
                sb.Append(" WHERE ad.UserId=@UserId AND ad.Status=4 ");
                builder.AddParameters(new { UserId = id });
            }
                var values = CheckGeneralFile(sb, builder, pais:"ad",ops:"ad",advs:"ad");
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


        public async Task<IEnumerable<AdvertCategoryResult>> GetAdvertCategoryList()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(AdvertQuery.GetAdvertCategoryDataTable);

            var values = CheckGeneralFile(sb, builder, pais:"ad", ops:"op");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertCategoryResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> RemoveAdvertCategory(IdCollectionViewModel model)
        {
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(AdvertQuery.DeleteAdvertCategory + " AdvertCategoryId=@Id;", 
                                                                                                                    new { Id = model.id}));

                var lst = await _connService.GetConnectionStringsByCountry(model.countryId);
                List<string> conns = lst.ToList();

                foreach(string constr in conns)
                {
                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(AdvertQuery.DeleteAdvertCategory + " AdtoneServerAdvertCategoryId=@Id;", 
                                                                                                                     new { Id = model.id }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateAdvertCategory(AdvertCategoryResult model)
        {
            var sb = new StringBuilder();
            var sb1 = new StringBuilder();
            int x = 0;
            try
            {
                sb.Append(AdvertQuery.UpdateAdvertCategory);
                sb.Append(" AdvertCategoryId=@Id;");
                x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(sb.ToString(),new { Id = model.AdvertCategoryId,
                                                                                    countryId = model.CountryId,
                                                                                    name = model.CategoryName}));

                var lst = await _connService.GetConnectionStringsByCountry(model.CountryId.GetValueOrDefault());
                List<string> conns = lst.ToList();
                sb1.Append(AdvertQuery.UpdateAdvertCategory);
                sb1.Append(" AdtoneServerAdvertCategoryId=@Id;");
                foreach (string constr in conns)
                {

                    var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId.GetValueOrDefault(), constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(sb1.ToString(), new { Id = model.AdvertCategoryId,
                                                                                            countryId = countryId,
                                                                                            name = model.CategoryName
                                                                                        }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<AdvertCategoryResult> GetAdvertCategoryDetails(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.GetAdvertCategoryDataTable + " WHERE AdvertCategoryId=@Id");
            try
            {
                builder.AddParameters(new { Id = id });


                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<AdvertCategoryResult>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertAdvertCategory(AdvertCategoryResult model)
        {
            int x = 0;
            int y = 0;
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.AddAdvertCategory);
            try
            {
                builder.AddParameters(new { countryId = model.CountryId });
                builder.AddParameters(new { name = model.CategoryName });
                builder.AddParameters(new { Id = model.AdtoneServerAdvertCategoryId });


                model.AdtoneServerAdvertCategoryId = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));


                var builder2 = new SqlBuilder();
                var select2 = builder2.AddTemplate(AdvertQuery.AddAdvertCategory);

                    builder2.AddParameters(new { name = model.CategoryName });
                    builder2.AddParameters(new { Id = model.AdtoneServerAdvertCategoryId });


                    var lst = await _connService.GetConnectionStringsByCountry(model.CountryId.GetValueOrDefault());
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId.GetValueOrDefault(), constr);

                        builder2.AddParameters(new { countryId = countryId });

                        y += await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(select2.RawSql, select2.Parameters));
                    }

            }
            catch
            {
                throw;
            }
            return x;
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
        public async Task<int> ChangeAdvertStatusOperator(UserAdvertResult model,int userId, int adId)
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
                builder.AddParameters(new { AdtoneServerAdvertRejectionId = 0});

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


        public async Task<int> UpdateAdvertForBilling(int advertId, int operatorId)
        {
            int adId = 0;
            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(AdvertQuery.UpdateAdvertFromBilling, new { Id = advertId }));

                var constr = await _connService.GetConnectionStringByOperator(operatorId);

                adId = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>("SELECT AdvertId FROM Advert WHERE AdtoneServerAdvertId=@Id", new { Id = advertId }));

                return await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(AdvertQuery.UpdateAdvertFromBilling, new { Id = adId }));
            }
            catch
            {
                throw;
            }

            return adId;
        }

    }
}
