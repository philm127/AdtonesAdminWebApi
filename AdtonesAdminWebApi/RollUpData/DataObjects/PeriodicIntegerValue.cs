namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public class PeriodicIntegerValue
    {
        public long StatId { get; set; }
        public string PeriodString { get; set; }
        public StatsPeriodicTypes PeriodType => PeriodString.ToPeriodType();
        public int PeriodName { get; set; }
        public long Value { get; set; }
    }
}