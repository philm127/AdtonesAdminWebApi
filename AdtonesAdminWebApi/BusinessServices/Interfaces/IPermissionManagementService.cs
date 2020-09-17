﻿using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IPermissionManagementService
    {
        Task<ReturnResult> GetPermissionsByUser(int id);

        Task<ReturnResult> UpdateUserPermissionsById(PermissionChangeModel model);

        Task<ReturnResult> AddNewPage(AddNewPermissionPart model);
        Task<ReturnResult> AddNewElement(AddNewPermissionPart model);
        Task<ReturnResult> SelectListPermissionPages();
    }
}
