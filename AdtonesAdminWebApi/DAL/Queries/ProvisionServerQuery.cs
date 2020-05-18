

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IProvisionServerQuery
    {
        string CheckExistingMSISDN { get; }
        string CheckIfBatchExists { get; }
    }


    public class ProvisionServerQuery : IProvisionServerQuery
    {
        public string CheckExistingMSISDN => @"SELECT DISTINCT(msisdn) 
                                                FROM
                                                    (SELECT DISTINCT(msisdn) AS msisdn FROM PromotionalUsers
                                                    UNION ALL
                                                    SELECT DISTINCT(msisdn) AS msisdn FROM UserProfile) t";


        public string CheckIfBatchExists => @"SELECT COUNT(1) FROM PromotionalUsers WHERE BatchId=@id";

    }
}
