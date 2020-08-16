using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAdvertiserCreditDAL
    {
        
        Task<int> AddUserCredit(AdvertiserCreditFormModel _creditmodel);
        Task<int> UpdateUserCredit(AdvertiserCreditFormModel _creditmodel);
        Task<AdvertiserCreditFormModel> GetUserCreditDetail(int id);
        Task<decimal> GetCreditBalance(int id);
        Task<bool> CheckUserCreditExist(int userId);

        Task<int> UpdateCampaignCredit(CampaignCreditResult model);
        Task<int> InsertCampaignCredit(CampaignCreditResult model);
    }
}
