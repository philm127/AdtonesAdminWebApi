using AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.Queries;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Api.Controllers
{
    public class SubscriberController : ApiController
    {

        [HttpGet]
        public async Task<ActionResult<ReturnResult>> GetAll(GetAllSubscribersQuery request)
        {
            return await Mediator.Send(request);
        }
    }
}
