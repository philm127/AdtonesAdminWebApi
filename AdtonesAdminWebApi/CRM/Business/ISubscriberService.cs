using AdtonesAdminWebApi.CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CRM.Business
{
    public interface ISubscriberService
    {
        Task<ReturnResult> GetSubscriberList(PagingSearchClass param);
        Task<ReturnResult> GetSubscriber(int subscriberId);
    }
}
