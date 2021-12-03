using Domain.Model;
using Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface ILoginDAL
    {
        Task<User> GetLoginUser(User userModel);
        Task<int> UpdateUserLockout(User userModel);
        Task<int> UpdatePassword(User userModel);
        Task<int> UpdateLastLoggedIn(int userId);
    }
}
