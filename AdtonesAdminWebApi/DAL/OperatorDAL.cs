using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class OperatorDAL : BaseDAL, IOperatorDAL
    {

        public OperatorDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor) 
            : base(configuration, executers, connService, httpAccessor)
        {
        }


        public async Task<IEnumerable<OperatorResult>> LoadOperatorResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(OperatorQuery.LoadOperatorDataTable);
            var values = CheckGeneralFile(sb, builder, pais:"op",ops: "op");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY op.OperatorId DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<OperatorResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckOperatorExists(OperatorFormModel model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(OperatorQuery.CheckIfOperatorExists);
            builder.AddParameters(new { op = model.OperatorName.ToLower() });

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


        public async Task<int> AddOperator(OperatorFormModel model)
        {
            int x = 0;
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(OperatorQuery.AddNewOperator);
            builder.AddParameters(new { OperatorName = model.OperatorName.Trim() });
            builder.AddParameters(new { CountryId = model.CountryId });
            builder.AddParameters(new { EmailCost = model.EmailCost });
            builder.AddParameters(new { SmsCost = model.SmsCost });
            builder.AddParameters(new { CurrencyId = model.CurrencyId });
            builder.AddParameters(new { AdtoneServerOperatorId = model.AdtoneServerOperatorId });

            try
            {
                model.AdtoneServerOperatorId = await _executers.ExecuteCommand(_connStr,
                                                    conn => conn.ExecuteScalar<int>(OperatorQuery.AddNewOperator, new
                                                                                                    {
                                                                                                        OperatorName = model.OperatorName.Trim(),
                                                                                                        CountryId = model.CountryId,
                                                                                                        EmailCost = model.EmailCost,
                                                                                                        SmsCost = model.SmsCost,
                                                                                                        CurrencyId = model.CurrencyId,
                                                                                                        AdtoneServerOperatorId = model.AdtoneServerOperatorId
                                                                                                    }));

                //var countryId = 0;
                //var lst = await _connService.GetConnectionStringsByCountry(model.CountryId);
                //List<string> conns = lst.ToList();

                //foreach (string constr in conns)
                //{
                //    countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);
                //    x = await _executers.ExecuteCommand(constr,
                //                    conn => conn.ExecuteScalar<int>(OperatorQuery.AddNewOperator, new
                //                                                                                    {
                //                                                                                        OperatorName = model.OperatorName.Trim(),
                //                                                                                        CountryId = countryId,
                //                                                                                        EmailCost = model.EmailCost,
                //                                                                                        SmsCost = model.SmsCost,
                //                                                                                        CurrencyId = model.CurrencyId,
                //                                                                                        AdtoneServerOperatorId = model.AdtoneServerOperatorId
                //                                                                                    }));
                //}
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<OperatorFormModel> GetOperatorById(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(OperatorQuery.GetOperatorById);
            builder.AddParameters(new { Id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<OperatorFormModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateOperator(OperatorFormModel model)
        {
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                                    conn => conn.ExecuteScalar<int>(OperatorQuery.UpdateOperator, new
                                                    {
                                                        IsActive = model.IsActive,
                                                        EmailCost = model.EmailCost,
                                                        SmsCost = model.SmsCost,
                                                        OperatorId = model.OperatorId
                                                    }));

                var operatorId = 0;
                var constr = await _connService.GetConnectionStringByOperator(model.OperatorId);
                operatorId = await _connService.GetOperatorIdFromAdtoneId(model.OperatorId);
                x = await _executers.ExecuteCommand(constr,
                                                conn => conn.ExecuteScalar<int>(OperatorQuery.UpdateOperator, new
                                                {
                                                    IsActive = model.IsActive,
                                                    EmailCost = model.EmailCost,
                                                    SmsCost = model.SmsCost,
                                                    OperatorId = operatorId
                                                }));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<IEnumerable<OperatorMaxAdvertsFormModel>> LoadOperatorMaxAdvertResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(OperatorQuery.GetMaxAdvertResultSet);
            var values = CheckGeneralFile(sb, builder, pais: "op", ops: "op");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY OperatorMaxAdvertId DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<OperatorMaxAdvertsFormModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckMaxAdvertExists(OperatorMaxAdvertsFormModel model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(OperatorQuery.CheckIfMaxAdvertExists);
            builder.AddParameters(new { keyname = model.KeyName.ToLower() });
            builder.AddParameters(new { opid = model.OperatorId });

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


        public async Task<int> AddOperatorMaxAdvert(OperatorMaxAdvertsFormModel model)
        {
            int x = 0;
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(OperatorQuery.AddOperatorMaxAdvert);
            builder.AddParameters(new { KeyName = model.KeyName.Trim() });
            builder.AddParameters(new { KeyValue = model.KeyValue });
            builder.AddParameters(new { OperatorId = model.OperatorId });
            

            try
            {
                model.AdtoneServerOperatorMaxAdvertId = await _executers.ExecuteCommand(_connStr,
                                                    conn => conn.ExecuteScalar<int>(OperatorQuery.AddOperatorMaxAdvert, new
                                                                                {
                                                                                    KeyName = model.KeyName.Trim(),
                                                                                    KeyValue = model.KeyValue,
                                                                                    OperatorId = model.OperatorId,
                                                                                    AdtoneServerOperatorMaxAdvertId = model.AdtoneServerOperatorMaxAdvertId
                                                                                }));

                var operatorId = 0;
                var constr = await _connService.GetConnectionStringByOperator(model.OperatorId);

                operatorId = await _connService.GetOperatorIdFromAdtoneId(model.OperatorId);
                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(OperatorQuery.AddOperatorMaxAdvert, new
                                                                                {
                                                                                    KeyName = model.KeyName.Trim(),
                                                                                    KeyValue = model.KeyValue,
                                                                                    OperatorId = operatorId,
                                                                                    AdtoneServerOperatorMaxAdvertId = model.AdtoneServerOperatorMaxAdvertId
                                                                                }));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<OperatorMaxAdvertsFormModel> GetOperatorMaxAdvertById(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(OperatorQuery.GetOperatorMaxAdvertById);
            builder.AddParameters(new { Id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<OperatorMaxAdvertsFormModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }



        public async Task<int> UpdateOperatorMaxAdvert(OperatorMaxAdvertsFormModel model)
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();

            sb.Append(OperatorQuery.UpdateOperatorMaxAdvert);
            sb2.Append(OperatorQuery.UpdateOperatorMaxAdvert);

            sb.Append("WHERE OperatorMaxAdvertId = @OperatorMaxAdvertId");
            sb2.Append("WHERE AdtoneServerOperatorMaxAdvertId = @OperatorMaxAdvertId");


            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                                                conn => conn.ExecuteScalar<int>(sb.ToString(), new
                                                                                                        {
                                                                                                            OperatorMaxAdvertId = model.OperatorMaxAdvertId,
                                                                                                            KeyValue = model.KeyValue
                                                                                                        }));

                var constr = await _connService.GetConnectionStringByOperator(model.OperatorId);
                x = await _executers.ExecuteCommand(constr,
                                                            conn => conn.ExecuteScalar<int>(sb2.ToString(), new
                                                                                                        {
                                                                                                            OperatorMaxAdvertId = model.OperatorMaxAdvertId,
                                                                                                            KeyValue = model.KeyValue
                                                                                                        }));
            }
            catch
            {
                throw;
            }

            return x;
        }


    }
}
