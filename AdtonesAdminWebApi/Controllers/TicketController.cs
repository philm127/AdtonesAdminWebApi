using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpGet("v1/GetTicketList")]
        public async Task<ReturnResult> GetTicketList()
        {
            return await _ticketService.GetTicketDetails();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains HelpAdminResult</returns>
        [HttpGet("v1/GetTicketDetails")]
        public async Task<ReturnResult> GetTicketDetails(IdCollectionViewModel model)
        {
            return  await _ticketService.GetTicketDetails(model.id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains int No. records Updated</returns>
        [HttpPut("v1/CloseTicket/{id}")]
        public async Task<ReturnResult> CloseTicket(int id)
        {
            return await _ticketService.CloseTicket(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains int No. records Updated</returns>
        [HttpPut("v1/ArchiveTicket/{id}")]
        public async Task<ReturnResult> ArchiveTicket(int id)
        {
            return await _ticketService.ArchiveTicket(id);
        }

    }
}
