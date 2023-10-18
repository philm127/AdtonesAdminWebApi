using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserCreditDAL
    {
        Task<int> AddUserCredit(AdvertiserCreditFormCommand _creditmodel);
        Task<int> UpdateUserCredit(int id, decimal amt);
        Task<UserCreditDetailsDto> GetUserCreditDetail(int id);
        Task<bool> CheckUserCreditExist(int userId);
        Task<decimal> GetAvailableCredit(int userId);
    }
}
