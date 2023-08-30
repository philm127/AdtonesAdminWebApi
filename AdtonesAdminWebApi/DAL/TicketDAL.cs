using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.TicketingModels;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class TicketDAL : BaseDAL, ITicketDAL
    {
        private readonly ILoggingService _logServ;

        public TicketDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                IHttpContextAccessor httpAccessor, ILoggingService logServ)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
        }


        public async Task<TicketListModel> GetTicketDetails(int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.GetTicketDetails);
            builder.AddParameters(new { Id = id });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<TicketListModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<TicketComments>> GetTicketcomments(int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.GetTicketComments);
            builder.AddParameters(new { questionId = id });
            builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<TicketComments>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<TicketListModel>> GetTicketList(int id)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(TicketQuery.GetLoadTicketDatatable);
            if (id == 0)
            {
                var values = CheckGeneralFile(sb, builder, pais: "camp", ops: "op");
                sb = values.Item1;
                builder = values.Item2;
                sb.Append(" ORDER BY q.Id DESC;");
            }
            else
            {
                sb.Append(" AND q.UserId=@userId ");
                sb.Append(" ORDER BY q.Id DESC;");
                builder.AddParameters(new { userId = id });
            }

            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<TicketListModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<TicketListModel>> GetTicketListAsync(PagingSearchClass param)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            sb.Append(TicketQuery.GetLoadTicketDatatable);

            var role = _httpAccessor.GetRoleFromJWT();
            if (role == Enums.UserRole.UserAdmin.ToString())
                sb.Append(" AND u.RoleId = 2 ");
            else if(role == Enums.UserRole.AdvertAdmin.ToString())
                sb.Append(" AND q.SubjectId IN(3,10) ");

            var searched = CreateSeachParams(sb, builder, param);

            sb = searched.Item1;
            builder = searched.Item2;

            if (param.elementId == 0)
            {
                var values = CheckGeneralFile(sb, builder, pais: "camp", ops: "op");
                sb = values.Item1;
                builder = values.Item2;
            }
            else
            {
                sb.Append(" AND q.UserId=@userId ");
                builder.AddParameters(new { userId = param.elementId });
            }

            sb = CreateSortParams(sb,param);

            

            var select = builder.AddTemplate(sb.ToString());

            builder.AddParameters(new { PageIndex = param.page });
            builder.AddParameters(new { PageSize = param.pageSize });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<TicketListModel>(select.RawSql, select.Parameters));
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = select.RawSql.ToString();
                _logServ.PageName = "TicketDAL";
                _logServ.ProcedureName = "GetTicketListAsync";
                await _logServ.LogError();
                throw;
            }
        }



        public async Task<IEnumerable<TicketListModel>> GetTicketListForSales(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(TicketQuery.GetTicketDatatableForSales);
            if (id == 0)
            {
                var values = CheckGeneralFile(sb, builder, pais: "con", ops: "op");
                sb = values.Item1;
                builder = values.Item2;
            }
            else
            {
                sb.Append(" AND sales.SalesExecId=@Sid ");
                builder.AddParameters(new { Sid = id });

            }
            sb.Append(" ORDER BY q.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<TicketListModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<TicketListModel>> GetTicketListForAdvertiser(int id)
        {
            string getTicketDatatable = @"SELECT q.Id,ISNULL(q.UserId,0) AS UserId,ISNULL(pay.Name,'-') AS PaymentMethod,
                                                            ISNULL(CONCAT(u.FirstName,' ',u.LastName), '-') AS UserName,qs.Name AS QuestionSubject,
                                                            ISNULL(u.Email, '-') AS Email,ISNULL(QNumber,'-') AS QNumber,
                                                            ISNULL(cl.Name,'-') AS ClientName,q.CampaignProfileId,camp.CampaignName,q.CreatedDate,
                                                            Title AS QuestionTitle,q.Status,LastResponseDateTime,LastResponseDateTimeByUser,
                                                            ISNULL(u.Organisation,'-') AS Organisation
                                                        FROM Question AS q LEFT JOIN Users AS u ON u.UserId=q.UserId
                                                        LEFT JOIN Contacts AS con ON u.UserId=con.UserId
                                                        LEFT JOIN Client AS cl ON cl.Id=q.ClientId
                                                        LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=q.CampaignProfileId
                                                        LEFT JOIN QuestionSubject AS qs ON qs.SubjectId=q.SubjectId
                                                        LEFT JOIN Operators AS op ON con.CountryId=op.CountryId
                                                        LEFT JOIN PaymentMethod AS pay ON pay.Id=q.PaymentMethodId
                                                        WHERE u.UserId=@userid AND 
                                                        q.SubjectId NOT IN(3, 4, 10) ";
            
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(getTicketDatatable);
            
                builder.AddParameters(new { userid = id });

            sb.Append(" ORDER BY q.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<TicketListModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<TicketListModel>> GetOperatorTicketList(PagingSearchClass param)
        {
            var sb = new StringBuilder();
            sb.Append(TicketQuery.GetOperatorLoadTicketTable);
            var builder = new SqlBuilder();
            
            builder.AddParameters(new { operatorId = param.elementId });
            builder.AddParameters(new { opadrev = (int)Enums.QuestionSubjectStatus.OperatorAdreview });
            builder.AddParameters(new { cred = (int)Enums.QuestionSubjectStatus.OperatorCreditRequest });
            builder.AddParameters(new { aderr = (int)Enums.QuestionSubjectStatus.AdvertError });
            builder.AddParameters(new { adreview = (int)Enums.QuestionSubjectStatus.Adreview });


            var searched = CreateSeachParams(sb, builder, param);

            sb = searched.Item1;
            builder = searched.Item2;

            sb = CreateSortParams(sb, param);

            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { PageIndex = param.page });
            builder.AddParameters(new { PageSize = param.pageSize });
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<TicketListModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<string> CreateNewHelpTicket(TicketFormModel ticket)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.CreateNewHelpTicket);
            builder.AddParameters(new { UserId = ticket.UserId });
            builder.AddParameters(new { AdvertId = ticket.AdvertId });
            builder.AddParameters(new { QNumber = ticket.QNumber });
            builder.AddParameters(new { SubjectId = ticket.SubjectId });
            builder.AddParameters(new { ClientId = ticket.ClientId });
            builder.AddParameters(new { CampaignProfileId = ticket.CampaignProfileId });
            builder.AddParameters(new { PaymentMethodId = ticket.PaymentMethodId });
            builder.AddParameters(new { QNumber = ticket.QNumber });
            builder.AddParameters(new { Title = ticket.Title });
            builder.AddParameters(new { Description = ticket.Description });
            builder.AddParameters(new { Status = ticket.Status });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<string>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateTicketStatus(TicketListModel model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(string.Empty);

            if (model.UserId == model.UpdatedBy)
                select = builder.AddTemplate(TicketQuery.UpdateTicketUpdatedByUser);
            else
                select = builder.AddTemplate(TicketQuery.UpdateTicketUpdatedByAdmin);

            builder.AddParameters(new { Id = model.Id });
            builder.AddParameters(new { UpdatedBy = model.UpdatedBy });
            builder.AddParameters(new { Status = model.Status });

            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
                return x;

            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddNewComment(TicketComments ticket)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.AddComment);
            builder.AddParameters(new { UserId = ticket.UserId });
            builder.AddParameters(new { QuestionId = ticket.QuestionId });
            builder.AddParameters(new { Description = ticket.Description });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddNewCommentImage(TicketComments ticket)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.InsertCommentImage);
            builder.AddParameters(new { QuestionCommentId = ticket.CommentId });
            builder.AddParameters(new { UploadImages = ticket.ImageFile });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<string> GetEmailForLiveServer(int questionId)
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<string>(TicketQuery.GetEmailForLiveServer, new { Id = questionId }));
            }
            catch
            {
                throw;
            }
        }


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

                if (searchList.Name != null)
                {
                    string likeq = searchList.Name + "%";
                    sb.Append(" AND QNumber LIKE @qNumber ");
                    builder.AddParameters(new { qNumber = likeq });
                }

                if (searchList.Status != null)
                {
                    int stat = 0;
                    Enums.QuestionStatus choice;
                    if (Enums.QuestionStatus.TryParse(searchList.Status, out choice))
                    {
                        stat = (int)choice;
                        sb.Append(" AND q.Status = @status ");
                        builder.AddParameters(new { status = stat });
                    }
                }


                if (searchList.responseFrom != null && (searchList.responseTo == null || searchList.responseTo >= searchList.responseFrom))
                {
                    sb.Append(" AND LastResponseDateTime >= @startfrom ");
                    builder.AddParameters(new { startfrom = searchList.responseFrom });
                }

                if (searchList.responseTo != null && (searchList.responseFrom == null || searchList.responseFrom <= searchList.responseTo))
                {
                    sb.Append(" AND LastResponseDateTime <= @startto");
                    builder.AddParameters(new { startto = searchList.responseTo });
                }


                if (searchList.DateFrom != null && (searchList.DateTo == null || searchList.DateTo >= searchList.DateFrom))
                {
                    sb.Append(" AND q.CreatedDate >= @datefrom ");
                    builder.AddParameters(new { datefrom = searchList.DateFrom });
                }

                if (searchList.DateTo != null && (searchList.DateFrom == null || searchList.DateFrom <= searchList.DateTo))
                {
                    sb.Append(" AND q.CreatedDate <= @dateto");
                    builder.AddParameters(new { dateto = searchList.DateTo });
                }

                if (searchList.Client != null)
                {
                    string likeClient = searchList.Client + "%";
                    sb.Append(" AND cl.Name LIKE @likeClient ");
                    builder.AddParameters(new { likeClient = likeClient });
                }


                if (searchList.Payment != null && int.Parse(searchList.Payment) > 0)
                {
                    var likePay = int.Parse(searchList.Payment);
                    sb.Append(" AND pay.Id = @likePay ");
                    builder.AddParameters(new { likePay = likePay });
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
                case "qNumber":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" QNumber  ASC ");
                    else
                        sb.Append(" QNumber  DESC ");
                    break;
                case "clientName":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" cl.Name  ASC ");
                    else
                        sb.Append(" cl.Name  DESC ");
                    break;
                case "campaignName":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" camp.CampaignName  ASC ");
                    else
                        sb.Append(" camp.CampaignName  DESC ");
                    break;
                case "createdDate":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" q.CreatedDate  ASC ");
                    else
                        sb.Append(" q.CreatedDate  DESC ");
                    break;
                case "questionTitle":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" Title  ASC ");
                    else
                        sb.Append(" Title  DESC ");
                    break;
                case "questionSubject":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" qs.Name  ASC ");
                    else
                        sb.Append(" qs.Name  DESC ");
                    break;


                case "paymentMethod":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" pay.Name  ASC ");
                    else
                        sb.Append(" pay.Name  DESC ");
                    break;
                case "lastResponseDatetime":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" LastResponseDateTime  ASC ");
                    else
                        sb.Append(" LastResponseDateTime  DESC ");
                    break;
                case "rStatus":
                    if (param.direction.ToLower() == "asc")
                        sb.Append(" q.Status  ASC ");
                    else
                        sb.Append(" q.Status  DESC ");
                    break;
                default:
                    sb.Append(" q.Id  DESC ");
                    break;
            }
            return sb;
        }
    }
}