

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ICheckExistsQuery
    {
        string CheckAreaExists { get; }
        string CheckCampaignBillingExists { get; }

    }


    public class CheckExistsQuery : ICheckExistsQuery
    {
        public string CheckAreaExists => @"SELECT COUNT(1) FROM Areas WHERE LOWER(AreaName) = @areaname AND CountryId=@countryId";


        public string CheckCampaignBillingExists => @"SELECT COUNT(1) FROM Billing WHERE CampaignProfileId=@Id;";

    }
}
