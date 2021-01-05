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


        // <summary>
        /// 
        /// </summary>
        /// <returns>body contains integer</returns>
        [HttpPut("v1/UpdateUserPermissions")]
        public async Task<ReturnResult> UpdateUserPermissions(PermissionChangeModel model)
        {
            return await _permService.UpdateUserPermissionsById(model);
        }


        // <summary>
        /// 
        /// </summary>
        /// <returns>body contains integer</returns>
        [HttpPost("v1/AddNewPage")]
        public async Task<ReturnResult> AddNewPage(AddNewPermissionPart model)
        {
            return await _permService.AddNewPage(model);
        }


        // <summary>
        /// 
        /// </summary>
        /// <returns>body contains integer</returns>
        [HttpPost("v1/AddNewElement")]
        public async Task<ReturnResult> AddNewElement(AddNewPermissionPart model)
        {
            return await _permService.AddNewElement(model);
        }


        [HttpGet("v1/GetPermissionPagesList/{roleid}")]
        public async Task<ReturnResult> GetPermissionPagesList(int roleid)
        {
            return await _permService.SelectListPermissionPages(roleid);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">IdCollectionModel uses userid for 1st and id for the second comparison</param>
        /// <returns></returns>
        [HttpPut("v1/ComparePermissionPages")]
        public async Task<ReturnResult> ComparePermissionPages(IdCollectionViewModel model)
        {
            return await _permService.Compare2Permissions(model);
        }

    }
}