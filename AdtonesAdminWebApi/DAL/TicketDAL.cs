using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
            try
            {
                return  await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<HelpAdminResult>(command));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> CloseTicket(string command, HelpAdminResult model)
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
