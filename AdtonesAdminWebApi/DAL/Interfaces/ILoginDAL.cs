using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ILoginDAL
    {
        Task<User> GetLoginUser(User userModel);
        Task<int> UpdateUserLockout(User userModel);
        Task<int> UpdatePassword(User userModel);
        Task<int> UpdateLastLoggedIn(int userId);
    }
}
