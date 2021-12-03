using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ITicketService
    {
        Task<ReturnResult> GetTicketDetails(int id = 0);
        Task<ReturnResult> UpdateTicketStatus(int id,int status);
        Task<ReturnResult> GetTicketList(int id = 0);
        Task<ReturnResult> GetTicketListAsync(PagingSearchClass paging);
        Task<ReturnResult> GetTicketListSales(int id = 0);
        Task<ReturnResult> AddComment(TicketComments model);
    }
}
