using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AreaDAL : BaseDAL, IAreaDAL
    {

        public AreaDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService) 
            : base(configuration, executers, connService)
        {
        }


        public async Task<IEnumerable<AreaResult>> LoadAreaResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(AreaQuery.LoadAreaDataTable);
            var values = CheckGeneralFile(sb, builder,pais:"ad");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY ad.AreaId DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<AreaResult>(select.RawSql, select.Parameters));
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

        //private (StringBuilder sbuild, SqlBuilder build) CheckGeneralFile(StringBuilder sb, SqlBuilder builder, string co)
        //{
        //    var genFile = System.IO.File.ReadAllText(_getFile.TempGetGeneralJsonFile());

        //    PermissionModel gen = JsonSerializer.Deserialize<PermissionModel>(genFile);

        //    var els = gen.elements.ToList();

        //    int[] country = els.Find(x => x.name == "country").arrayId.ToArray();
        //    // operators plural as operator is a key word
        //    int[] operators = els.Find(x => x.name == "operator").arrayId.ToArray();
        //    int[] advertiser = els.Find(x => x.name == "advertiser").arrayId.ToArray();

        //    if (country.Length > 0)
        //    {
        //        sb.Append($" AND {co}.CountryId IN @country ");
        //        builder.AddParameters(new { country = country.ToArray() });

        //    }
        //    //if (operators.Length > 0)
        //    //    sb.Append(" AND ad.OperatorId IN @operator ");
        //    //if (advertiser.Length > 0)
        //    //    sb.Append(" AND ad.UserId IN @advertiser ");

        //    return (sb, builder);

        //}
    }
}
