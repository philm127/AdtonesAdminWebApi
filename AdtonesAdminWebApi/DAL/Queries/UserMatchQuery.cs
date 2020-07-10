using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{

    public static class UserMatchQuery
    {
        public static string UpdateMediaLocation => "UPDATE CampaignMatches SET MEDIA_URL=@media WHERE MSCampaignProfileId=@campaignProfileId";


    }
}
