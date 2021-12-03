using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IUserCreditService
    {
        Task<ReturnResult> AddUserCredit(AdvertiserCreditFormCommand _usercredit);
        Task<ReturnResult> AddPaymentToUserCredit(AdvertiserCreditFormCommand model);
        Task<UserCreditDetailsDto> GetUserCreditDetail(int id);
    }
}
