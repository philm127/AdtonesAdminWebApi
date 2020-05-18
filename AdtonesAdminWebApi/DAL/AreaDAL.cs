using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AreaDAL : IAreaDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;


        public AreaDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


        public async Task<IEnumerable<AreaResult>> LoadAreaResultSet(string command)
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<AreaResult>(command));
            }
            catch
            {
                throw;
            }
        }


        public async Task<AreaResult> GetAreaById(string command, int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { areaid = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<AreaResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> DeleteAreaById(string command, int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { areaid = id });

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


        public async Task<int> UpdateArea(string command, AreaResult model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { AreaName = model.AreaName });
            builder.AddParameters(new { IsActive = model.IsActive });
            builder.AddParameters(new { CountryId = model.CountryId });
            builder.AddParameters(new { AreaId = model.AreaId });

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


        public async Task<int> AddArea(string command, AreaResult areamodel)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { AreaName = areamodel.AreaName.Trim().ToLower() });
            builder.AddParameters(new { CountryId = areamodel.CountryId });

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
