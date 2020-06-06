using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAdvertDAL
    {
        Task<IEnumerable<UserAdvertResult>> GetAdvertResultSet(string command, int id=0);
        Task<IEnumerable<AdvertCategoryResult>> GetAdvertCategoryList(string command);
        Task<UserAdvertResult> GetAdvertDetail(string command, int id = 0);
        Task<int> ChangeAdvertStatus(string query, UserAdvertResult command);
        Task<int> ChangeAdvertStatusOperator(string command, UserAdvertResult model, int userId);
        Task<FtpDetailsModel> GetFtpDetails(string command, int operatorId);
        Task<int> UpdateMediaLoaded(string command, UserAdvertResult advert);
    }
}
