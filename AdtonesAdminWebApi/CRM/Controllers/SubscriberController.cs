using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.CRM.Models.Subscriber;
using AdtonesAdminWebApi.CRM.Business;
using Microsoft.AspNetCore.Authorization;
using AdtonesAdminWebApi.CRM.Models;

namespace AdtonesAdminWebApi.CRM.Controllers
{
    [Route("api/[controller]")]
   // [Authorize]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriberService _subscriber;
        ReturnResult result = new ReturnResult();


        public SubscriberController(ISubscriberService subscriber)
        {
            _subscriber = subscriber;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>body contains BillingPaymentModel</returns>
        [HttpGet("v1/GetSubscriberList")]
        public async Task<ActionResult<ReturnResult>> GetSubscriberList(PagingSearchClass param)
        {
            return await _subscriber.GetSubscriberList(param);
        }


        [HttpGet("v1/GetSubscriber/{id}")]
        public async Task<ActionResult<ReturnResult>> GetSubscriber(int id)
        {
            return await _subscriber.GetSubscriber(id);
        }

    }
}
