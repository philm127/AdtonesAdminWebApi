﻿using System;
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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IAreaService _areaService;


        public CountryController(ICountryService countryService, IAreaService areaService)
        {
            _countryService = countryService;
            _areaService = areaService;
        }


        #region Country


        [Route("v1/GetCountryData")]
        public async Task<ReturnResult> GetCountryData()
        {
            var tst = await _countryService.LoadDataTable();
            return tst;
        }


        [Route("v1/GetCountry")]
        public async Task<ReturnResult> GetCountry(IdCollectionViewModel model)
        {
            var tst = await _countryService.GetCountry(model);
            return tst;
        }


        [Route("v1/AddCountry")]
        public async Task<ReturnResult> AddCountry([FromForm] CountryFormModel countrymodel)
        {
            return await _countryService.AddCountry(countrymodel);
        }


        [Route("v1/UpdateCountry")]
        public async Task<ReturnResult> UpdateCountry([FromForm]CountryFormModel countrymodel)
        {
            return await _countryService.UpdateCountry(countrymodel);
        }


        #endregion


        #region Areas


        [Route("v1/GetAreaData")]
        public async Task<ReturnResult> GetAreaData()
        {
            return await _areaService.LoadDataTable();
        }


        [Route("v1/GetAreaById")]
        public async Task<ReturnResult> GetAreaById([FromBody]IdCollectionViewModel model)
        {
            return await _areaService.GetArea(model.id);
        }


        [Route("v1/AddArea")]
        public async Task<ReturnResult> AddArea([FromBody]AreaResult model)
        {
            return await _areaService.AddArea(model);
        }


        [Route("v1/UpdateArea")]
        public async Task<ReturnResult> UpdateArea([FromBody]AreaResult model)
        {
            return await _areaService.UpdateArea(model);
        }


        [Route("v1/DeleteArea")]
        public async Task<ReturnResult> DeleteArea([FromBody]IdCollectionViewModel model)
        {
            return await _areaService.DeleteArea(model.id);
        }


        #endregion


    }
}