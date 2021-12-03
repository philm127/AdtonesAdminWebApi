using System;
using System.Collections.Generic;
using System.Linq;
using AdtonesAdminWebApi.RollUpData.Services;

namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public static class DshboardSummariesDaoExtensions
    {
        public static decimal FreePlaysPercentage(this DashboardSummariesDao summary)
        {
            return summary.TotalPlays == 0 ? 0 : Math.Round(Convert.ToDecimal((decimal)summary.FreePlays / summary.TotalPlays) * 100, 2, MidpointRounding.ToEven);
        }
        public static decimal BudgetPercentage(this DashboardSummariesDao summary)
        {
            return summary.Budget == 0 ? 0 : Math.Round(Convert.ToDecimal(summary.Spend / summary.Budget) * 100, 2, MidpointRounding.ToEven);
        }

        public static decimal ConvertToDisplayCurrencyIfNeeded(this DashboardSummariesDao summary, Func<DashboardSummariesDao, decimal> valueAccessor, ICurrencyConverter converter)
        {
            if (summary.CampaignCurrencyId == converter.DisplayCurrencyId)
                return valueAccessor(summary);
            return valueAccessor(summary).ConvertIfNeeded(summary.CampaignCurrencyId, converter);
        }

        public static decimal ConvertIfNeeded(this decimal value, int fromCurrency, ICurrencyConverter converter)
        {
            if (converter != null && fromCurrency != converter.DisplayCurrencyId)
                return converter.ConvertToDisplay(value, fromCurrency);
            return value;
        }

        public static DashboardSummariesDao ConvertValuesIfNeeded(this DashboardSummariesDao summary, ICurrencyConverter converter)
        {
            return new DashboardSummariesDao
            {
                DetailLevel = summary.DetailLevel,
                OperatorId = summary.OperatorId,
                AdvertiserId = summary.AdvertiserId,
                CampaignId = summary.CampaignId,
                AdvertId = summary.AdvertId,
                CampaignCountryId = summary.CampaignCountryId,
                CampaignCurrencyId = converter.DisplayCurrencyId,
                LastUpdateDateTimeUtc = summary.LastUpdateDateTimeUtc,
                Budget = summary.Budget.ConvertIfNeeded(summary.CampaignCurrencyId, converter),
                Spend = summary.Spend.ConvertIfNeeded(summary.CampaignCurrencyId, converter),
                FundsAvailable = summary.FundsAvailable.ConvertIfNeeded(summary.CampaignCurrencyId, converter),
                AvgBid = summary.AvgBid.ConvertIfNeeded(summary.CampaignCurrencyId, converter),
                MaxBid = summary.MaxBid.ConvertIfNeeded(summary.CampaignCurrencyId, converter),
                TotalSMSCost = summary.TotalSMSCost.ConvertIfNeeded(summary.CampaignCurrencyId, converter),
                TotalEmailCost = summary.TotalEmailCost.ConvertIfNeeded(summary.CampaignCurrencyId, converter),
                TotalPlays = summary.TotalPlays,
                AvgPlayLength = summary.AvgPlayLength,
                Reach = summary.Reach,
                FreePlays = summary.FreePlays,
                MaxPlayLength = summary.MaxPlayLength,
                MoreSixSecPlays = summary.MoreSixSecPlays,
                TotalSMS = summary.TotalSMS,
                TotalEmail = summary.TotalEmail
            };
        }

        public static DashboardSummariesDao Reduce(this List<DashboardSummariesDao> source, StatsDetailLevels detailLevel, ICurrencyConverter converter)
        {
            if(source.Count == 0)
                return new DashboardSummariesDao {DetailLevel = detailLevel.ToSqlValue()};

            List<int> currenciesInvolved = source.Select(s => s.CampaignCurrencyId).Distinct().ToList();
            bool isMultiCurrency = currenciesInvolved.Count > 1;
            
            int? operatorId, advertiserId;
            switch (detailLevel)
            {
                case StatsDetailLevels.Advertiser:
                    advertiserId = source.Select(s => s.AdvertiserId).Distinct().SingleOrDefault(s => s != null);
                    operatorId = source.Select(s => s.OperatorId).Distinct().FirstOrDefault();
                    break;
                case StatsDetailLevels.Operator:
                    operatorId = source.Select(s => s.OperatorId).Distinct().SingleOrDefault(o => o != null);
                    advertiserId = null;
                    break;
                case StatsDetailLevels.Campaign:
                    if(source.Count > 1)
                        throw new ArgumentException("Cannot Reduce with DetailLevel = Campaign, because List contains more than 1 element expected.");
                    return source.FirstOrDefault()?.ConvertValuesIfNeeded(converter) ?? new DashboardSummariesDao { DetailLevel = detailLevel.ToSqlValue() };
                case StatsDetailLevels.Advert:
                    if (source.Count == 0)
                        return new DashboardSummariesDao {DetailLevel = StatsDetailLevels.Advert.ToSqlValue()};
                    if (source.Count == 1)
                        return source.First().ConvertValuesIfNeeded(converter);
                    
                    if(source.Select(s=>s.AdvertId).Distinct().Count() > 1)
                        throw new ArgumentException("Cannot Reduce with DetailLevel = Advert, because List contains more than 1 distinct advert expected.");
                    return new DashboardSummariesDao
                    {
                        DetailLevel = detailLevel.ToSqlValue(),
                        OperatorId = source.First().OperatorId,
                        AdvertiserId = source.First().AdvertiserId,
                        CampaignId = null,
                        AdvertId = source.First().AdvertId,
                        CampaignCountryId = source.Select(s => s.CampaignCountryId).Distinct().FirstOrDefault(),
                        CampaignCurrencyId = converter.DisplayCurrencyId,
                        LastUpdateDateTimeUtc = source.Max(s => s.LastUpdateDateTimeUtc),
                        Budget = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.Budget, converter)),
                        Spend = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.Spend, converter)),
                        FundsAvailable = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.FundsAvailable, converter)),
                        AvgBid = source.Where(s => s.AvgBid != 0M).Average(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.AvgBid, converter)),
                        MaxBid = source.Max(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.MaxBid, converter)),
                        TotalSMSCost = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.TotalSMSCost, converter)),
                        TotalEmailCost = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.TotalEmailCost, converter)),
                        TotalPlays = source.Sum(s => s.TotalPlays),
                        AvgPlayLength = source.Where(s => s.TotalPlays != 0M).Average(s => s.AvgPlayLength),
                        Reach = 0,
                        FreePlays = source.Sum(s => s.FreePlays),
                        MaxPlayLength = source.Max(s => s.MaxPlayLength),
                        MoreSixSecPlays = source.Sum(s => s.MoreSixSecPlays),
                        TotalSMS = source.Sum(s => s.TotalSMS),
                        TotalEmail = source.Sum(s => s.TotalEmail)
                    };
                default:
                    throw new ArgumentException("Cannot Reduce with DetailLevel = Campaign. For Campaigns: each line is already reduced. Please specify either Advertiser or Operator instead.");
            }
            if (isMultiCurrency)
            {
                return new DashboardSummariesDao
                {
                    DetailLevel = detailLevel.ToSqlValue(),
                    OperatorId = operatorId,
                    AdvertiserId = advertiserId,
                    CampaignId = null,
                    AdvertId = null,
                    CampaignCountryId = source.Select(s => s.CampaignCountryId).Distinct().FirstOrDefault(),
                    CampaignCurrencyId = converter.DisplayCurrencyId,
                    LastUpdateDateTimeUtc = source.Max(s => s.LastUpdateDateTimeUtc),
                    Budget = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v=>v.Budget, converter)),
                    Spend = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.Spend, converter)),
                    FundsAvailable = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.FundsAvailable, converter)),
                    AvgBid = source.Where(s => s.AvgBid != 0M).Average(s => s.ConvertToDisplayCurrencyIfNeeded(v=>v.AvgBid, converter)),
                    MaxBid = source.Max(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.MaxBid, converter)),
                    TotalSMSCost = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.TotalSMSCost, converter)),
                    TotalEmailCost = source.Sum(s => s.ConvertToDisplayCurrencyIfNeeded(v => v.TotalEmailCost, converter)),
                    TotalPlays = source.Sum(s => s.TotalPlays),
                    AvgPlayLength = source.Where(s => s.TotalPlays != 0M).Average(s => s.AvgPlayLength),
                    Reach = 0,
                    FreePlays = source.Sum(s => s.FreePlays),
                    MaxPlayLength = source.Max(s => s.MaxPlayLength),
                    MoreSixSecPlays = source.Sum(s => s.MoreSixSecPlays),
                    TotalSMS = source.Sum(s => s.TotalSMS),
                    TotalEmail = source.Sum(s => s.TotalEmail)
                };
            }

            var fromCurrency = currenciesInvolved[0];
            return new DashboardSummariesDao
            {
                DetailLevel = detailLevel.ToSqlValue(),
                OperatorId = operatorId,
                AdvertiserId = advertiserId,
                CampaignId = null,
                AdvertId = null,
                CampaignCountryId = source.Select(s=>s.CampaignCountryId).Distinct().FirstOrDefault(),
                CampaignCurrencyId = fromCurrency,
                LastUpdateDateTimeUtc = source.Max(s => s.LastUpdateDateTimeUtc),
                Budget = source.Sum(s => s.Budget).ConvertIfNeeded(fromCurrency, converter),
                Spend = source.Sum(s=>s.Spend).ConvertIfNeeded(fromCurrency, converter),
                FundsAvailable = source.Sum(s=>s.FundsAvailable).ConvertIfNeeded(fromCurrency, converter),
                AvgBid = source.Where(s=>s.AvgBid != 0M).Average(s=>s.AvgBid).ConvertIfNeeded(fromCurrency, converter),
                MaxBid = source.Max(s=>s.MaxBid).ConvertIfNeeded(fromCurrency, converter),
                TotalSMSCost = source.Sum(s=>s.TotalSMSCost).ConvertIfNeeded(fromCurrency, converter),
                TotalEmailCost = source.Sum(s=>s.TotalEmailCost).ConvertIfNeeded(fromCurrency, converter),
                TotalPlays = source.Sum(s=>s.TotalPlays),
                AvgPlayLength = source.Where(s=>s.TotalPlays != 0M).Average(s=>s.AvgPlayLength),
                Reach = 0,
                FreePlays = source.Sum(s=>s.FreePlays),
                MaxPlayLength = source.Max(s=>s.MaxPlayLength),
                MoreSixSecPlays = source.Sum(s=>s.MoreSixSecPlays),
                TotalSMS = source.Sum(s=>s.TotalSMS),
                TotalEmail = source.Sum(s=>s.TotalEmail)
            };
        }
    }
}