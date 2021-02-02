using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
// using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class PermisionManagementService : IPermissionManagementService
    {

        private readonly IConfiguration _configuration;
        private readonly IPermissionManagementDAL _permDAL;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        const string PageName = "PermissionManagementService";


        public PermisionManagementService(IConfiguration configuration, IPermissionManagementDAL permDAL, ILoggingService logServ)

        {
            _configuration = configuration;
            _permDAL = permDAL;
            _logServ = logServ;
        }


        


        /// <summary>
        /// Gets either a list of Profile Information or a single if passed model Id is not zero
        /// </summary>
        /// <param name="model">The id is used to select a single profile</param>
        /// <returns>Either a List or single ProfileInformationResult model</returns>
        public async Task<ReturnResult> GetPermissionsByUser(int id)
        {
            try
            {
                var permList = new List<PermissionModel>();
                permList = await PermissionsByUser(id);

                result.body = permList;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetPermissionsByUser";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateUserPermissionsById(PermissionChangeModel model)
        {
            try
            {
                // PermissionList permListOfList = JsonSerializer.Deserialize<PermissionList>(model.permissions.ToString());
                List<PermissionModel> permList = JsonSerializer.Deserialize<List<PermissionModel>>(model.permissions.ToString()); ;// permListOfList.pageData;
                var str = JsonSerializer.Serialize(permList);
                result.body = await _permDAL.UpdateUserPermissions(model.UserId, str);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateUserPermissionsById";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddNewPage(AddNewPermissionPart model)
        {
            List<PermissionChangeModel> perms = new List<PermissionChangeModel>();
            try
            {

                int[] Roles = model.roles.Length > 0 ? Array.ConvertAll(model.roles, int.Parse) : new int[] { };

                var page = new PermissionModel();
                page.pageName = model.pageName;
                page.visible = model.visible;
                page.elements = model.elements;

                var iqPerms = await _permDAL.GetPermissionsByRoleId(Roles);
                perms = iqPerms.ToList();

                foreach (var perm in perms)
                {
                    List<PermissionModel> pg = new List<PermissionModel>();
                    if (perm.permissions != null && perm.permissions.Length > 0)
                        pg = JsonSerializer.Deserialize<List<PermissionModel>>(perm.permissions);
                    if (pg.Any(x => x.pageName == model.pageName))
                    {
                        result.result = 0;
                        result.error = "The PageName already exist";
                        return result;
                    }
                    else
                        pg.Add(page);

                    var str = JsonSerializer.Serialize(pg);

                    var x = await _permDAL.UpdateUserPermissions(perm.UserId, str);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddNewPage";
                await _logServ.LogError();
                
                result.result = 0;
            }

            return result;
        }


        public async Task<ReturnResult> AddNewElement(AddNewPermissionPart model)
        {
            List<PermissionChangeModel> perms = new List<PermissionChangeModel>();
            var els = new List<PermElement>();
            els = model.elements;
            try
            {

                int[] Roles = model.roles.Length > 0 ? Array.ConvertAll(model.roles, int.Parse) : new int[] { };

                var iqPerms = await _permDAL.GetPermissionsByRoleId(Roles);
                perms = iqPerms.ToList();

                foreach (var perm in perms)
                {
                    List<PermissionModel> pgs = new List<PermissionModel>();
                    if (perm.permissions != null && perm.permissions.Length > 0)
                        pgs = JsonSerializer.Deserialize<List<PermissionModel>>(perm.permissions);
                    if (pgs.Find(x => x.pageName == model.pageName).elements == null || pgs.Find(x => x.pageName == model.pageName).elements.Count == 0)
                    {
                        //List<PermElement> els = new List<PermElement>();
                        //els.Add(el);
                        pgs.Find(x => x.pageName == model.pageName).elements = els;
                    }
                    else
                    {
                        foreach (var pg in pgs)
                        {
                            if (pg.pageName == model.pageName)
                            {
                                var emts = new List<PermElement>();
                                emts = pg.elements;
                                foreach (var el in els)
                                {
                                    if (emts.Any(x => x.name == el.name))
                                    {
                                        result.result = 0;
                                        result.error = $"{el.name} -  An element with the same nae and type already exists";
                                        return result;
                                    }
                                    else
                                    {
                                        emts.Add(el);
                                        pgs.Find(x => x.pageName == model.pageName).elements = emts;
                                    }
                                }
                            }
                        }
                    }

                    var str = JsonSerializer.Serialize(pgs);

                    var x = await _permDAL.UpdateUserPermissions(perm.UserId, str);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddNewElement";
                await _logServ.LogError();
                
                result.result = 0;
            }

            return result;
        }


        public async Task<ReturnResult> SelectListPermissionPages(int roleid)
        {
            // var pages = new List<string>();
            List<PermissionModel> pg = new List<PermissionModel>();
            List<SharedSelectListViewModel> selectedList = new List<SharedSelectListViewModel>();

            try
            {
                var permList = await _permDAL.GetPermissionsForSelectList(roleid);
                pg = JsonSerializer.Deserialize<List<PermissionModel>>(permList);

                foreach (PermissionModel mod in pg)
                {
                    var itemModel = new SharedSelectListViewModel();
                    itemModel.Value = mod.pageName;
                    itemModel.Text = mod.pageName;
                    selectedList.Add(itemModel);
                    // pages.Add(mod.pageName);
                }

                

                //foreach (var item in pages)
                //{
                //    var itemModel = new SharedSelectListViewModel();
                //    itemModel.Value = item.ToString();
                //    itemModel.Text = item.ToString();
                //    selectedList.Add(itemModel);
                //}
                result.body = selectedList;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "SelectListPermissionPages";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }



        public async Task<ReturnResult> Compare2Permissions(IdCollectionViewModel model)
        {

            var arrayOfDiffs = new List<string>();
            try
            {
                List<PermissionModel> firstObject = await PermissionsByUser(model.userId);
                List<PermissionModel> secondObject = await PermissionsByUser(model.id);

                foreach (var page in firstObject)
                {
                    var pageSec = secondObject.Find(x => x.pageName == page.pageName);
                    if (pageSec == null)
                    {
                        arrayOfDiffs.Add($"[Missing Page] - {page.pageName} ");
                    }
                    else
                    {

                        if (page.pageName == "GeneralAccess")
                        {
                            foreach (var els in page.elements)
                            {
                                var elsSec = pageSec.elements.Find(x => x.name == els.name);

                                arrayOfDiffs.Add($"[PGA] - {page.pageName} ");
                                if (els.name == "country")
                                {
                                    arrayOfDiffs.Add($"[Country M] - {string.Join(", ", els.arrayId)} ");
                                    arrayOfDiffs.Add($"[Country 2nd] - {string.Join(",", elsSec.arrayId)} ");
                                }
                                else if (els.name == "operator")
                                {
                                    arrayOfDiffs.Add($"[Operator M] - {string.Join(", ", els.arrayId)} ");
                                    arrayOfDiffs.Add($"[Operator 2nd] - {string.Join(",", elsSec.arrayId)} ");
                                }
                                else if (els.name == "advertiser")
                                {
                                    arrayOfDiffs.Add($"[Advertiser M] - {string.Join(", ", els.arrayId)} ");
                                    arrayOfDiffs.Add($"[Advertiser 2nd] - {string.Join(",", elsSec.arrayId)} ");
                                }

                            }
                        }
                        else if (page.pageName == "MainMenu")
                        {
                            foreach (var els in page.elements)
                            {
                                var elsSec = pageSec.elements.Find(x => x.name == els.name);
                                // arrayOfDiffs.Add($"[MenuHeader] - {els.name} ");
                                if (elsSec == null)
                                    arrayOfDiffs.Add($"[MissingHeader] - {els.name} ");
                                else
                                {
                                    foreach (var submen in els.elements)
                                    {
                                        var submenSec = elsSec.elements.Find(x => x.name == submen.name);
                                        // arrayOfDiffs.Add($"[SunMenu] - {submen.name} ");
                                        if (submenSec == null)
                                            arrayOfDiffs.Add($"[MissingSub] - {submen.name} ");
                                        else
                                        {
                                            if (submen.visible != submenSec.visible)
                                            {
                                                arrayOfDiffs.Add($"[SubVisible] - {submen.name} [2nd Vis State] = {submenSec.visible}");
                                            }
                                            if (submen.route != submenSec.route)
                                            {
                                                arrayOfDiffs.Add($"[SubRoute] - {submen.name} [2nd Route] = {submenSec.route}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (page.pageName == "Login")
                        {
                            // Prints page name where there is a difference in page visible true/false
                            if (pageSec.visible != page.visible)
                                arrayOfDiffs.Add($"[Page Visible] - {page.pageName} ");
                            foreach (var els in page.elements)
                            {
                                var elsSec = pageSec.elements.Find(x => x.name == els.name);
                                if (elsSec == null)
                                    arrayOfDiffs.Add($"[MissingElement] - [Page] {page.pageName} [Element:]  {els.name}");
                                else
                                {
                                    if (els.name == "defaultroute")
                                    {
                                        if (els.route != elsSec.route)
                                            arrayOfDiffs.Add($"[DefaultRoute] - [Page] {page.pageName} Element: {els.name} [2nd DefRoute] = {elsSec.route}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Prints page name where there is a difference in page visible true/false
                            if (pageSec.visible != page.visible)
                                arrayOfDiffs.Add($"[Page Visible] - {page.pageName} ");
                            foreach (var els in page.elements)
                            {
                                var elsSec = pageSec.elements.Find(x => x.name == els.name && x.type == els.type);
                                if (elsSec == null)
                                    arrayOfDiffs.Add($"[MissingElement] - [Page] {page.pageName} [Element:]  {els.name}  [Type:] {els.type}");
                                else
                                {
                                    if (els.visible != elsSec.visible)
                                    {
                                        arrayOfDiffs.Add($"[ElementVisible] - [Page] {page.pageName} Element: {els.name} Type : {els.type}  [2nd Vis State] = {elsSec.visible}");
                                    }
                                    if (els.enabled != elsSec.enabled)
                                    {
                                        arrayOfDiffs.Add($"[ElementEnabled] - [Page] {page.pageName} Element: {els.name} Type : {els.type}  [2nd Enabled State] = {elsSec.enabled}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "Compare2Permissions";
                await _logServ.LogError();
                
                result.result = 0;
            }

            result.body = arrayOfDiffs;
            return result;
        }


        private async Task<List<PermissionModel>> PermissionsByUser(int id)
        {
            var permList = new List<PermissionModel>();
            try
            {
                string permPreList = string.Empty;
                permPreList = await _permDAL.GetPermissionsByUserId(id);
                if (permPreList != null && permPreList.Length > 50)
                    permList = JsonSerializer.Deserialize<List<PermissionModel>>(permPreList);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "PermissionsByUser";
                await _logServ.LogError();
            }
            return permList;
        }
    }
}
