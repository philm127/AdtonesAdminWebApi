using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface IProfileMatchInfoDAL
    {
        Task<IEnumerable<ProfileMatchInformationFormModel>> LoadProfileResultSet();
        Task<ProfileMatchInformationFormModel> GetProfileById(int id);
        Task<IEnumerable<ProfileMatchLabelFormModel>> GetProfileLabelById(int id);
        Task<int> UpdateProfileInfo(ProfileMatchInformationFormModel model);
        Task<int> AddProfileInfo(ProfileMatchInformationFormModel model);
        Task<int> AddProfileInfoLabel(ProfileMatchLabelFormModel model);
        Task<int> UpdateProfileInfoLabel(ProfileMatchLabelFormModel model);
        Task<int> DeleteProfileLabelById(int id);
        Task<bool> CheckIfProfileInfoExists(ProfileMatchInformationFormModel model);

    }
}
