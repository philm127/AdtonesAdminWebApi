using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }


        [Route("v1/GetAreaResult")]
        public async Task<ReturnResult> GetAreaResult()
        {
           return await _areaService.FillAreaResult();
        }


        [Route("v1/GetAreaById")]
        public async Task<ReturnResult> GetAreaById(IdCollectionViewModel model)
        {
            return await _areaService.GetArea(model.id);
        }


        [Route("v1/AddArea")]
        public async Task<ReturnResult> AddArea(AreaResult model)
        {
            return await _areaService.AddArea(model);
        }


        [Route("v1/UpdateArea")]
        public async Task<ReturnResult> UpdateArea(AreaResult model)
        {
            return await _areaService.UpdateArea(model);
        }


        [Route("v1/DeleteArea")]
        public async Task<ReturnResult> DeleteArea(IdCollectionViewModel model)
        {
            return await _areaService.DeleteArea(model.id);
        }

    }
}