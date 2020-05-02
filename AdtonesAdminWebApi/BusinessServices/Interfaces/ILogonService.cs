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
        Task<ReturnResult> ForgotPassword(IdCollectionViewModel model);
    }
}
