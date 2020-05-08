using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IRewardsService
    {
        Task<ReturnResult> LoadRewardsDataTable();
        Task<ReturnResult> GetReward(IdCollectionViewModel model);
        Task<ReturnResult> AddReward(RewardResult model);
        Task<ReturnResult> UpdateReward(RewardResult model);
    }
}
