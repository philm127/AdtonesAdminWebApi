using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.DAL.Queries;
using System.Text;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.DAL
{
    public class UserDashboardDAL : BaseDAL, IUserDashboardDAL
    {

        public UserDashboardDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboard(int operatorId=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            
            // TODO: If we expand Operators add back comments
            //if (operatorId > 0)
            //{
            //    sb.Append(UserDashboardQuery.OperatorAdvertiserResultQuery);
            //    builder.AddParameters(new { operatorId = operatorId });
            //}
            //else
            //{
                var ytr = _httpAccessor.GetRoleIdFromJWT();
                if (ytr == (int)Enums.UserRole.SalesManager)
                {
                    sb.Append(UserDashboardQuery.SalesManagerAdvertiserResultQuery);
                }
                else
                {
                    sb.Append(UserDashboardQuery.AdvertiserResultQuery);
                }
                //sb.Append(" WHERE 1=1 ");
                //var values = CheckGeneralFile(sb, builder, pais: "cont", ops: "op", advs: "item");
                //sb = values.Item1;
                //builder = values.Item2;
                sb.Append(" AND cont.CountryId=10  ");
            //}
            sb.Append(" ORDER BY u.UserId DESC  ");
            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertiserDashboardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<OperatorDashboardResult>> GetOperatorDashboard()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            try
            {
                sb.Append(UserDashboardQuery.OperatorResultQuery);
                var values = CheckGeneralFile(sb, builder, pais: "o", ops: "o");
                sb = values.Item1;
                builder = values.Item2;

                sb.Append(" ORDER BY u.DateCreated DESC");

                var select = builder.AddTemplate(sb.ToString());
                return await _executers.ExecuteCommand(_connStr,
                                        conn => conn.Query<OperatorDashboardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdminDashboardResult>> GetAdminDashboard()
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                        conn => conn.Query<AdminDashboardResult>(UserDashboardQuery.AdminResultQuery));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SubscriberDashboardResult>> GetSubscriberDashboard(PagingSearchClass paging, string conn)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            sb.Append(UserDashboardQuery.SubscriberResultQuery);

            var searched = CreateSeachParams(sb, builder, paging);

            sb = searched.Item1;
            builder = searched.Item2;

            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(conn,
                                    conn => conn.Query<SubscriberDashboardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        #region SalesTeam

        public async Task<IEnumerable<AdvertiserDashboardResult>> GetSalesExecDashboard()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(UserDashboardQuery.SalesExecResultQuery);

            var ytr = _httpAccessor.GetRoleIdFromJWT();
            if (ytr == (int)Enums.UserRole.SalesManager)
            {
                sb.Append(" WHERE ss.ManId=@ManId");
                builder.AddParameters(new { ManId = _httpAccessor.GetUserIdFromJWT() });
            }
            sb.Append(" GROUP BY ss.ExecId,adsales.SalesExecId,u.Email, u.DateCreated, u.Activated,u.FirstName, u.LastName ");
            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertiserDashboardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }



        public async Task<IEnumerable<AdvertiserDashboardResult>> GetSalesExecForAdminDashboard()
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertiserDashboardResult>(UserDashboardQuery.SalesExecForAdminResultQuery));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboardForSales(int userId = 0)
        {
            var sb = new StringBuilder(UserDashboardQuery.SalesManagerAdvertiserResultQuery);
            var builder = new SqlBuilder();
            if (userId > 0)
            {
                sb.Append(" AND sales.UserId=@Sid ");
                builder.AddParameters(new { Sid = userId });
            }

            // TODO: If we expand Operators add back comments
            //var values = CheckGeneralFile(sb, builder, pais: "cont");

            //sb = values.Item1;
            //builder = values.Item2;
            sb.Append(" AND cont.CountryId=10  ");
            sb.Append(" ORDER BY u.UserId DESC;");

            var select = builder.AddTemplate(sb.ToString());
            
            return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertiserDashboardResult>(select.RawSql, select.Parameters));
        }



        #endregion


        private (StringBuilder sbuild, SqlBuilder build) CreateSeachParams(StringBuilder sb, SqlBuilder builder, PagingSearchClass param)
        {
            PageSearchModel searchList = null;

            if (param.search != null && param.search.Length > 3)
            {
                searchList = JsonConvert.DeserializeObject<PageSearchModel>(param.search);

                if (searchList.fullName != null)
                {
                    string likefull = searchList.fullName + "%";
                    sb.Append(" AND CONCAT(u.FirstName,' ',u.LastName) LIKE @likefull ");
                    builder.AddParameters(new { likefull = likefull });
                }

                if (searchList.DateFrom != null && (searchList.DateTo == null || searchList.DateTo >= searchList.DateFrom))
                {
                    sb.Append(" AND u.DateCreated >= @datefrom ");
                    builder.AddParameters(new { datefrom = searchList.DateFrom });
                }

                if (searchList.DateTo != null && (searchList.DateFrom == null || searchList.DateFrom <= searchList.DateTo))
                {
                    sb.Append(" AND u.DateCreated <= @dateto");
                    builder.AddParameters(new { dateto = searchList.DateTo });
                }

                if (searchList.Name != null)
                {
                    string likeMsisdn = searchList.Name + "%";
                    sb.Append(" AND p.MSISDN LIKE @likeMsisdn ");
                    builder.AddParameters(new { likeMsisdn = likeMsisdn });
                }

                if (searchList.Email != null)
                {
                    var likeMail = searchList.Email;
                    sb.Append(" AND u.Email=@likeMail ");
                    builder.AddParameters(new { likeMail = likeMail });
                }


                if (searchList.Status != null)
                {
                    int stat = 0;
                    Enums.QuestionStatus choice;
                    if (Enums.UserStatus.TryParse(searchList.Status, out choice))
                    {
                        stat = (int)choice;
                        sb.Append(" AND u.Activated = @status ");
                        builder.AddParameters(new { status = stat });
                    }
                }

            }

            return (sb, builder);
        }


    }
}
