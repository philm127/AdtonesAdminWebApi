using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
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


        public async Task<IEnumerable<AreaResult>> LoadAreaResultSet()
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<AreaResult>(AreaQuery.LoadAreaDataTable));
            }
            catch
            {
                throw;
            }
        }


        public async Task<AreaResult> GetAreaById(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AreaQuery.GetAreaById);
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


        public async Task<int> DeleteAreaById(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AreaQuery.DeleteArea);
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


        public async Task<int> UpdateArea(AreaResult model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AreaQuery.UpdateArea);
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


        public async Task<int> AddArea(AreaResult areamodel)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AreaQuery.AddArea);
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


        public async Task<bool> CheckAreaExists(AreaResult areamodel)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AreaQuery.CheckAreaExists);
            builder.AddParameters(new { areaname = areamodel.AreaName.Trim().ToLower() });
            builder.AddParameters(new { countryId = areamodel.CountryId });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<bool>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }
    }
}
