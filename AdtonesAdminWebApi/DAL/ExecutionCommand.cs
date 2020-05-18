using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class ExecutionCommand : IExecutionCommand
    {
        public void ExecuteCommand(string connStr, Action<SqlConnection> task)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                task(conn);
            }
        }


        public async Task<T> ExecuteCommand<T>(string connStr, Func<SqlConnection, T> task)
        {
            using (var conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                return task(conn);
            }
        }


        public T ExecuteCommandTEST<T>(string connStr, Func<SqlConnection, T> task)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                return task(conn);
            }
        }
    }


    public interface IExecutionCommand
    {
        void ExecuteCommand(string connStr, Action<SqlConnection> task);
        Task<T> ExecuteCommand<T>(string connStr, Func<SqlConnection, T> task);
        T ExecuteCommandTEST<T>(string connStr, Func<SqlConnection, T> task);
    }
}
