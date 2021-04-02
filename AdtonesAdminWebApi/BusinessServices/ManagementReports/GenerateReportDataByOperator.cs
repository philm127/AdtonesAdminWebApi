using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.BusinessServices.ManagementReports.ManagementReportParts;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.ManagementReports
{
    public class GenerateReportDataByOperator : IGenerateReportDataByOperator
    {
        private readonly IConnectionStringService _connService;
        private readonly IConfiguration _config;
        private readonly ILoggingService _logServ;
        private readonly IRewards _iRewards;
        private readonly ITotalPlays _iPlays;
        private readonly ITotalCampaigns _iCampaigns;
        private readonly ITotalAdverts _iAdverts;
        private readonly ITotalListened _iListen;
        private readonly ITotalSubscribers _iSubscribers;
        private readonly IAmountOfCredit _iCredit;
        private readonly IAmountPaid _iPaid;
        private readonly IAmountSpentInPeriod _iSpent;
        private readonly ITotalSpent _iTotSpend;
        const string PageName = "GenerateReportDataByOperator";


        public GenerateReportDataByOperator(IConnectionStringService connService, IConfiguration config, ILoggingService logServ,
                                            IRewards iRewards,
                                            ITotalPlays iPlays,
                                            ITotalCampaigns iCampaigns,
                                            ITotalAdverts iAdverts,
                                            ITotalListened iListen,
                                            ITotalSubscribers iSubscribers,
                                            IAmountOfCredit iCredit,
                                            IAmountPaid iPaid,
                                            IAmountSpentInPeriod iSpent,
                                            ITotalSpent iTotSpend
                                            )
        {
            _connService = connService;
            _config = config;

            _logServ = logServ;
            _iRewards = iRewards;
            _iPlays = iPlays;
            _iCampaigns = iCampaigns;
            _iAdverts = iAdverts;
            _iListen = iListen;
            _iSubscribers = iSubscribers;
            _iCredit = iCredit;
            _iPaid = iPaid;
            _iSpent = iSpent;
            _iTotSpend = iTotSpend;
        }

        public async Task<ManagementReportModel> GetReportDataByOperator(ManagementReportsSearch search, int op)
        {
            ManagementReportModel model = new ManagementReportModel();

            try
            {
                string constring = string.Empty;
                string mainConstring = _config.GetConnectionString("DefaultConnection");
                constring = await _connService.GetConnectionStringByOperator(op);

                if (constring != null && constring.Length > 10)
                {

                    // Separated this out as conversion likely to take more time than the initial fetch.
                    /// Spend & Credit
                    Task<TotalCostCredit> amtSpent = _iSpent.GetTotalAmountSpentInPeriod(search, op, constring);
                    Task<TotalCostCredit> amtCredit = _iCredit.GetTotalAmountOfCredit(search, op, mainConstring);
                    Task<TotalCostCredit> amtPaid = _iPaid.GetTotalAmountPaid(search, op, mainConstring);
                    Task<TotalCostCredit> totCost = _iTotSpend.GetTotalTotalSpent(search, op, constring);

                    /// Subscribers
                    Task<ManRepUsers> totUser = _iSubscribers.GetData(search, op, mainConstring);
                    Task<TwoDigitsManRep> totListen = _iListen.GetData(search, op, mainConstring);

                    /// Campaigns & Adverts
                    Task<TwoDigitsManRep> totads = _iAdverts.GetData(search, op, mainConstring);
                    Task<TwoDigitsManRep> totCam = _iCampaigns.GetData(search, op, mainConstring);

                    /// Plays
                    Task<CampaignTableManReport> totPlays = _iPlays.GetPlayData(search, op, constring);

                    Task<RewardsManModel> totRewards = _iRewards.GetRewardData(search, op, constring);


                    await Task.WhenAll(
                        amtSpent,
                        amtPaid,
                        totCost,
                        totUser,
                        totListen,
                        totads,
                        totCam,
                        totPlays,
                        amtCredit,
                        totRewards);

                    var currency = await _iCredit.GetCurrencyUsingCurrencyIdAsync(search.currency);

                    // var eqOver6 = totPlays.Result;
                    var numPlay = totPlays.Result;
                    var usrs = totUser.Result;
                    var camps = totCam.Result;
                    var ads = totads.Result;
                    var listen = totListen.Result;
                    var rewardsTot = totRewards.Result;
                    /// Users
                    model.TotalUsers = usrs.TotalUsers;
                    model.TotalListened = listen.TotalItem;
                    model.TotalRemovedUser = usrs.TotalRemovedUser;
                    model.AddedUsers = usrs.AddedUsers;
                    model.NumListened = listen.NumItem;

                    /// Campaigns & Adverts
                    model.TotalAdverts = ads.TotalItem;
                    model.AdvertsProvisioned = ads.NumItem;
                    model.TotalCampaigns = camps.TotalItem;
                    model.CampaignsAdded = camps.NumItem;
                    model.TotalCancelled = numPlay.TotCancelled;
                    model.NumCancelled = numPlay.NumCancelled;
                    /// Plays
                    model.TotalEmail = numPlay.TotOfEmail;
                    model.TotalSMS = numPlay.TotOfSMS;
                    //model.TotalPlays = eqOver6.TotalPlays;
                    model.TotalPlayLength = numPlay.TotPlaylength;
                    model.Total6Over = numPlay.TotOfPlaySixOver;
                    model.TotalUnder6 = numPlay.TotOfPlayUnderSix;

                    model.NumEmail = numPlay.NumOfEmail;
                    model.NumSMS = numPlay.NumOfSMS;
                    //model.TotalPlays = eqOver6.TotalPlays;
                    model.PeriodPlayLength = numPlay.Playlength;
                    model.Num6Over = numPlay.NumOfPlaySixOver;
                    model.NumUnder6 = numPlay.NumOfPlayUnderSix;
                    // Total Plays
                    model.TotalPlays = numPlay.TotalPlays;
                    model.NumPlays = numPlay.Plays;

                    /// Credit & Spend
                    model.TotalCredit = (int)amtCredit.Result.TotalCredit;
                    model.TotalSpend = (int)totCost.Result.TotalSpend;
                    model.AmountSpent = (int)amtSpent.Result.TotalSpend;
                    model.AmountCredit = (int)amtPaid.Result.TotalCredit;
                    model.CurrencyCode = _iCredit.GetCurrencySymbol(currency.CurrencyCode);

                    /// Rewards
                    model.TotalRewards = rewardsTot.IsRewardReceivedTot;
                    model.TotRewardUsers = rewardsTot.UserProfileIdTot;
                    model.NumRewards = rewardsTot.IsRewardReceivedNum;
                    model.NumRewardUsers = rewardsTot.UserProfileIdNum;
                }
                else
                {
                    model = null;
                }

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetManreportAsync";
                await _logServ.LogError();

            }
            return model;
        }
    }
}
