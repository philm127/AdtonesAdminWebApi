﻿using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ILogonService
    {
        Task<ReturnResult> Login(User userForm);
        Task<ReturnResult> ForgotPassword(string emailAddress);
        Task<int> UpdatePasswordHistory(int userId, string password);
        Task<bool> IsPreviousPassword(int userId, string newPassword);
        Task<ReturnResult> ChangePassword(PasswordModel model);
        Task<ReturnResult> ResetPassword(PasswordModel model);
        Task<ReturnResult> RefreshAccessToken(string email);
    }
}
