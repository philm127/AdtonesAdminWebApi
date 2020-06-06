using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.UserMatchServices
{
    public class UserMatchTableProcess : IUserMatchInterface
    {
        public void UpdateMediaFileLocation(int campaignProfileId, string mediaUrl, string conn)
        {
            //var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaignProfileId).FirstOrDefault();
            //if (campaignmatch != null)
            //{
            //    campaignmatch.MEDIA_URL = mediaUrl;
            //    SQLServerEntities.SaveChanges();
            //}
        }
    }
}
