using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ISharedSelectListsDAL
    {
        // Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectList<T>(string sql, dynamic model, int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetSelectList(string sql, int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectListById(string command,int id=0);
        Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectList(string command);
        Task<User> GetUserById(string sql, int id);
    }
}
