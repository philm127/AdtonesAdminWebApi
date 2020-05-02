using AdtonesAdminWebApi.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CountryTaxService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public CountryTaxService(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        public async Task<ReturnResult> LoadData()
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<CountryTaxResult> _result = new List<CountryTaxResult>();
                IEnumerable<CountryTax> countryTax = null;
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;

                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                if (param.Columns != null)
                {
                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                        if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                            searchValue = true;
                    }
                }

                if (searchValue == true)
                {
                    #region Search Functionality
                    int?[] CountryId = new int?[cnt];
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            CountryId = columnSearch[0].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[0] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            var data = columnSearch[3].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[3] = null;
                        }
                    }

                    countryTax = _countryTaxRepository.GetAll().OrderByDescending(top => top.Id).ToList();
                    if (columnSearch[0] != null)
                    {
                        countryTax = countryTax.Where(top => (CountryId.Contains(Convert.ToInt32(top.CountryId)))).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        countryTax = countryTax.Where(top => (top.CreatedDate >= CreatedDatefromdate && top.CreatedDate <= CreatedDatetodate)).ToList();
                    }

                    cnt = countryTax.Count();
                    countryTax = countryTax.Skip(param.Start).Take(param.Length);

                    #endregion
                }
                else
                {
                    countryTax = _countryTaxRepository.GetAll().OrderByDescending(top => top.Id).ToList();
                    cnt = countryTax.Count();
                    countryTax = countryTax.Skip(param.Start).Take(param.Length);
                }

                foreach (var item in countryTax)
                {
                    _result.Add(new CountryTaxResult { Id = item.Id, CreatedDate = item.CreatedDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, Name = item.Country.Name, ShortName = item.Country.ShortName, Status = item.Status, TaxPercantage = item.TaxPercantage });
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<CountryTaxResult> result = new DTResult<CountryTaxResult>
                {
                    draw = param.Draw,
                    data = _result,
                    recordsFiltered = cnt,
                    recordsTotal = cnt
                };

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        //Add 28-06-2019
        // Function For Filter/Sorting CountryTax Data
        private static List<CountryTaxResult> ApplySorting(DTParameters param, List<CountryTaxResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Name).ToList();
                    else
                        result = result.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.ShortName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ShortName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TaxPercantage).ToList();
                    else
                        result = result.OrderByDescending(top => top.TaxPercantage).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        public void FillAddCountry()
        {
            var clientdetails = _countryRepository.GetAll().Select(top => new SelectListItem
            {
                Text = top.Name,
                Value = top.Id.ToString()
            }).ToList();
            ViewBag.country = clientdetails;

        }
        public List<CountryTaxResult> FillCountryTaxResult()
        {
            List<CountryTaxResult> _result = new List<CountryTaxResult>();
            var countryResult = _countryTaxRepository.GetAll().OrderByDescending(top => top.Id).ToList();
            foreach (var item in countryResult)
            {
                _result.Add(new CountryTaxResult { Id = item.Id, CountryId = item.CountryId.Value, CreatedDate = item.CreatedDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, Name = item.Country.Name, ShortName = item.Country.ShortName, Status = item.Status, TaxPercantage = item.TaxPercantage });
            }
            return _result;
        }
        [Route("AddCountryTax")]
        public ActionResult AddCountryTax()
        {
            FillAddCountry();
            CountryTaxFormModel _countryModel = new CountryTaxFormModel();

            return View(_countryModel);
        }

        [Route("AddCountryTax")]
        [HttpPost]
        public ActionResult AddCountryTax(CountryTaxFormModel countryTaxmodel)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CountryTaxFormModel countryTax = new CountryTaxFormModel();
                var checkexists = _countryTaxRepository.Get(top => top.CountryId == countryTaxmodel.CountryId);
                if (checkexists != null)
                {
                    TempData["Error"] = "Record already exists.";
                    FillAddCountry();
                    return View(countryTaxmodel);
                }
                countryTax.UserId = efmvcUser.UserId;
                countryTax.CountryId = countryTaxmodel.CountryId;
                countryTax.TaxPercantage = countryTaxmodel.TaxPercantage;
                countryTax.CreatedDate = DateTime.Now;

                CreateOrUpdateCountryTaxCommand command =
                Mapper.Map<CountryTaxFormModel, CreateOrUpdateCountryTaxCommand>(countryTax);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record added successfully.";
                    var countryName = _countryRepository.GetById(Convert.ToInt32(countryTax.CountryId)).Name;
                    TempData["status"] = "Country tax rate of " + countryTax.TaxPercantage + "% is added to Country " + countryName;
                    return RedirectToAction("Index");
                }
            }
            FillAddCountry();
            return View(countryTaxmodel);
        }
        [Route("UpdateCountryTax")]
        [HttpPost]
        public ActionResult UpdateCountryTax(CountryTaxFormModel countryTaxmodel)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CountryTaxFormModel countryTax = new CountryTaxFormModel();
                countryTax.Id = countryTaxmodel.Id;
                countryTax.UserId = efmvcUser.UserId;
                countryTax.CountryId = countryTaxmodel.CountryId;
                countryTax.TaxPercantage = countryTaxmodel.TaxPercantage;
                countryTax.CreatedDate = countryTaxmodel.CreatedDate;


                CreateOrUpdateCountryTaxCommand command =
                Mapper.Map<CountryTaxFormModel, CreateOrUpdateCountryTaxCommand>(countryTax);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record updated successfully.";
                    var countryName = _countryRepository.GetById(Convert.ToInt32(countryTax.CountryId)).Name;
                    TempData["status"] = "Country tax rate of " + countryTax.TaxPercantage + "% is updated for Country " + countryName;
                    return RedirectToAction("Index");
                }
            }
            FillAddCountry();
            return View(countryTaxmodel);
        }
        [Route("UpdateCountryTax")]
        public ActionResult UpdateCountryTax(int id)
        {
            FillAddCountry();
            var countryTax = _countryTaxRepository.Get(top => top.Id == id);
            CountryTaxFormModel command =
               Mapper.Map<CountryTax, CountryTaxFormModel>(countryTax);
            return View(command);
        }

    }
}
