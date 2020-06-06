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
        Task<IEnumerable<TicketListModel>> GetTicketList(string command, int id);
        Task<TicketListModel> GetTicketDetails(string command, int id = 0);
        Task<int> UpdateTicketStatus(string command, TicketListModel model);
        Task<IEnumerable<TicketListModel>> GetOperatorTicketList(string command, int operatorId);
        Task<TicketComments> GetTicketcomments(string command, int id = 0);
        Task<string> CreateNewHelpTicket(string command, TicketFormModel ticket);
    }
}
