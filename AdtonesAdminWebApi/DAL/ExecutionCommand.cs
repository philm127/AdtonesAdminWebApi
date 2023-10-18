using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public interface IExecutionCommand
    {
        Task<T> ExecuteCommand<T>(string connStr, Func<SqlConnection, T> task);
    }

    public class ExecutionCommand : IExecutionCommand
    {
        public async Task<T> ExecuteCommand<T>(string connStr, Func<SqlConnection, T> task)
        {
            using (var conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                return task(conn);
            }
        }

    }
}
