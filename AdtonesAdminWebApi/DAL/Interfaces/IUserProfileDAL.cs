using AdtonesAdminWebApi.ViewModels.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserProfileDAL
    {
        Task<UserProfileDto> GetUserProfileByUserId(int userId);
        Task<UserProfilePreferenceDto> GetUserProfilePreferenceByUserProfileId(int userId);
    }
}
