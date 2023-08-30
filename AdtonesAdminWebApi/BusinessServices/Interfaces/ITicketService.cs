using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ITicketService
    {
        Task<ReturnResult> UpdateTicketStatus(int id,int status);
        Task<ReturnResult> AddComment(TicketComments model);
        Task<ReturnResult> GetOperatorTicketList(PagingSearchClass paging);
    }
}
