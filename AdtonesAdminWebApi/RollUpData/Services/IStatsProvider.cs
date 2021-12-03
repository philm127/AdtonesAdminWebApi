using AdtonesAdminWebApi.RollUpData.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.RollUpData.Services
{
    public interface IStatsProvider
    {
        Task<ConsolidatedStatsDao> GetConsolidatedStatsAsync(StatsDetailLevels detailLevel, int entityId, ICurrencyConverter converter);
        Task<List<CampaignUserMatches>> GetCampaignUserMatchCountAsync(Int32[] CampaignProfileIds);
    }
}
