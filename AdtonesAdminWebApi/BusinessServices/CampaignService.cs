
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using AdtonesAdminWebApi.ViewModels.DTOs;
using DocumentFormat.OpenXml.Spreadsheet;
using Dapper;
using Microsoft.CodeAnalysis.Editing;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CampaignService : ICampaignService
    {
        private readonly IConfiguration _configuration;

        public IConnectionStringService _connService { get; }
        // private readonly ISaveFiles _saveFile;
        ReturnResult result = new ReturnResult();
        private readonly ICurrencyDAL _currencyConversion;
        private readonly ISalesManagementService _salesService;
        IHttpContextAccessor _httpAccessor;
        private readonly ICampaignDAL _campDAL;
        private readonly ICampaignMatchDAL _campMatchDAL;
        private readonly ILoggingService _logServ;
        const string PageName = "CampaignService";

        public CampaignService(IConfiguration configuration, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                ICampaignDAL campDAL, ILoggingService logServ, ICampaignMatchDAL campMatchDAL,
                                ICurrencyDAL currencyConversion,
                                ISalesManagementService salesService) //ISaveFiles saveFile)

        {
            _configuration = configuration;
            _connService = connService;
           // _saveFile = saveFile;
            _httpAccessor = httpAccessor;
            _campDAL = campDAL;
            _campMatchDAL = campMatchDAL;
            _logServ = logServ;
            _currencyConversion = currencyConversion;
            _salesService = salesService;
        }

        public async Task<ReturnResult> GetAdminOpAdminCampaignList()
        {
            try
            {
                result.body = await _campDAL.GetAdminOpAdminCampaignResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CampaignService - GetAdminOpAdminCampaignList";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }

        /// <summary>
        /// Use the Sals Id to creat an array of Advertiser,(UserId) from to match with
        /// AdtoneServerUserId on 103
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ReturnResult> GetSalesCampaignList(int id)
        {
            IEnumerable<int> adList = null;
            if (id > 0)
            {
                adList = await _salesService.GetAdvertiserIdsBySalesExecList(id);
            }

            Dictionary<int, SalesExecDetails> salesDict = await _salesService.GetSalesExecDictDetails();
            List<CampaignAdminResult> campaigns = null;
            try
            {
                var campaignResult = await _campDAL.GetCampaignResultSetBySalesExec(adList);
                campaigns = campaignResult.ToList();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetsalesCampaignList";
                await _logServ.LogError();

                result.result = 0;
            }

            var campaignSalesData = new List<CampaignAdminResult>();

            foreach (var campaign in campaigns)
            {
                if (salesDict.ContainsKey(campaign.AdtoneServerUserId))
                {
                    SalesExecDetails salesData = salesDict[campaign.AdtoneServerUserId];
                    campaign.sUserId = salesData.UserId;
                    campaign.SalesExec = salesData.FullName;
                }
                else
                    campaign.SalesExec = "UnAllocated";


                campaignSalesData.Add(campaign);
            }

            result.body = campaignSalesData;


            return result;
        }


        public async Task<ReturnResult> GetCampaignById(int id)
        {
            CampaignAdminResult campaign = new CampaignAdminResult();
            try
            {
                campaign = await _campDAL.GetCampaignResultSetById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCampaignById";
                await _logServ.LogError();

                result.result = 0;
            }
            var roleId = _httpAccessor.GetRoleIdFromJWT();
            if (roleId == (int)Enums.UserRole.SalesManager)
            {
                Dictionary<int, SalesExecDetails> salesDict = await _salesService.GetSalesExecDictDetails();

                if (salesDict.ContainsKey(campaign.AdtoneServerUserId))
                {
                    SalesExecDetails salesData = salesDict[campaign.AdtoneServerUserId];
                    campaign.sUserId = salesData.UserId;
                    campaign.SalesExec = salesData.FullName;
                }
                else
                    campaign.SalesExec = "UnAllocated";
            }
            List<CampaignAdminResult> campaignMatch = new List<CampaignAdminResult>();
            campaignMatch.Add(campaign);
            result.body = campaignMatch;
            return result;
        }

        /// <summary>
        /// Changed for the actual campaign directly
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                result.body = await ChangeUpdateStatus(model);
            }
            catch (Exception ex)
            {
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Changed when advert status changed, called by Adverts changed status
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns>True or False</returns>
        public async Task<bool> ChangeCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                var result = await ChangeUpdateStatus(model);

                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private async Task<int> ChangeUpdateStatus(IdCollectionViewModel model)
        {

            try
            {
                bool exists = false;
                var status = 0;
                var operatorId = (int)Enums.OperatorTableId.Safaricom;

                exists = await _campDAL.CheckCampaignBillingExists(model.id);
                if (!exists)
                {
                    status = (int)Enums.CampaignStatus.InsufficientFunds;
                }
                else if (model.status == 0)
                {
                    var roleId = _httpAccessor.GetRoleIdFromJWT();
                    if (exists || roleId == (int)Enums.UserRole.ProfileAdmin)
                    {
                        status = await CheckStartDateOfCampaign(model.id);
                    }
                }
                else
                    status = model.status;

                var res = await _campDAL.ChangeCampaignProfileStatus(model.id, status);
                var x = await _campDAL.ChangeCampaignProfileStatusOperator(model.id, model.status, operatorId);
                if (model.status > 0)
                {
                    var y = await _campMatchDAL.UpdateCampaignMatch(model.id, operatorId, model.status);
                }
                return res;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = $"ChangeUpdateStatus - CampId:{model.id}";
                await _logServ.LogError();
                return 0;
            }
        }

        private async Task<int> CheckStartDateOfCampaign(int campaignId)
        {
            var _campProfile = await _campDAL.GetCampaignProfileDetail(campaignId);
            int status = 0;
            if (_campProfile.StartDate == null && _campProfile.EndDate == null)
            {
               status = (int)Enums.CampaignStatus.Play;
            }
            else
            {
                if (_campProfile.StartDate != null)
                {
                    if (_campProfile.StartDate == DateTime.Now.Date)
                    {
                        status = (int)Enums.CampaignStatus.Play;
                    }
                    else
                    {
                        status = (int)Enums.CampaignStatus.Planned;
                    }
                }
                else
                {
                    status = (int)Enums.CampaignStatus.Planned;
                }
            }
            return status;
        }


        
        public async Task<ReturnResult> GetAdvertiserCamapaignTable()
        {
            //var efmvcUser = _httpAccessor.GetUserIdFromJWT();
            //CurrencySymbol currencySymbol = new CurrencySymbol();

            //var consolidatedStats = await _statsProvider.GetConsolidatedStatsAsync(StatsDetailLevels.Advertiser,
            //    efmvcUser, _currencyConv);

            //var campaigns = await _campDAL.GetCampaignTableForAdvertiser(efmvcUser, consolidatedStats);// _profileRepository.AsQueryable().Where(c => c.UserId == efmvcUser.UserId)
            //Type type = campaigns.GetType();
            //PropertyInfo[] fields = type.GetProperties();
            //foreach (var field in fields)
            //{
            //    string name = field.Name;
            //    var temp = field.GetValue(obj, null);
            //    Console.WriteLine(name + "  " + temp);
            //}
            //var currencyConv = await _currencyConversion.GetDisplayCurrencyCodeForUserAsync(efmvcUser);
            //var resultUserMatches = await _statsProvider.GetCampaignUserMatchCountAsync(campaigns.Select(x => x.Campaign.CampaignProfileId).ToArray());
            
            //var data = joined.Select(item => new CampaignProfileResult
            //{
            //    CampaignName = item.Campaign.CampaignName,
            //    CampaignProfileId = item.Campaign.CampaignProfileId,
            //    Status = item.Campaign.Status,
            //    TotalBudget = item.Summary.Budget,
            //    totalaveragebid = item.Summary.AvgBid,
            //    totalspend = item.Summary.Spend,
            //    finaltotalplays = (int)item.Summary.MoreSixSecPlays,
            //    advertname = item.Advert?.AdvertName ?? "-",
            //    AdvertId = item.Advert?.AdvertId ?? 0,
            //    FundsAvailable = item.Summary.FundsAvailable,
            //    ClientName = item.Campaign.ClientId == 0 ? "-" : item.Campaign.ClientId == null ? "-" : item.Campaign.Client.Name,
            //    ClientId = item.Campaign.ClientId,
            //    IsAdminApproval = item.Campaign.IsAdminApproval,
            //    CurrencyCode = currencySymbol.GetCurrencySymbolByCurrencyCode(currencyConv.CurrencyCode),
            //    CountryId = currencyConv.CountryId,
            //    Reach = (int)item.Summary.Reach,
            //    CurrencyId = currencyConv.CurrencyId,
            //    UserMatchedStatus = resultUserMatches.FirstOrDefault(x => x.CampaignProfileId == item.Campaign.CampaignProfileId)?.MatchedUsers > 0 ? 1 : 0,
            //    NumUsersMatched = resultUserMatches.FirstOrDefault(x => x.CampaignProfileId == item.Campaign.CampaignProfileId)?.MatchedUsers ?? 0,
            //}).ToList();

            //result.body = data;
            return result;
        }
    }
}
