using System.Collections.Generic;

namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public class SpikePlayLengthsDao : LeveledStatsObjectDaoBase
    {
        public SpikePlayLengthsDao()
        {
            Values = new List<long>();
        }
        public List<long> Values { get; set; }
    }
}