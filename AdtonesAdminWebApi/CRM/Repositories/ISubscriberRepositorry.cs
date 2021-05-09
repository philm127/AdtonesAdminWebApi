using AdtonesAdminWebApi.CRM.Models;
using AdtonesAdminWebApi.CRM.Models.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CRM.Repositories
{
    public interface ISubscriberRepositorry
    {
        Task<IEnumerable<SubscriberListModel>> GetSubscribers(PagingSearchClass param);
        Task<SubscriberDto> GetSubscriber(int id);
    }
}
