using AdtonesAdminWebApi.DAL.Interfaces;
// using AdtonesAdminWebApi.DAL.Queries;
// using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
//using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class AdvertCategoryDAL : BaseDAL, IAdvertCategoryDAL
    {



        public AdvertCategoryDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<AdvertCategoryResult>> GetAdvertCategoryList()
        {
            string GetAdvertCategoryDataTable = @"SELECT AdvertCategoryId,ad.Name AS CategoryName,ad.CountryId, ISNULL(c.Name,'-') AS CountryName, ad.CreatedDate
                                                        FROM AdvertCategories AS ad INNER JOIN Country AS c ON c.Id = ad.CountryId
                                                        LEFT JOIN Operators AS op ON op.CountryId=ad.CountryId";
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(GetAdvertCategoryDataTable);

            var values = CheckGeneralFile(sb, builder, pais: "ad", ops: "op");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertCategoryResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> RemoveAdvertCategory(IdCollectionViewModel model)
        {
            int x = 0;
            string DeleteAdvertCategory = @"DELETE FROM AdvertCategories WHERE ";
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(DeleteAdvertCategory + " AdvertCategoryId=@Id;",
                                                                                                                    new { Id = model.id }));

                var lst = await _connService.GetConnectionStringsByCountryId(model.countryId);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(DeleteAdvertCategory + " AdtoneServerAdvertCategoryId=@Id;",
                                                                                                                     new { Id = model.id }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }

        public async Task<int> UpdateAdvertCategory(AdvertCategoryResult model)
        {
            string UpdateAdvertCategory = @"UPDATE AdvertCategories SET CountryId=@countryId, Name=@name, UpdatedDate=GETDATE() WHERE ";
            var sb = new StringBuilder();
            var sb1 = new StringBuilder();
            int x = 0;
            try
            {
                sb.Append(UpdateAdvertCategory);
                sb.Append(" AdvertCategoryId=@Id;");
                x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(sb.ToString(), new
                                    {
                                        Id = model.AdvertCategoryId,
                                        countryId = model.CountryId,
                                        name = model.CategoryName
                                    }));

                var lst = await _connService.GetConnectionStringsByCountryId(model.CountryId.GetValueOrDefault());
                List<string> conns = lst.ToList();
                sb1.Append(UpdateAdvertCategory);
                sb1.Append(" AdtoneServerAdvertCategoryId=@Id;");
                foreach (string constr in conns)
                {

                    var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId.GetValueOrDefault(), constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(sb1.ToString(), new
                                    {
                                        Id = model.AdvertCategoryId,
                                        countryId = countryId,
                                        name = model.CategoryName
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<AdvertCategoryResult> GetAdvertCategoryDetails(int id)
        {
            string GetAdvertCategoryDataTable = @"SELECT AdvertCategoryId,ad.Name AS CategoryName,ad.CountryId, ISNULL(c.Name,'-') AS CountryName, ad.CreatedDate
                                                        FROM AdvertCategories AS ad INNER JOIN Country AS c ON c.Id = ad.CountryId
                                                        LEFT JOIN Operators AS op ON op.CountryId=ad.CountryId";
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(GetAdvertCategoryDataTable + " WHERE AdvertCategoryId=@Id");
            try
            {
                builder.AddParameters(new { Id = id });


                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<AdvertCategoryResult>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertAdvertCategory(AdvertCategoryResult model)
        {
            int x = 0;
            int y = 0;
            string AddAdvertCategory = @"INSERT INTO AdvertCategories (CountryId,Name,CreatedDate,UpdatedDate,AdtoneServerAdvertCategoryId)
                                                        VALUES(@countryId,@name,GETDATE(),GETDATE(),@Id);
                                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AddAdvertCategory);
            try
            {
                builder.AddParameters(new { countryId = model.CountryId });
                builder.AddParameters(new { name = model.CategoryName });
                builder.AddParameters(new { Id = model.AdtoneServerAdvertCategoryId });


                model.AdtoneServerAdvertCategoryId = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));


                var builder2 = new SqlBuilder();
                var select2 = builder2.AddTemplate(AddAdvertCategory);

                builder2.AddParameters(new { name = model.CategoryName });
                builder2.AddParameters(new { Id = model.AdtoneServerAdvertCategoryId });


                var lst = await _connService.GetConnectionStringsByCountryId(model.CountryId.GetValueOrDefault());
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId.GetValueOrDefault(), constr);

                    builder2.AddParameters(new { countryId = countryId });

                    y += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(select2.RawSql, select2.Parameters));
                }

            }
            catch
            {
                throw;
            }
            return x;
        }


    }
}
