using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using AdtonesAdminWebApi.ViewModels.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAdvertDAL
    {
        Task<IEnumerable<UserAdvertResult>> GetAdvertResultSet(int id = 0);
        Task<IEnumerable<UserAdvertResult>> GetAdvertForSalesResultSet(int id = 0);
        Task<IEnumerable<AdvertTableAdvertiserDto>> GetAdvertForAdvertiserResultSet(int id);
        Task<AdvertiserAdvertTableDashboardDto> GetAdvertForAdvertiserDashboard(int id);
        Task<IEnumerable<UserAdvertResult>> GetAdvertResultSetById(int id = 0);
        Task<NewAdvertFormModel> CreateNewAdvert(NewAdvertFormModel model);
        Task<NewAdvertFormModel> CreateNewOperatorAdvert(NewAdvertFormModel model);
        Task<NewAdvertFormModel> GetAdvertForUpdateModel(int id);
        Task<int> UpdateAdvert(NewAdvertFormModel model);
        Task<UserAdvertResult> GetAdvertDetail(int id = 0);
        Task<int> ChangeAdvertStatus(UserAdvertResult command);
        Task<int> ChangeAdvertStatusOperator(UserAdvertResult model, int userId, int adId);
        Task<FtpDetailsModel> GetFtpDetails(int operatorId);
        Task<int> UpdateMediaLoaded(UserAdvertResult advert);
        Task<int> RejectAdvertReason(UserAdvertResult model);
        Task<int> RejectAdvertReasonOperator(UserAdvertResult model, string connString, int uid, int rejId, int adId);

        Task<int> DeleteAdvertRejection(UserAdvertResult model);
        Task<int> DeleteRejectAdvertReasonOperator(string connString, int adId);
        Task<int> UpdateAdvertForBilling(int advertId, string constr);
        Task<bool> CheckAdvertNameExists(string advertName, int userId);
        Task<int> GetAdvertIdByCampid(int campaignId);
    }
}
