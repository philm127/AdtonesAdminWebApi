using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users;
using System.Text;
using AdtonesAdminWebApi.ViewModels;
using Newtonsoft.Json;

namespace AdtonesAdminWebApi.CrmApp.Data.Repositories.Users
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IPermmisionSearch _search;

        public SubscriberRepository(IConfiguration configuration, IPermmisionSearch search)
        {
            _configuration = configuration;
            _search = search;
        }

        public async Task<IEnumerable<Core.Entities.User>> GetAll(PagingSearchClass entity)
        {
            PagingSearchClass param = new PagingSearchClass();
            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            string getQuery = @"SELECT u.UserId,u.Activated,u.DateCreated,FirstName,LastName,p.MSISDN,op.OperatorName,u.Email,u.VerificationStatus,
                                                  co.Name,op.CountryId,u.OperatorId
                                                  FROM Users AS u LEFT JOIN UserProfile AS p ON p.UserId=u.UserId
                                                  INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                  INNER JOIN Country AS co ON co.Id=op.CountryId
                                                  WHERE u.RoleId=2 AND u.VerificationStatus=1 ";

            sb.Append(getQuery);

            var searched = CreateSeachParams(sb, builder, param);

            sb = searched.Item1;
            builder = searched.Item2;

            var values = _search.CheckGeneralFile(sb, builder, pais: "op", ops: "u");
            sb = values.Item1;
            builder = values.Item2;

            sb = CreateSortParams(sb, param);

            var select = builder.AddTemplate(sb.ToString());

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Core.Entities.User>(select.RawSql, select.Parameters);
                return result;
            }
        }





        #region Create Search and Order Params

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

                if (searchList.country != null)
                {
                    sb.Append(" AND co.Name = @country ");
                    builder.AddParameters(new { country = searchList.country });
                }

                if (searchList.Operator != null)
                {
                    sb.Append(" AND op.OperatorName = @operators ");
                    builder.AddParameters(new { operators = searchList.Operator });
                }


                if (searchList.Status != null)
                {
                    int stat = 0;
                    Enums.UserStatus choice;
                    if (Enums.UserStatus.TryParse(searchList.Status, out choice))
                    {
                        stat = (int)choice;
                        sb.Append(" AND u.Activated = @status ");
                        builder.AddParameters(new { status = stat });
                    }
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



                if (searchList.TypeName != null)
                {
                    var likeSub = int.Parse(searchList.TypeName);
                    sb.Append(" AND qs.SubjectId=@likeSub ");
                    builder.AddParameters(new { likeSub = likeSub });
                }

            }

            return (sb, builder);
        }


        private StringBuilder CreateSortParams(StringBuilder sb, PagingSearchClass param)
        {
            sb.Append(" ORDER BY ");

            switch (param.sort)
            {
                case "userName":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" u.FirstName  ASC ");
                    else
                        sb.Append(" u.FirstName  DESC ");
                    break;
                case "email":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" u.Email  ASC ");
                    else
                        sb.Append(" u.Email  DESC ");
                    break;
                case "organisation":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" u.Organisation  ASC ");
                    else
                        sb.Append(" u.Organisation  DESC ");
                    break;

                case "createdDate":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" u.DateCreated  ASC ");
                    else
                        sb.Append(" q.DateCreated  DESC ");
                    break;

                case "rStatus":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" u.Activated  ASC ");
                    else
                        sb.Append(" u.Activated  DESC ");
                    break;
                default:
                    sb.Append(" u.UserId  DESC ");
                    break;
            }
            return sb;
        }


        #endregion
    }
}
