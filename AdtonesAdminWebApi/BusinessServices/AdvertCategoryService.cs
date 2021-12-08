using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertCategoryService : IAdvertCategoryService
    {
        private readonly ILoggingService _logServ;
        private readonly IAdvertCategoryDAL _advertDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "AdvertCategoryService";

        public AdvertCategoryService(ILoggingService logServ, IAdvertCategoryDAL advertDAL)
        {
            _logServ = logServ;
            _advertDAL = advertDAL;
        }

    }
}
