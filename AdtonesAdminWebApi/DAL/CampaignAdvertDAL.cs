using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class CampaignAdvertDAL : BaseDAL, ICampaignAdvertDAL
    {

        public CampaignAdvertDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }

        /// <summary>
        /// Insert Into CampaignAdverts table
        /// </summary>
        /// <param name="model"></param>
        /// <param name="operatorId"></param>
        /// <param name="provAdId"></param>
        /// <returns></returns>
        public async Task<CampaignAdvertFormModel> CreateNewCampaignAdvert(CampaignAdvertFormModel model, int operatorId, int provAdId)
        {
            string InsertNewIntoCampaignAdverts = @"INSERT INTO CampaignAdverts(CampaignProfileId, AdvertId, NextStatus, 
                                                                        AdtoneServerCampaignAdvertId)
                                                               VALUES(@CampaignProfileId, @AdvertId, @NextStatus,@AdtoneServerCampaignAdvertId);
                                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {

                model.AdtoneServerCampaignAdvertId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(InsertNewIntoCampaignAdverts, model));

                return await CreateNewOperatorCampaignAdvert(model, operatorId, provAdId);
            }
            catch
            {
                throw;
            }
        }


        private async Task<CampaignAdvertFormModel> CreateNewOperatorCampaignAdvert(CampaignAdvertFormModel model, int operatorId, int provAdId)
        {
            string InsertNewIntoCampaignAdverts = @"INSERT INTO CampaignAdverts(CampaignProfileId, AdvertId, NextStatus, 
                                                                        AdtoneServerCampaignAdvertId)
                                                               VALUES(@CampaignProfileId, @AdvertId, @NextStatus,@AdtoneServerCampaignAdvertId);
                                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var newModel = new CampaignAdvertFormModel();
            try
            {
                if (model.AdtoneServerCampaignAdvertId != null && model.AdtoneServerCampaignAdvertId > 0)
                {
                    var conn = await _connService.GetConnectionStringByOperator(operatorId);
                    if (conn != null && conn != "")
                    {
                        newModel.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneId(model.CampaignProfileId, operatorId);
                        newModel.AdvertId = provAdId;
                        newModel.AdtoneServerCampaignAdvertId = model.AdtoneServerCampaignAdvertId;
                        newModel.Advert = model.Advert;
                        newModel.NextStatus = model.NextStatus;

                        newModel.CampaignAdvertId = await _executers.ExecuteCommand(conn,
                                conn => conn.ExecuteScalar<int>(InsertNewIntoCampaignAdverts, newModel));
                    }
                }
            }
            catch
            {
                throw;
            }

            return newModel;
        }


        public async Task<CampaignAdvertFormModel> CreateOnUpdateCampaignAdvert(CampaignAdvertFormModel model, int operatorId)
        {
            var campAdId = 0;
            var opAdId = 0;
            string checkMain = "SELECT CampaignAdvertId FROM CampaignAdverts WHERE AdvertId=@id";
            string checkOpAdId = "SELECT AdvertId FROM CampaignAdverts WHERE AdtoneServerCampaignAdvertId=@id";

            opAdId = await _connService.GetAdvertIdFromAdtoneId(model.AdvertId, operatorId);
            var conn = await _connService.GetConnectionStringByOperator(operatorId);

            var newModel = new CampaignAdvertFormModel();
            try
            {
                campAdId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(checkMain, new { id = model.AdvertId }));

                if (campAdId == 0)
                {
                    return await CreateNewCampaignAdvert(model, operatorId, opAdId);
                }
                else
                {
                    var opCampAdId = await _executers.ExecuteCommand(conn,
                                conn => conn.ExecuteScalar<int>(checkOpAdId, new { id = campAdId }));

                    if (opCampAdId == 0)
                    {
                        return await CreateNewOperatorCampaignAdvert(model, operatorId, opAdId);
                    }
                }
            }
            catch
            {
                throw;
            }

            return newModel;
        }

        public async Task<int> GetCampaignIdByAdvertId(int advertId)
        {
            string checkMain = "SELECT CampaignProfileId FROM CampaignAdverts WHERE AdvertId=@id";
            
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(checkMain, new { id = advertId }));

            }
            catch
            {
                throw;
            }

        }

        public async Task<int> GetAdvertIdByCampaignId(int campaignId)
        {
            string checkMain = "SELECT AdvertId FROM CampaignAdverts WHERE CampaignProfileId=@id";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(checkMain, new { id = campaignId }));

            }
            catch
            {
                throw;
            }

        }


    }
}
