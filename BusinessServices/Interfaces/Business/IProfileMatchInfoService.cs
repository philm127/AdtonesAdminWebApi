using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface IProfileMatchInfoService
    {
        ReturnResult FillProfileType();
        Task<ReturnResult> LoadDataTable();
        Task<ReturnResult> GetProfileInfo(int id);
        Task<ReturnResult> UpdateProfileInfo(ProfileMatchInformationFormModel model);
        Task<ReturnResult> AddProfileInfo(ProfileMatchInformationFormModel model);
        Task<ReturnResult> DeleteProfileLabel(int id);
    }
}
