﻿using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AdvertDAL : BaseDAL, IAdvertDAL
    {

        public AdvertDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService) 
            : base(configuration, executers, connService)
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
            sb.Append(" Where ad.AdvertId=@Id");
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
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(AdvertQuery.UpdateAdvertCategory + " AdvertCategoryId=@Id;",
                                                                                                                new { Id = model.AdvertCategoryId,
                                                                                                                countryId = model.CountryId,
                                                                                                                name = model.CategoryName}));

                var lst = await _connService.GetConnectionStringsByCountry(model.CountryId.GetValueOrDefault());
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(AdvertQuery.UpdateAdvertCategory + " AdtoneServerAdvertCategoryId=@Id;",
                                                                                                            new { Id = model.AdvertCategoryId,
                                                                                                                countryId = model.CountryId,
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.AddAdvertCategory);
            try
            {
                builder.AddParameters(new { countryId = model.CountryId });
                builder.AddParameters(new { name = model.CategoryName });
                builder.AddParameters(new { Id = 0 });


                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertAdvertCategoryOperator(AdvertCategoryResult model, int catId)
        {
            var x = 0;
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertQuery.AddAdvertCategory);
            try
            {
                builder.AddParameters(new { countryId = model.CountryId });
                builder.AddParameters(new { name = model.CategoryName });
                builder.AddParameters(new { Id = catId });


                var lst = await _connService.GetConnectionStringsByCountry(model.CountryId.GetValueOrDefault());
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    x += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
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
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorId);
            var sb = new StringBuilder();
            sb.Append(AdvertQuery.UpdateAdvertStatus);
            sb.Append(" AdtoneServerAdvertId=@AdvertId;");

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


        public async Task<int> RejectAdvertReasonOperator(UserAdvertResult model,string connString,int uid, int rejId, int adId)
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

    }
}
