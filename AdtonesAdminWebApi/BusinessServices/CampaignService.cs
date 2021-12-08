
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

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CampaignService : ICampaignService
    {
        private readonly IConfiguration _configuration;

        public IConnectionStringService _connService { get; }
        // private readonly ISaveFiles _saveFile;
        ReturnResult result = new ReturnResult();
        private readonly ICurrencyDAL _currencyConversion;
        IHttpContextAccessor _httpAccessor;
        private readonly ICampaignDAL _campDAL;
        private readonly ICampaignMatchDAL _campMatchDAL;
        private readonly ILoggingService _logServ;
        const string PageName = "CampaignService";

        public CampaignService(IConfiguration configuration, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                ICampaignDAL campDAL, ILoggingService logServ, ICampaignMatchDAL campMatchDAL,
                                ICurrencyDAL currencyConversion) //ISaveFiles saveFile)

        {
            _configuration = configuration;
            _connService = connService;
           // _saveFile = saveFile;
            _httpAccessor = httpAccessor;
            _campDAL = campDAL;
            _campMatchDAL = campMatchDAL;
            _logServ = logServ;
            _currencyConversion = currencyConversion;
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
                // Need to do this to get OperatorId
                CampaignProfileDto _campProfile = await _campDAL.GetCampaignProfileDetail(model.id);
                bool exists = false;

                exists = await _campDAL.CheckCampaignBillingExists(model.id);

                if (!exists)
                    _campProfile.Status = (int)Enums.CampaignStatus.InsufficientFunds;
                else
                    _campProfile.Status = model.status;

                result.body = await _campDAL.ChangeCampaignProfileStatus(_campProfile);
                var x = await _campDAL.ChangeCampaignProfileStatusOperator(_campProfile);
                var y = await _campMatchDAL.UpdateCampaignMatch(_campProfile.CampaignProfileId, _campProfile.OperatorId, _campProfile.Status);


            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateStatus";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Changed when advert status changed, called by Adverts changed status
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns>True or False</returns>
        public async Task<bool> ChangeCampaignStatus(int campaignId)
        {
            try
            {
                CampaignProfileDto _campProfile = await _campDAL.GetCampaignProfileDetail(campaignId);

                if (_campProfile != null)
                {
                    bool exists = false;

                    exists = await _campDAL.CheckCampaignBillingExists(campaignId);
                    var roleId = _httpAccessor.GetRoleIdFromJWT();
                    if (exists || roleId == (int)Enums.UserRole.ProfileAdmin)
                    {
                        _campProfile.Status = CheckStartDateOfCampaign(_campProfile);

                    }
                    else
                    {
                        _campProfile.Status = (int)Enums.CampaignStatus.InsufficientFunds;
                    }

                    var y = await _campDAL.ChangeCampaignProfileStatus( _campProfile);
                    var x = await _campDAL.ChangeCampaignProfileStatusOperator( _campProfile);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ChangeCampaignStatus";
                await _logServ.LogError();
                
                return false;
            }
            return true;
        }


        private static int CheckStartDateOfCampaign(CampaignProfileDto _campProfile)
        {
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
