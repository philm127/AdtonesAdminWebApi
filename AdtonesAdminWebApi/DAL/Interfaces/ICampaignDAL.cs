using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICampaignDAL
    {
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(string command, int id=0);
        Task<IEnumerable<PromotionalCampaignResult>> GetPromoCampaignResultSet(string command);
        Task<IEnumerable<CampaignCreditResult>> GetCampaignCreditResultSet(string command);
    }
}
