using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.TicketingModels;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class TicketDAL : BaseDAL, ITicketDAL
    {

        public TicketDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        {}


        public async Task<TicketListModel> GetTicketDetails(int id=0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.GetTicketDetails);
            builder.AddParameters(new { Id =id });
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
                sb.Append(" WHERE q.UserId=@userId ");
                sb.Append(" ORDER BY q.Id DESC;");
                builder.AddParameters(new { userId = id });
            }
            
            var select = builder.AddTemplate(sb.ToString());
            
            try
            {
                return  await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<TicketListModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<TicketListModel>> GetTicketListForSales(int id=0)
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


        public async Task<IEnumerable<TicketListModel>> GetOperatorTicketList(int operatorId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.GetOperatorLoadTicketTable);
            builder.AddParameters(new { operatorId = operatorId });
            builder.AddParameters(new { opadrev = (int)Enums.QuestionSubjectStatus.OperatorAdreview });
            builder.AddParameters(new { cred = (int)Enums.QuestionSubjectStatus.OperatorCreditRequest });
            builder.AddParameters(new { aderr = (int)Enums.QuestionSubjectStatus.AdvertError });
            builder.AddParameters(new { adreview = (int)Enums.QuestionSubjectStatus.Adreview });

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


        public async Task<int> UpdateTicketStatus(TicketListModel model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(TicketQuery.UpdateTicketStatus);
            builder.AddParameters(new { Id = model.Id });
            builder.AddParameters(new { Status = model.Status });
            builder.AddParameters(new { UpdatedBy = model.UpdatedBy });

            try
            {
               var x =  await _executers.ExecuteCommand(_connStr,
                            conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
                return x;

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


        public async Task<int> UpdateTicketByUser(TicketListModel model)
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

    }
}
