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


        public PermisionManagementService(IConfiguration configuration, IPermissionManagementDAL permDAL)

        {
            _configuration = configuration;
            _permDAL = permDAL;
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
                string permPreList = string.Empty;
                var permList = new List<PermissionModel>();
                permPreList = await _permDAL.GetPermissionsByUserId(id);
                if(permPreList != null && permPreList.Length > 50)
                    permList = JsonSerializer.Deserialize<List<PermissionModel>>(permPreList);

                result.body = permList;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PermissionManagementService",
                    ProcedureName = "GetPermissionsByUser"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PermissionManagementService",
                    ProcedureName = "UpdateUserPermissionsById"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PermissionManagementService",
                    ProcedureName = "AddNewPage"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PermissionManagementService",
                    ProcedureName = "AddNewPage"
                };
                _logging.LogError();
                result.result = 0;
            }

            return result;
        }


        public async Task<ReturnResult> SelectListPermissionPages()
        {
            // var pages = new List<string>();
            List<PermissionModel> pg = new List<PermissionModel>();
            List<SharedSelectListViewModel> selectedList = new List<SharedSelectListViewModel>();

            try
            {
                var permList = await _permDAL.GetPermissionsForSelectList();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "PermissionManagementService",
                    ProcedureName = "SelectListPermissionPages"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


    }
}
