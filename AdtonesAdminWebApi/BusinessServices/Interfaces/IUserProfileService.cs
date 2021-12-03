using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IUserProfileService
    {
        Task<ReturnResult> GetUserProfilePreference(int id, int? campaignId);
    }
}
