﻿using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class CountryAreaDAL : BaseDAL, ICountryAreaDAL
    {

        public CountryAreaDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor) 
            : base(configuration, executers, connService, httpAccessor)
        {
        }


       
        public async Task<IEnumerable<CountryResult>> LoadCountryResultSet()
        {
            string loadCountryDataTable = @"SELECT c.Id,c.Name,c.ShortName,c.CountryCode,c.CreatedDate,c.Status,
                                                        ISNULL(t.TaxPercantage,0) AS TaxPercentage, min.MinBid 
                                                        FROM Country AS c INNER JOIN CountryTax AS t ON t.CountryId=c.Id
                                                        LEFT JOIN CountryMinBid AS min ON min.CountryId=c.Id ";

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(loadCountryDataTable);
            var values = CheckGeneralFile(sb, builder, pais: "t");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY c.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CountryResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<CountryResult> GetCountryById(int id)
        {
            string getCountry = @"SELECT c.Id,c.Name,ShortName,c.CountryCode,c.CreatedDate,c.Status,
                                            t.TaxPercantage AS TaxPercentage,TermAndConditionFileName,min.MinBid
                                            FROM Country AS c INNER JOIN CountryTax AS t ON t.CountryId=c.Id
                                            LEFT JOIN CountryMinBid AS min ON min.CountryId=c.Id
                                            WHERE c.Id=@id";

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(getCountry);
            builder.AddParameters(new { id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CountryResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckCountryExists(CountryResult model)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<bool>("SELECT COUNT(1) FROM Country WHERE LOWER(Name) = @name", new { name = model.Name.Trim().ToLower() }));

            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddCountry(CountryResult model)
        {
            var sb = new StringBuilder();

            string addCountry = @"INSERT INTO Country(UserId,Name,ShortName,CreatedDate,UpdatedDate,Status,
                                                    TermAndConditionFileName,CountryCode,AdtoneServeCountryId)
                                            VALUES(@UserId,@Name,@ShortName, GETDATE(), GETDATE(),1, 
                                                @TermAndConditionFileName, @CountryCode,@AdtoneServeCountryId);
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            string addTax = @"INSERT INTO CountryTax(UserId,CountryId,TaxPercantage,CreatedDate,UpdatedDate,Status)
                                            VALUES(@UserId,@CountryId,@TaxPercantage, GETDATE(), GETDATE(),1);";



            int x = 0;
            var ctryId = 0;
            var userId = _httpAccessor.GetUserIdFromJWT();

            ctryId = await _executers.ExecuteCommand(_connStr,
                                                conn => conn.ExecuteScalar<int>(addCountry, new
                                                {
                                                    Name = model.Name.Trim(),
                                                    ShortName = model.ShortName.ToUpper().Trim(),
                                                    TermAndConditionFileName = model.TermAndConditionFileName,
                                                    CountryCode = model.CountryCode.Trim(),
                                                    AdtoneServeCountryId = model.AdtoneServeCountryId,
                                                    UserId = userId
                                                }));

            var y = await _executers.ExecuteCommand(_connStr,
                                                conn => conn.ExecuteScalar<int>(addTax, new
                                                {
                                                    UserId = userId,
                                                    CountryId = ctryId,
                                                    TaxPercantage = model.TaxPercentage
                                                }));


            var lst = await _connService.GetConnectionStrings();
            List<string> conns = lst.ToList();

            sb.Append("SET IDENTITY_INSERT Country ON;");
            sb.Append(addCountry);
            sb.Append("SET IDENTITY_INSERT Country OFF;");

            foreach (string constr in conns)
            {
                if (constr != null && constr.Length > 10)
                {
                    model.UserId = await _connService.GetUserIdFromAdtoneIdByConnString(userId, constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(addCountry, new
                                    {
                                        Id = ctryId,
                                        Name = model.Name.Trim(),
                                        ShortName = model.ShortName.ToUpper().Trim(),
                                        TermAndConditionFileName = model.TermAndConditionFileName,
                                        CountryCode = model.CountryCode.Trim(),
                                        AdtoneServeCountryId = ctryId,
                                        UserId = model.UserId
                                    }));
                }

            }

            x = await AddMinBidCountry(ctryId, model.MinBid);

            return x;
        }


        public async Task<int> AddMinBidCountry(int countryId, decimal minbid)
        {
            int x = 0;

            x = await _executers.ExecuteCommand(_connStr,
                                                conn => conn.ExecuteScalar<int>(CountryAreaQuery.AddMinBid, new
                                                {
                                                    CountryId = countryId,
                                                    MinBid = minbid
                                                }));

            var lst = await _connService.GetConnectionStrings();
            List<string> conns = lst.ToList();

            foreach (string constr in conns)
            {
                if (constr != null && constr.Length > 10)
                {
                    x = await AddMinBidCountryByProv(countryId, minbid, constr);
                }

            }

            return x;
        }


        public async Task<int> AddMinBidCountryByProv(int countryId, decimal minbid, string constr)
        {
            int x = 0;

                    x = await _executers.ExecuteCommand(constr,
                                                conn => conn.ExecuteScalar<int>(CountryAreaQuery.AddMinBid, new
                                                {
                                                    CountryId = countryId,
                                                    MinBid = minbid
                                                }));

            return x;
        }


        public async Task<int> UpdateMinBidCountry(int countryId, decimal minbid)
        {
            int x = 0;
            var ctryId = 0;

            var select_query = @"SELECT CountryId FROM CountryMinBid WHERE CountryId=@Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    ctryId = await connection.QueryFirstOrDefaultAsync<int>(select_query, new { Id = countryId });
                }

            if (ctryId == 0)
                x = await AddMinBidCountry(countryId, minbid);
            else
            {
                x = await _executers.ExecuteCommand(_connStr,
                                                    conn => conn.ExecuteScalar<int>(CountryAreaQuery.UpdateMinBid, new
                                                    {
                                                        CountryId = countryId,
                                                        MinBid = minbid
                                                    }));
            }

            var lst = await _connService.GetConnectionStrings();
            List<string> conns = lst.ToList();

            foreach (string constr in conns)
            {
                if (constr != null && constr.Length > 10)
                {
                    using (var connection = new SqlConnection(constr))
                    {
                        await connection.OpenAsync();
                        ctryId = await connection.QueryFirstOrDefaultAsync<int>(select_query, new { Id = countryId });
                    }
                    if (ctryId == 0)
                        x = await AddMinBidCountryByProv(countryId, minbid,constr);
                    else
                    {
                        x = await _executers.ExecuteCommand(constr,
                                                conn => conn.ExecuteScalar<int>(CountryAreaQuery.UpdateMinBid, new
                                                {
                                                    CountryId = ctryId,
                                                    MinBid = minbid
                                                }));
                    }
                }

            }

            return x;
        }



        public async Task<int> UpdateCountry(CountryResult model)
        {
            int x = 0;
            var userId = _httpAccessor.GetUserIdFromJWT();

            x = await _executers.ExecuteCommand(_connStr,
                                                conn => conn.ExecuteScalar<int>(CountryAreaQuery.UpdateCountry, new
                                                {
                                                    Name = model.Name.Trim(),
                                                    ShortName = model.ShortName.ToUpper().Trim(),
                                                    TermAndConditionFileName = model.TermAndConditionFileName,
                                                    CountryCode = model.CountryCode.Trim(),
                                                    UserId = userId,
                                                    Id = model.Id
                                                }));

            var y = await _executers.ExecuteCommand(_connStr,
                                                conn => conn.ExecuteScalar<int>(CountryAreaQuery.UpdateTax, new
                                                {
                                                    UserId = userId,
                                                    CountryId = model.Id,
                                                    TaxPercantage = model.TaxPercentage
                                                }));

            var countryId = 0;
            var lst = await _connService.GetConnectionStrings();
            List<string> conns = lst.ToList();

            foreach (string constr in conns)
            {
                if (constr != null && constr.Length > 10)
                {
                    model.UserId = await _connService.GetUserIdFromAdtoneIdByConnString(userId, constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(CountryAreaQuery.UpdateCountry, new
                                    {
                                        Name = model.Name.Trim(),
                                        ShortName = model.ShortName.ToUpper().Trim(),
                                        TermAndConditionFileName = model.TermAndConditionFileName,
                                        CountryCode = model.CountryCode.Trim(),
                                        UserId = model.UserId,
                                        Id = model.Id
                                    }));
                }
            }
            x = await UpdateMinBidCountry(model.Id, model.MinBid);

            return x;
        }



        public async Task<IEnumerable<AreaResult>> LoadAreaResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(CountryAreaQuery.LoadAreaDataTable);
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
            var select = builder.AddTemplate(CountryAreaQuery.GetAreaById);
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
            var select = builder.AddTemplate(CountryAreaQuery.DeleteArea);
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
            var select = builder.AddTemplate(CountryAreaQuery.UpdateArea);
            builder.AddParameters(new { AreaName = model.AreaName });
            builder.AddParameters(new { AreaId = model.AreaId });

            var x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));

            var lst = await _connService.GetConnectionStrings();
            List<string> conns = lst.ToList();

            foreach (string constr in conns)
            {
                if (constr != null && constr.Length > 10)
                {
                    x = await _executers.ExecuteCommand(constr,
                             conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
                }
            }
            return x;
        }


        public async Task<int> AddArea(AreaResult areamodel)
        {
            var x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(CountryAreaQuery.AddArea, new
                         {
                             AreaName = areamodel.AreaName.Trim(),
                             CountryId = areamodel.CountryId
                         }));

            var countryId = 0;
            var lst = await _connService.GetConnectionStrings();
            List<string> conns = lst.ToList();

            foreach (string constr in conns)
            {
                if (constr != null && constr.Length > 10)
                {
                    countryId = await _connService.GetCountryIdFromAdtoneId(areamodel.CountryId.GetValueOrDefault(), constr);
                    x = await _executers.ExecuteCommand(_connStr,
                                 conn => conn.ExecuteScalar<int>(CountryAreaQuery.AddArea, new
                                 {
                                     AreaName = areamodel.AreaName.Trim(),
                                     CountryId = countryId
                                 }));
                }

            }
            return x;
        }


        public async Task<bool> CheckAreaExists(AreaResult areamodel)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CountryAreaQuery.CheckAreaExists);
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


        public async Task<decimal> GetMinBidByCountry(int countryId)
        {
            var select_query = @"SELECT MinBid FROM CountryMinBid WHERE CountryId=@Id";

            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<decimal>(select_query, new { Id = countryId });
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
