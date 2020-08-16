using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IRewardDAL
    {
        Task<IEnumerable<RewardResult>> LoadRewardResultSet();
        Task<RewardResult> GetRewardById(int id);
        Task<int> DeleteRewardById(int id);
        Task<int> UpdateReward(RewardResult model);
        Task<int> AddReward(RewardResult model);

    }
}
