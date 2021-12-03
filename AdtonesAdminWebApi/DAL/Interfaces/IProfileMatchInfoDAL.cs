using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IProfileMatchInfoDAL
    {
        Task<IEnumerable<ProfileMatchInformationFormModel>> LoadProfileResultSet();
        Task<IEnumerable<ProfileMatchInformationFormModel>> GetProfileMatchInformation(int countryId);
        Task<ProfileMatchInformationFormModel> GetProfileById(int id);
        Task<ProfileMatchLabelFormModel> GetProfileLabelById(int id);
        Task<IEnumerable<ProfileMatchLabelFormModel>> GetListProfileLabelById(int id);
        Task<int> UpdateProfileInfo(ProfileMatchInformationFormModel model);
        Task<int> AddProfileInfo(ProfileMatchInformationFormModel model);
        Task<int> AddProfileInfoLabel(ProfileMatchLabelFormModel model);
        Task<int> UpdateProfileInfoLabel(ProfileMatchLabelFormModel model);
        Task<int> DeleteProfileLabelById(int id);
        Task<bool> CheckIfProfileInfoExists(ProfileMatchInformationFormModel model);

    }
}
