using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ILoginDAL
    {
        Task<User> GetLoginUser(string command, User userModel);
        Task<int> UpdateUserLockout(string command, User userModel);

    }
}
