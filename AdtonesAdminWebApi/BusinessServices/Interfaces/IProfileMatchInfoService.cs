using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IProfileMatchInfoService
    {
        ReturnResult FillProfileType();
        Task<ReturnResult> GetProfileInfo(int id);
        Task<ReturnResult> UpdateProfileInfo(ProfileMatchInformationFormModel model);
        Task<ReturnResult> AddProfileInfo(ProfileMatchInformationFormModel model);
    }
}
