using AdtonesAdminWebApi.DAL.Interfaces;
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


        public TicketDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


        public async Task<HelpAdminResult> GetTicketDetails(string command, int id=0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { Id =id });
            try
            {
                builder.AddParameters(new { Id = id });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<HelpAdminResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<HelpAdminResult>> GetTicketList(string command)
        {
            var sb = new StringBuilder();
            sb.Append(command);
            sb.Append(" ORDER BY q.Id DESC;");
            try
            {
                return  await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<HelpAdminResult>(sb.ToString()));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<HelpAdminResult>> GetOperatorTicketList(string command,int operatorId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { operatorId = operatorId });
            builder.AddParameters(new { opadrev = (int)Enums.QuestionSubjectStatus.OperatorAdreview });
            builder.AddParameters(new { cred = (int)Enums.QuestionSubjectStatus.OperatorCreditRequest });
            builder.AddParameters(new { aderr = (int)Enums.QuestionSubjectStatus.AdvertError });
            builder.AddParameters(new { adreview = (int)Enums.QuestionSubjectStatus.Adreview });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<HelpAdminResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateTicketStatus(string command, HelpAdminResult model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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

    }
}
