using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAdvertDAL
    {
        Task<IEnumerable<UserAdvertResult>> GetAdvertResultSet();
        Task<IEnumerable<AdvertCategoryResult>> GetAdvertCategoryList();
        Task<UserAdvertResult> GetAdvertDetail(int id = 0);
        Task<int> ChangeAdvertStatus(UserAdvertResult command);
        Task<int> ChangeAdvertStatusOperator(UserAdvertResult model, int userId);
        Task<FtpDetailsModel> GetFtpDetails(int operatorId);
        Task<int> UpdateMediaLoaded(UserAdvertResult advert);
        Task<int> RejectAdvertReason(UserAdvertResult model);
        Task<int> RejectAdvertReasonOperator(UserAdvertResult model, string connString, int uid, int rejId, int adId);
    }
}
