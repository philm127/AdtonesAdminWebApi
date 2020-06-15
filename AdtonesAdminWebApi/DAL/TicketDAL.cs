using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.TicketingModels;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class TicketDAL : ITicketDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly ITicketQuery _commandText;

        public TicketDAL(IConfiguration configuration, IExecutionCommand executers, ITicketQuery commandText)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _commandText = commandText;
        }


        public async Task<TicketListModel> GetTicketDetails(int id=0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.GetTicketDetails);
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


        public async Task<TicketComments> GetTicketcomments(int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.GetTicketComments);
            builder.AddParameters(new { questionId = id });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<TicketComments>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<TicketListModel>> GetTicketList(int id)
        {
            var sb = new StringBuilder();
            sb.Append(_commandText.GetLoadTicketDatatable);
            if(id == 0)
                sb.Append(" ORDER BY q.Id DESC;");
            else
                sb.Append(" WHERE q.UserId=@userId;");
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { userId = id });
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


        public async Task<IEnumerable<TicketListModel>> GetOperatorTicketList(int operatorId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.GetOperatorLoadTicketTable);
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
            var select = builder.AddTemplate(_commandText.UpdateTicketStatus);
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


        public async Task<string> CreateNewHelpTicket(string command, TicketFormModel ticket)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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

    }
}
