using System;

namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public static class StatsPeriodicTypesExtentions
    {
        public static string ToSqlValue(this StatsPeriodicTypes level)
        {
            return level == StatsPeriodicTypes.Weekly ? "W" :
                level == StatsPeriodicTypes.Daily ? "D" :
                level == StatsPeriodicTypes.Hourly ? "H" : string.Empty;
        }

        public static StatsPeriodicTypes ToPeriodType(this string level)
        {
            return "W".Equals(level, StringComparison.InvariantCulture) ? StatsPeriodicTypes.Weekly :
                "D".Equals(level, StringComparison.InvariantCulture) ? StatsPeriodicTypes.Daily :
                "H".Equals(level, StringComparison.InvariantCulture) ? StatsPeriodicTypes.Hourly :
                StatsPeriodicTypes.Undefined;
        }
    }
}