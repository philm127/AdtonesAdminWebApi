using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class OperatorService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManagementService _userService;
        ReturnResult result = new ReturnResult();


        public OperatorService(IConfiguration configuration, IUserManagementService userService)

        {
            _configuration = configuration;
            _userService = userService;
        }


        //public async Task<ReturnResult> AdminOperatorRegistration(OperatorAdminFormModel model)
        //{
        //    int registeredId = 0;
        //    var topRoleId = (int)Enums.UserRole.OperatorAdmin;

        //    try
        //    {
        //        bool userOperatorIdExist = false;

        //        // Checks to see if an Operator Admin already exist.
        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            userOperatorIdExist = connection.ExecuteScalar<bool>(@"SELECT COUNT(1) FROM Users 
        //                                                            WHERE OperatorId=@OperatorId AND RoleId=@topRoleId",
        //                                                          new { OperatorId = model.OperatorId, topRoleId = topRoleId });
        //        }

        //        if (userOperatorIdExist)
        //        {
        //            result.body = "An Operator admin already exists.";
        //            result.result = 0;
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "OperatorService",
        //            ProcedureName = "AddOperator-CheckExists"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //        result.error = "Checks for unique failed";
        //    }

        //    try
        //    {

        //        var command = new User();

        //        command.Email = model.Email;
        //        command.FirstName = model.FirstName;
        //        command.LastName = model.LastName;
        //        command.PasswordHash = Md5Encrypt.Md5EncryptPassword(model.PasswordHash);
        //        command.DateCreated = DateTime.Now;
        //        command.Organisation = model.Organisation;
        //        command.LastLoginTime = DateTime.Now;
        //        command.RoleId = (int)Enums.UserRole.OperatorAdmin;
        //        command.Activated = model.Activated;
        //        command.VerificationStatus = true;
        //        command.Outstandingdays = 0;
        //        command.OperatorId = model.OperatorId;
        //        command.IsMsisdnMatch = true;
        //        command.IsEmailVerfication = true;
        //        command.PhoneticAlphabet = null;
        //        command.IsMobileVerfication = true;
        //        command.OrganisationTypeId = null;
        //        command.UserMatchTableName = null;


        //        var body = await _userService.AddUser(command);
        //        if (body.result == 0)
        //        {
        //            result.result = 0;
        //            result.error = body.error;
        //            return result;
        //        }

        //        registeredId = (int)body.body;

        //        if (registeredId > 0)
        //        {
        //            var command1 = new Contacts();

        //            int currencyId = 0;
        //            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //            {
        //                currencyId = connection.ExecuteScalar<int>(@"SELECT CurrencyId FROM Currencies WHERE CountryId=@countryId;",
        //                                                              new { countryId = model.CountryId });
        //            }

        //            if (currencyId == 0)
        //            {
        //                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //                {
        //                    currencyId = connection.ExecuteScalar<int>(@"SELECT CurrencyId FROM Currencies WHERE CurrencyCode='USD';");
        //                }
        //            }
        //            command1.CurrencyId = currencyId;
        //            command1.UserId = registeredId;
        //            command1.MobileNumber = model.MobileNumber;
        //            command1.FixedLine = null;
        //            command1.Email = model.Email;
        //            command1.PhoneNumber = model.PhoneNumber;
        //            command1.Address = model.Address;
        //            command1.CountryId = model.CountryId;

        //            if (result1.Success)
        //            {
        //                // For scalability added an array of app config settings to retrieve specifically in this case for
        //                // operator admin.
        //                var confSettings = new string[] { "OperatorAdminRegistrationEmailTemplete", "OperatorAdminUrl" };
        //                SendEmailVerificationCode(model.FirstName, model.LastName, model.Email, model.Password, confSettings);
        //                //TempData["status"] = "Record added successfully.";
        //                TempData["status"] = "Operator Admin registered for Operator " + model.FirstName + " " + model.LastName;
        //                return RedirectToAction("Index");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "OperatorService",
        //            ProcedureName = "AddOperator-Adding"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //        result.error = "Adding user failed";
        //    }
        //}



    }
}
