using System;

namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public static class StatsDetailLevelExtentions
    {
        public static string ToSqlValue(this StatsDetailLevels level)
        {
            return level == StatsDetailLevels.Campaign ? "C" :
                level == StatsDetailLevels.Advertiser ? "A" :
                level == StatsDetailLevels.Operator ? "O" : string.Empty;
        }

        public static StatsDetailLevels ToLevel(this string level)
        {
            return "C".Equals(level, StringComparison.InvariantCulture) ? StatsDetailLevels.Campaign :
                "A".Equals(level, StringComparison.InvariantCulture) ? StatsDetailLevels.Advertiser :
                "O".Equals(level, StringComparison.InvariantCulture) ? StatsDetailLevels.Operator :
                StatsDetailLevels.Undefined;
        }
    }
}