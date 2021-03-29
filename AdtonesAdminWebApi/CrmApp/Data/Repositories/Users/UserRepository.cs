using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users;

namespace AdtonesAdminWebApi.CrmApp.Data.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //public async Task<int> Add(Core.Entities.Task entity)
        //{
        //    entity.DateCreated = DateTime.Now;
        //    var sql = "INSERT INTO Tasks (Name, Description, Status, DueDate, DateCreated) Values (@Name, @Description, @Status, @DueDate, @DateCreated);";
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql, entity);
        //        return affectedRows;
        //    }
        //}

        //public async Task<int> Delete(int id)
        //{
        //    var sql = "DELETE FROM Tasks WHERE Id = @Id;";
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        //        return affectedRows;
        //    }
        //}

        //public async Task<Core.Entities.Task> Get(int id)
        //{
        //    var sql = "SELECT * FROM Tasks WHERE Id = @Id;";
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<Core.Entities.Task>(sql, new { Id = id });
        //        return result.FirstOrDefault();
        //    }
        //}

        public async Task<IEnumerable<Core.Entities.User>> GetAll()
        {
            var sql = "SELECT UserId,OperatorId,Smelly FROM Users WHERE RoleId=3;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Core.Entities.User>(sql);
                return result;
            }
        }

        //public async Task<int> Update(Core.Entities.Task entity)
        //{
        //    entity.DateModified = DateTime.Now;
        //    var sql = "UPDATE Tasks SET Name = @Name, Description = @Description, Status = @Status, DueDate = @DueDate, DateModified = @DateModified WHERE Id = @Id;";
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql, entity);
        //        return affectedRows;
        //    }
        //}
    }
}