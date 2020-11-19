using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class PromotionalCampaignDAL : BaseDAL, IPromotionalCampaignDAL
    {

        public PromotionalCampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<string>> GetMsisdnCheckForExisting(int operatorId)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(operatorId);
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
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(model.OperatorId);
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


        public async Task<bool> GetPromoCampaignBatchIdCheckForExisting(PromotionalCampaignResult model)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<bool>(PromotionalCampaignQuery.CheckIfBatchInCampaignsExists, new { id = model.BatchID, op = model.OperatorId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdatePromotionalCampaignStatus(IdCollectionViewModel model)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(model.operatorId);
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(PromotionalCampaignQuery.UpdatePromotionalCampaignStatus);
            try
            {
                builder.AddParameters(new { Id = model.id });
                builder.AddParameters(new { Status = model.status });

                var x = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql + " ID=@Id", select.Parameters));

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
            builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
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

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(PromotionalCampaignQuery.AddPromoCampaign, model));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddPromotionalCampaignToOperator(PromotionalCampaignResult model)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(model.OperatorId);
            var operatorId = await _connService.GetOperatorIdFromAdtoneId(model.OperatorId);
            model.OperatorId = operatorId;
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
                builder.AddParameters(new { CampaignID = model.ID });
                builder.AddParameters(new { AdvertName = model.AdvertName });
                builder.AddParameters(new { AdvertLocation = model.AdvertLocation });
                builder.AddParameters(new { AdtoneServerPromotionalAdvertId = model.AdtoneServerPromotionalCampaignId });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddPromotionalAdvertToOperator(PromotionalCampaignResult model)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(model.OperatorId);
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(PromotionalCampaignQuery.AddPromoAdvert);
            try
            {
                builder.AddParameters(new { CampaignID = model.ID });
                builder.AddParameters(new { AdvertName = model.AdvertName });
                builder.AddParameters(new { AdvertLocation = model.AdvertLocation });
                builder.AddParameters(new { AdtoneServerPromotionalAdvertId = model.AdtoneServerPromotionalCampaignId });

                return await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetPromoBatchIdList(int id)
        {
            try
            {
                var operatorConnectionString = await _connService.GetConnectionStringByOperator(id);
                return await _executers.ExecuteCommand(operatorConnectionString,
                                conn => conn.Query<SharedSelectListViewModel>(PromotionalCampaignQuery.GetBatchIdForPromocampaign));
            }
            catch
            {
                throw;
            }
        }

    }
}
