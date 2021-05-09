using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AdtonesAdminWebApi.CRM.Models;
using AdtonesAdminWebApi.CRM.Models.Subscriber;
using AdtonesAdminWebApi.Services;
using AutoMapper;

namespace AdtonesAdminWebApi.CRM.Repositories
{
    public class SubscriberRepositorry : BaseDAL, ISubscriberRepositorry
    {
        private readonly IMapper _mapper;

        public SubscriberRepositorry(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                      IMapper mapper) : base(configuration, executers, connService, httpAccessor)
                                    {
                                        _mapper = mapper;
                                    }


        public async Task<IEnumerable<SubscriberListModel>> GetSubscribers(PagingSearchClass param)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            string getQuery = @"SELECT u.UserId,u.Activated,u.DateCreated,FirstName,LastName,p.MSISDN,op.OperatorName,u.Email,u.VerificationStatus,
                                                  co.Name,op.CountryId,u.OperatorId
                                                  FROM Users AS u LEFT JOIN UserProfile AS p ON p.UserId=u.UserId
                                                  INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                  INNER JOIN Country AS co ON co.Id=op.CountryId
                                                  WHERE u.RoleId=2 AND u.VerificationStatus=1 ";

            sb.Append(getQuery);

            var role = _httpAccessor.GetRoleFromJWT();
            //if (role == Enums.UserRole.UserAdmin.ToString())
            //    sb.Append(" AND u.RoleId = 2 ");
            //else if (role == Enums.UserRole.AdvertAdmin.ToString())
            //    sb.Append(" AND q.SubjectId IN(3,10) ");

            var searched = CreateSeachParams(sb, builder, param);

            sb = searched.Item1;
            builder = searched.Item2;

            var values = CheckGeneralFile(sb, builder, pais: "op", ops: "u");
            sb = values.Item1;
            builder = values.Item2;

            sb = CreateSortParams(sb, param);

            var select = builder.AddTemplate(sb.ToString());

            builder.AddParameters(new { PageIndex = param.page });
            builder.AddParameters(new { PageSize = param.pageSize });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<SubscriberListModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }



        public async Task<SubscriberDto> GetSubscriber(int id)
        {

            string getQuery = @"SELECT u.UserId,u.Activated,u.DateCreated,FirstName,LastName,p.MSISDN,op.OperatorName,u.Email,u.VerificationStatus,
                                                  co.Name,op.CountryId,u.OperatorId
                                                  FROM Users AS u LEFT JOIN UserProfile AS p ON p.UserId=u.UserId
                                                  INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                  INNER JOIN Country AS co ON co.Id=op.CountryId
                                                  WHERE u.RoleId=2 AND u.VerificationStatus=1 ";


            try
            {

                var entity =  await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<SubscriberModel>(getQuery, new { Id = id }));
                return _mapper.Map<SubscriberDto>(entity);
            }
            catch
            {
                throw;
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


        public static string TotalPlayStuff => @"SELECT
                                                    ISNULL(SUM(TotOfPlaySixOver),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(TotOfPlayUnderSix),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(TotPlaylength),0) AS TotPlaylength,
                                                    ISNULL(SUM(TotOfSMS),0) AS TotOfSMS,
                                                    ISNULL(SUM(TotOfEmail),0) AS TotOfEmail,
                                                    ISNULL(SUM(TotCancelled),0) AS TotCancelled,
                                                    ISNULL(SUM(NumOfPlaySixOver),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(NumOfPlayUnderSix),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(Playlength),0) AS Playlength,
                                                    ISNULL(SUM(NumOfSMS),0) AS NumOfSMS,
                                                    ISNULL(SUM(NumOfEmail),0) AS NumOfEmail,
                                                    ISNULL(SUM(NumCancelled),0) AS NumCancelled
                                                    FROM (
                                                    SELECT CampaignProfileId,ISNULL(SUM(TotOfPlaySixOver),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(TotOfPlayUnderSix),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(TotPlaylength),0) AS TotPlaylength,
                                                    ISNULL(SUM(TotOfSMS),0) AS TotOfSMS,
                                                    ISNULL(SUM(TotOfEmail),0) AS TotOfEmail,
                                                    ISNULL(SUM(TotCancelled),0) AS TotCancelled,
                                                    ISNULL(SUM(NumOfPlaySixOver),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(NumOfPlayUnderSix),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(Playlength),0) AS Playlength,
                                                    ISNULL(SUM(NumOfSMS),0) AS NumOfSMS,
                                                    ISNULL(SUM(NumOfEmail),0) AS NumOfEmail,
                                                    ISNULL(SUM(NumCancelled),0) AS NumCancelled
                                                    FROM (
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit GROUP BY CampaignProfileId

                                                    UNION ALL
                                                    " + PlayStuffUnion + @"
                                                    FROM CampaignAudit2 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                    " + PlayStuffUnion + @"
                                                    FROM CampaignAudit3 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit4 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit5 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit6 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit7 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit8 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit9 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit10 GROUP BY CampaignProfileId
                                                    ) as x
                                                    GROUP BY CampaignProfileId ) AS ca
                                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                    WHERE op.OperatorId=@searchOperators ";


        public static string PlayStuffUnion => @"SELECT CampaignProfileId,ISNULL(SUM(CASE WHEN PlayLengthTicks >= 6000 THEN 1 ELSE 0 END),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks < 6000 THEN 1 ELSE 0 END),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(ISNULL(PlayLengthTicks,0)),0) AS TotPlaylength,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND SMS IS NOT NULL) THEN 1 ELSE 0 END),0) AS TotOfSMS,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND EmailCost != 0) THEN 1 ELSE 0 END),0) AS TotOfEmail,
                                                    ISNULL(SUM(CASE WHEN Status= 'cancelled' THEN 1 ELSE 0 END),0) AS TotCancelled,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks>=6000 AND StartTime 
                                                                        BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks < 6000 AND StartTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks> 0 AND StartTime BETWEEN @start AND @end THEN PlayLengthTicks ELSE 0 END),0) AS Playlength,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND SMS IS NOT NULL AND StartTime BETWEEN @start AND @end) THEN 1 ELSE 0 END),0) AS NumOfSMS,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND EmailCost != 0 AND StartTime BETWEEN @start AND @end) THEN 1 ELSE 0 END),0) AS NumOfEmail,
                                                    ISNULL(SUM(CASE WHEN Status= 'cancelled' AND StartTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumCancelled";



    }
}
