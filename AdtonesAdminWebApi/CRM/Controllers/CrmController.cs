using AdtonesAdminWebApi.CRM.Business;
using AdtonesAdminWebApi.CRM.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CRM.Controllers
{
    [Route("api/[controller]")]
    // [Authorize]
    [ApiController]
    public class CrmController : ControllerBase
    {
        private readonly ICrmService _crm;
        ReturnResult result = new ReturnResult();


        public CrmController(ICrmService crm)
        {
            _crm = crm;
        }

    }
}
