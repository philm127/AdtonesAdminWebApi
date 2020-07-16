using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PermissionManagementController : ControllerBase
    {
        private readonly IPermissionManagementService _permService;

        public PermissionManagementController(IPermissionManagementService permService)
        {
            _permService = permService;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List PermissionModel</returns>
        [HttpGet("v1/GetPermissionsByUser/{id}")]
        public async Task<ReturnResult> GetPermissionsByUser(int id)
        {
            return await _permService.GetPermissionsByUser(id);
        }


        

    }
}