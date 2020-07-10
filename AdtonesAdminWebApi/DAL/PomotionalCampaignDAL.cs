using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class PromotionalCampaignDAL : IPromotionalCampaignDAL
    {
        private readonly IConfiguration _configuration;
        private readonly IExecutionCommand _executers;
        private readonly string _mainConnStr;
        private readonly IConnectionStringService _connService;

        public PromotionalCampaignDAL(IConfiguration configuration, IExecutionCommand executers,
            IConnectionStringService connService)
        {
            _configuration = configuration;
            _executers = executers;
            _mainConnStr = _configuration.GetConnectionString("DefaultConnection");
            _connService = connService;
        }


        public async Task<IEnumerable<string>> GetMsisdnCheckForExisting(int operatorId)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(operatorId);
            try
            {
                return await _executers.ExecuteCommand(operatorConnectionString,
                             conn => conn.Query<string>(PromotionalCampaignQuery.CheckExistingMSISDN));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> GetPromoUserBatchIdCheckForExisting(PromotionalUserFormModel model)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorId);
            try
            {
                return await _executers.ExecuteCommand(operatorConnectionString,
                             conn => conn.ExecuteScalar<bool>(PromotionalCampaignQuery.CheckIfBatchExists, new { id = model.BatchID }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> GetPromoCampaignBatchIdCheckForExisting(string batch)
        {
            try
            {
                return await _executers.ExecuteCommand(_mainConnStr,
                             conn => conn.ExecuteScalar<bool>(PromotionalCampaignQuery.CheckIfBatchInCampaignsExists, new { id = batch }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdatePromotionalCampaignStatus(IdCollectionViewModel model)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.operatorId);
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(PromotionalCampaignQuery.UpdatePromotionalCampaignStatus);
            try
            {
                builder.AddParameters(new { Id = model.id });
                builder.AddParameters(new { Status = model.status });

                var x = await _executers.ExecuteCommand(_mainConnStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql + " CampaignProfileId=@Id", select.Parameters));

                var y = await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql + " AdtoneServerPromotionalCampaignId=@Id", select.Parameters));

                return x + y;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<PromotionalCampaignResult>> GetPromoCampaignResultSet()
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(PromotionalCampaignQuery.GetPromoCampaignResultSet);
            try
            {

                return await _executers.ExecuteCommand(_mainConnStr,
                                conn => conn.Query<PromotionalCampaignResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddPromotionalCampaign(PromotionalCampaignResult model)
        {
            try
            {

                return await _executers.ExecuteCommand(_mainConnStr,
                                conn => conn.ExecuteScalar<int>(PromotionalCampaignQuery.AddPromoCampaign, model));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddPromotionalCampaignToOperator(PromotionalCampaignResult model)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorID);
            try
            {

                return await _executers.ExecuteCommand(operatorConnectionString,
                                conn => conn.ExecuteScalar<int>(PromotionalCampaignQuery.AddPromoCampaign, model));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddPromotionalAdvert(PromotionalCampaignResult model)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(PromotionalCampaignQuery.AddPromoAdvert);
            try
            {
                builder.AddParameters(new { CampaigID = model.ID });
                builder.AddParameters(new { AdvertName = model.AdvertName });
                builder.AddParameters(new { AdvertLocation = model.AdvertLocation });
                builder.AddParameters(new { AdtoneServerPromotionalAdvertId = model.AdtoneServerPromotionalCampaignId });

                return await _executers.ExecuteCommand(_mainConnStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql + " CampaignProfileId=@Id", select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddPromotionalAdvertToOperator(PromotionalCampaignResult model)
        {
            var operatorConnectionString = await _connService.GetSingleConnectionString(model.OperatorID);
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(PromotionalCampaignQuery.AddPromoAdvert);
            try
            {
                builder.AddParameters(new { CampaigID = model.ID });
                builder.AddParameters(new { AdvertName = model.AdvertName });
                builder.AddParameters(new { AdvertLocation = model.AdvertLocation });
                builder.AddParameters(new { AdtoneServerPromotionalAdvertId = model.AdtoneServerPromotionalCampaignId });

                return await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql + " CampaignProfileId=@Id", select.Parameters));
            }
            catch
            {
                throw;
            }
        }

    }
}
