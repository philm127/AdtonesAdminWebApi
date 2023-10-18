using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserCreditService : IUserCreditService
    {
        private readonly IBillingDAL _billDAL;
        private readonly ILoggingService _logServ;
        private readonly IUserCreditDAL _creditDAL;
        private static Random random = new Random();
        ReturnResult result = new ReturnResult();
        const string PageName = "UserCreditService";
        public UserCreditService(ILoggingService logServ, IBillingDAL billDAL, IUserCreditDAL creditDAL)
        {
            _billDAL = billDAL;
            _creditDAL = creditDAL;
            _logServ = logServ;
        }

        /// <summary>
        /// Used for both Adding New Credit and Updating Existing Credit
        /// </summary>
        /// <param name="_usercredit"></param>
        /// <returns></returns>
        public async Task<ReturnResult> AddPaymentToUserCredit(AdvertiserCreditFormCommand model)
        {
            try
            {
                int x = 0;
                var creditDetails = await _creditDAL.GetUserCreditDetail(model.UserId);
                var balance = await _billDAL.GetCreditBalance(model.UserId);

                if (creditDetails != null)
                {
                    var available = creditDetails.AssignCredit + (balance);
                    if (available > creditDetails.AssignCredit)
                        available = creditDetails.AssignCredit;
                    model.Id = creditDetails.Id;
                    model.AssignCredit = creditDetails.AssignCredit;
                    model.AvailableCredit = available;
                    x = await _creditDAL.UpdateUserCredit(model.Id, available);
                }
                else
                {
                    model.AvailableCredit = (balance);
                    model.AssignCredit = model.Amount;
                    x = await _creditDAL.AddUserCredit(model);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddPaymentToUserCredit";
                await _logServ.LogError();

                result.result = 0;
                result.error = "Credit was not added successfully";
                return result;
            }
            result.body = "Assigned credit successfully";
            return result;
        }


        /// <summary>
        /// Used for both Adding New Credit and Updating Existing Credit
        /// </summary>
        /// <param name="_usercredit"></param>
        /// <returns></returns>
        public async Task<ReturnResult> AddUserCredit(AdvertiserCreditFormCommand model)
        {
            try
            {
                int x = 0;

                var creditDetails = await GetUserCreditDetail(model.UserId);

                if (creditDetails != null)
                {
                    var balance = await _billDAL.GetCreditBalance(model.UserId);
                    
                    var availableCredit = model.AssignCredit + creditDetails.AvailableCredit + (balance);
                    model.AssignCredit = model.AssignCredit;
                    model.Id = creditDetails.Id;
                    if (availableCredit > model.AssignCredit)
                        model.AvailableCredit = model.AssignCredit;
                    else
                        model.AvailableCredit = availableCredit;
                    x = await _creditDAL.UpdateUserCredit(model.UserId, availableCredit);
                }
                else
                {
                    model.AvailableCredit = model.AssignCredit;
                    x = await _creditDAL.AddUserCredit(model);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddUserCredit";
                await _logServ.LogError();

                result.result = 0;
                result.error = "Credit was not added successfully";
                return result;
            }
            result.body = "Assigned credit successfully";
            return result;
        }


        public async Task<UserCreditDetailsDto> GetUserCreditDetail(int id)
        {
            return await _creditDAL.GetUserCreditDetail(id);
        }


        // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        //private bool UpdateUserCrediTFromPayWithUserCredit(int userId, decimal availablecredit)
        //{
        //    var _usercreditDetails = _userCreditRepository.Get(top => top.UserId == userId);
        //    if (_usercreditDetails != null)
        //    {
        //        UsersCreditFormModel _userCreditModel = new UsersCreditFormModel();
        //        _userCreditModel.Id = _usercreditDetails.Id;
        //        _userCreditModel.UserId = _usercreditDetails.UserId;
        //        _userCreditModel.AssignCredit = _usercreditDetails.AssignCredit;
        //        _userCreditModel.AvailableCredit = availablecredit;
        //        _userCreditModel.UpdatedDate = DateTime.Now;
        //        _userCreditModel.CreatedDate = _usercreditDetails.CreatedDate;
        //        _userCreditModel.CurrencyId = _usercreditDetails.CurrencyId;
        //        CreateOrUpdateUsersCreditCommand command =
        //          Mapper.Map<UsersCreditFormModel, CreateOrUpdateUsersCreditCommand>(_userCreditModel);
        //        ICommandResult result = _commandBus.Submit(command);
        //        if (result.Success)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}


        //public decimal updateusercreditFromRecievedPayment(int userid, decimal receivedamount)
        //{
        //    int status = 0;
        //    decimal extraCredit = 0;
        //    var usercredit = _userCreditRepository.Get(top => top.UserId == userid);
        //    if (usercredit != null)
        //    {
        //        var AvailableCredit = usercredit.AvailableCredit;
        //        AvailableCredit = AvailableCredit + receivedamount;
        //        if (AvailableCredit > usercredit.AssignCredit)
        //        {
        //            extraCredit = AvailableCredit - usercredit.AssignCredit;
        //            AvailableCredit = AvailableCredit - extraCredit;
        //        }
        //        UpdateUserCreditCommand command = new UpdateUserCreditCommand();
        //        command.UserId = userid;
        //        command.CurrencyId = usercredit.CurrencyId;
        //        command.AvailableCredit = AvailableCredit;
        //        ICommandResult result = _commandBus.Submit(command);
        //        if (result.Success)
        //        {
        //            if (extraCredit != 0) return extraCredit;
        //            status = 0;
        //        }
        //    }
        //    return status;
        //}


        //public ActionResult UpdateCredit(UserCreditFormModel _creditmodel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        UsersCreditFormModel _usercredit = new UsersCreditFormModel();
        //        _usercredit.Id = _creditmodel.Id;
        //        _usercredit.UserId = _creditmodel.UserId;
        //        _usercredit.AssignCredit = _creditmodel.AssignCredit;
        //        var usercreditpayment = _usercreditpaymentRepository.GetAll().Where(top => top.UserId == _creditmodel.UserId).Sum(top => top.Amount);
        //        var userbilling = _billingRepository.GetAll().Where(top => top.UserId == _creditmodel.UserId && top.PaymentMethodId == 1).Sum(top => top.FundAmount);
        //        _usercredit.AvailableCredit = (_creditmodel.AssignCredit + usercreditpayment) - (userbilling);
        //        _usercredit.CreatedDate = DateTime.Now;
        //        _usercredit.UpdatedDate = DateTime.Now;
        //        _usercredit.CurrencyId = _creditmodel.CurrencyId;
        //        CreateOrUpdateUsersCreditCommand command =
        //        Mapper.Map<UsersCreditFormModel, CreateOrUpdateUsersCreditCommand>(_usercredit);
        //        ICommandResult result = _commandBus.Submit(command);
        //        if (result.Success)
        //        {
        //            //TempData["status"] = "Update user credit successfully.";
        //            var userName = _userRepository.GetById(_creditmodel.UserId);
        //            TempData["status"] = "Advertiser " + userName.FirstName + " " + userName.LastName + " have credit " + _creditmodel.AssignCredit + " updated successfully.";
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View(_creditmodel);
        //}
    }
}
