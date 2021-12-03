namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public class LeveledStatsObjectDaoBase 
    {
        public string DetailLevel { get; set; }
        public StatsDetailLevels Level => DetailLevel.ToLevel();
        public int? OperatorId { get; set; }
        public int? AdvertiserId { get; set; }
        public int? CampaignId { get; set; }
    }
}