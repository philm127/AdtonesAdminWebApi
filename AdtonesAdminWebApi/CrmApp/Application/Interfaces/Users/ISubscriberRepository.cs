using AdtonesAdminWebApi.CrmApp.Core.Entities;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users
{
    public interface ISubscriberRepository : IGenericSearchRepository<User>
    {
        // public PagingSearchClass request { get; set; }
    }
}