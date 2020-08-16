using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{

    public static class UserMatchQuery
    {
        public static string UpdateMediaLocation => "UPDATE CampaignMatches SET MEDIA_URL=@media WHERE MSCampaignProfileId=@campaignProfileId";

        public static string GetBudgetUpdateAmount => "SELECT MaxHourlyBudget, MaxBid FROM CampaignProfile WHERE CampaignProfileId=@Id";

        public static string UpdateBucketAmount => "Update CampaignProfile SET BucketCount=@BucketCount WHERE CampaignProfileId=@Id";
    }
}
