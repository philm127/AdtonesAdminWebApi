using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserCreditDAL
    {
        Task<IEnumerable<UserCreditResult>> LoadUserCreditResultSet();
        Task<int> AddUserCredit(UserCreditFormModel _creditmodel);
        Task<int> UpdateUserCredit(UserCreditFormModel _creditmodel);
    }
}
