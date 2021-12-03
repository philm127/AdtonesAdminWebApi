using System.Collections.Generic;
using System.Linq;

namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public class PeriodicIntegerData : LeveledStatsObjectDaoBase
    {
        public PeriodicIntegerData()
        {
            Values = new List<PeriodicIntegerValue>();
        }
        public List<PeriodicIntegerValue> Values { get; set; }

        public PeriodicIntegerData Reduce()
        {
            int weekCounts = Values.Count(v => v.PeriodType == StatsPeriodicTypes.Weekly);
            if(weekCounts>2)
                return new PeriodicIntegerData
                {
                    AdvertiserId = AdvertiserId, 
                    CampaignId = CampaignId, 
                    OperatorId = OperatorId, 
                    DetailLevel = DetailLevel, 
                    Values = Values.Where(v=>v.PeriodType == StatsPeriodicTypes.Weekly).ToList()
                };
            if (Values.Count(v => v.PeriodType == StatsPeriodicTypes.Daily) > 2)
            {
                return new PeriodicIntegerData
                {
                    AdvertiserId = AdvertiserId,
                    CampaignId = CampaignId,
                    OperatorId = OperatorId,
                    DetailLevel = DetailLevel,
                    Values = Values.Where(v => v.PeriodType == StatsPeriodicTypes.Daily).ToList()
                };
            }
            return new PeriodicIntegerData
            {
                AdvertiserId = AdvertiserId,
                CampaignId = CampaignId,
                OperatorId = OperatorId,
                DetailLevel = DetailLevel,
                Values = Values.Where(v => v.PeriodType == StatsPeriodicTypes.Hourly).ToList()
            };
        }
    }
}