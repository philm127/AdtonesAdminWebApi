using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    // [Authorize]
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
        [HttpGet("v1/GetTicketList/{id}")]
        public async Task<ReturnResult> GetTicketList(int id=0)
        {
            return await _ticketService.GetTicketList(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpPut("v1/GetTicketListAsync")]
        public async Task<ReturnResult> GetTicketListAsync(PagingSearchClass paging)
        {
            return await _ticketService.GetTicketListAsync(paging);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpGet("v1/GetTicketListForSales/{id}")]
        public async Task<ReturnResult> GetTicketListForSales(int id = 0)
        {
            return await _ticketService.GetTicketListSales(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains HelpAdminResult</returns>
        [HttpGet("v1/GetTicketDetails/{id}")]
        public async Task<ReturnResult> GetTicketDetails(int id)
        {
            return  await _ticketService.GetTicketDetails(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains int No. records Updated</returns>
        [HttpPut("v1/UpdateTicketStatus")]
        public async Task<ReturnResult> UpdateTicketStatus(IdCollectionViewModel model)
        {
            return await _ticketService.UpdateTicketStatus(model.id,model.status);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains message success</returns>
        [HttpPost("v1/AddTicketComment")]
        public async Task<ReturnResult> AddTicketComment([FromForm] TicketComments model)
        {
            return await _ticketService.AddComment(model);
        }

    }
}
