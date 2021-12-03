using System.Collections.Generic;

namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public class ConsolidatedStatsDao : LeveledStatsObjectDaoBase
    {
        public List<DashboardSummariesDao> Dashboard { get; set; }
        public DashboardSummariesDao DashboardReduced { get; set; }
        public SpikePlayLengthsDao SpikeLengths { get; set; }
        public ReachDao Reach { get; set; }
        public PeriodicIntegerData PlaysByPeriods { get; set; }
    }
}