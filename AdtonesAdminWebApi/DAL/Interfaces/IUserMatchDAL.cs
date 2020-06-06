using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserMatchDAL
    {
        Task<int> UpdateMediaLocation(string command, string conn, string media, int id);
        Task PrematchProcessForCampaign(int campaignId, string conn);
    }
}
