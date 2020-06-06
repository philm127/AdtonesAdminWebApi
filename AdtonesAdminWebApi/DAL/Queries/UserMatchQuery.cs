using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{

    public interface IUserMatchQuery
    {
        string UpdateMediaLocation { get; }
    }

    public class UserMatchQuery : IUserMatchQuery
    {
        public string UpdateMediaLocation => "UPDATE CampaignMatches SET MEDIA_URL=@media WHERE MSCampaignProfileId=@campaignProfileId";


    }
}
