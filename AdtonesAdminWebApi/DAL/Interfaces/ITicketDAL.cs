using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.TicketingModels;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ITicketDAL
    {
        Task<IEnumerable<TicketListModel>> GetTicketList(int id);
        Task<IEnumerable<TicketListModel>> GetTicketListForSales(int id = 0);
        Task<IEnumerable<TicketListModel>> GetTicketListForAdvertiser(int id);
        Task<TicketListModel> GetTicketDetails(int id = 0);
        Task<int> UpdateTicketStatus(TicketListModel model);
        Task<IEnumerable<TicketListModel>> GetOperatorTicketList(PagingSearchClass paging);
        Task<IEnumerable<TicketComments>> GetTicketcomments(int id = 0);
        Task<string> CreateNewHelpTicket(TicketFormModel ticket);
        Task<int> AddNewComment(TicketComments ticket);
        Task<int> AddNewCommentImage(TicketComments ticket);
        Task<IEnumerable<TicketListModel>> GetTicketListAsync(PagingSearchClass param);
        Task<string> GetEmailForLiveServer(int questionId);
    }
}
