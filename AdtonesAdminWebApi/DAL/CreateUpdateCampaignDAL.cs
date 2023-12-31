﻿using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class CreateUpdateCampaignDAL : BaseDAL, ICreateUpdateCampaignDAL
    {

        public CreateUpdateCampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                            IHttpContextAccessor httpAccessor) : base(configuration, executers, connService, httpAccessor)
        { }


        


        //public async Task<int> AddProfileTimeSettings(CampaignProfileTimeSetting model, int countryId, int provCampaignId)
        //{
        //    var x = 0;

        //    try
        //    {

        //        x = await _executers.ExecuteCommand(_connStr,
        //                        conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.AddProfileTimeSettings,model));

        //        if(x > 0)
        //        {
        //            var connList = await _connService.GetConnectionStringsByCountry(countryId);
        //            if (connList != null)
        //            {
        //                foreach (var conn in connList)
        //                {
        //                    model.CampaignProfileId = provCampaignId;
        //                    model.AdtoneServerCampaignProfileTimeId = x;

        //                    x = await _executers.ExecuteCommand(conn,
        //                            conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.AddProfileTimeSettings, model));
        //                }

        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return x;
        //}



        public async Task<NewCampaignProfileFormModel> CreateNewCampaign(NewCampaignProfileFormModel model)
        {
            var x = 0;
            var y = 0;

            try
            {

                model.AdtoneServerCampaignProfileId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewCampaign, model));

                // Campaign Profile Collection Project
                if(_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                    x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.AddCampaignProfileEx, new 
                                                                                                { CampaignProfileId = model.AdtoneServerCampaignProfileId, 
                                                                                                   NonBillable = 1, IsProfileCampaign = 1, 
                                                                                                   CampaignCategoryId = model.CampaignCategoryId.GetValueOrDefault() 
                                                                                                } ));

                if (model.AdtoneServerCampaignProfileId != null && model.AdtoneServerCampaignProfileId > 0)
                {
                    var conn = await _connService.GetConnectionStringByOperator(model.OperatorId);
                    if (conn != null && conn.Length > 10)
                    {

                        model.UserId = await _connService.GetUserIdFromAdtoneIdByConnString(model.UserId, conn);
                        if (model.ClientId != null)
                            model.ClientId = await _connService.GetClientIdFromAdtoneIdByConnString(model.ClientId.Value, conn);
                        model.CampaignProfileId = await _executers.ExecuteCommand(conn,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewCampaign, model));


                        if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                        {
                            string sql = @"SELECT CampaignCategoryId FROM CampaignCategory WHERE AdtoneServerCampaignCategoryId=@Id";

                            y = await _executers.ExecuteCommand(conn,
                                        conn => conn.ExecuteScalar<int>(sql, new { Id = model.CampaignCategoryId.GetValueOrDefault() }));

                            x = await _executers.ExecuteCommand(conn,
                                        conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.AddCampaignProfileEx, new
                                        {
                                            CampaignProfileId = model.CampaignProfileId,
                                            NonBillable = 1,
                                            IsProfileCampaign = 1,
                                            CampaignCategoryId = y
                                        }));

                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return model;
        }


        public async Task<int> UpdateCampaignDetails(NewCampaignProfileFormModel model)
        {
            var x = 0;
            var y = 0;
            var cid = model.CampaignProfileId;

            try
            {

                x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.UpdateCampaignDetails, model));

                // Campaign Profile Collection Project
                if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                    x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.UpdateCampaignProfileEx, new
                                {
                                    CampaignProfileId = model.CampaignProfileId,
                                    CampaignCategoryId = model.CampaignCategoryId.GetValueOrDefault()
                                }));

                var conn = await _connService.GetConnectionStringByOperator(model.OperatorId);
                if (conn != null && conn.Length > 10)
                {

                    model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(model.CampaignProfileId, conn);

                    x = await _executers.ExecuteCommand(conn,
                            conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.UpdateCampaignDetails, model));


                    if (_httpAccessor.GetRoleIdFromJWT() == (int)Enums.UserRole.ProfileAdmin)
                    {
                        string sql = @"SELECT CampaignCategoryId FROM CampaignCategory WHERE AdtoneServerCampaignCategoryId=@Id";

                        y = await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(sql, new { Id = model.CampaignCategoryId.GetValueOrDefault() }));

                        x = await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.UpdateCampaignProfileEx, new
                                    {
                                        CampaignProfileId = model.CampaignProfileId,
                                        CampaignCategoryId = y
                                    }));

                    }
                }
                model.CampaignProfileId = cid;
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> InsertNewClient(ClientViewModel model)
        {
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                     conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewClient, model));

                var constr = await _connService.GetConnectionStringByOperator(model.OperatorId);
                if (constr != null && constr.Length > 10)
                {
                    model.AdtoneServerClientId = x;
                    model.UserId = await _connService.GetUserIdFromAdtoneIdByConnString(model.UserId, constr);
                    model.CountryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);


                    var y = await _executers.ExecuteCommand(constr,
                         conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewClient, model));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<ClientViewModel> GetClientDetails(int clientId)
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.QueryFirstOrDefault<ClientViewModel>(CreateUpdateCampaignQuery.GetClientDetails, new { Id = clientId }));

            }
            catch
            {
                throw;
            }
        }


        


        //public async Task<CampaignAdvertFormModel> CreateNewIntoCampaignAdverts(CampaignAdvertFormModel model, int operatorId, int provAdId)
        //{
        //    var newModel = new CampaignAdvertFormModel();
        //    try
        //    {

        //        model.AdtoneServerCampaignAdvertId = await _executers.ExecuteCommand(_connStr,
        //                        conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewIntoCampaignAdverts, model));

        //        if (model.AdtoneServerCampaignAdvertId != null && model.AdtoneServerCampaignAdvertId > 0)
        //        {
        //            var conn = await _connService.GetConnectionStringByOperator(operatorId);
        //            if (conn != null && conn != "")
        //            {
        //                newModel.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneId(model.CampaignProfileId, operatorId);
        //                newModel.AdvertId = provAdId;
        //                newModel.AdtoneServerCampaignAdvertId = model.AdtoneServerCampaignAdvertId;
        //                newModel.Advert = model.Advert;
        //                newModel.NextStatus = model.NextStatus;

        //                newModel.CampaignAdvertId = await _executers.ExecuteCommand(conn,
        //                        conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewIntoCampaignAdverts, newModel));
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return newModel;
        //}


        public async Task<CampaignProfileTimeSetting> GetProfileTimeSettingsByCampId(int id)
        {

            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            sb.Append(CreateUpdateCampaignQuery.GetProfileTimeSettingsByCampId);
            builder.AddParameters(new { Id = id });


            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignProfileTimeSetting>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }

    }
}
