using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class TicketService : ITicketService
    {
        ReturnResult result = new ReturnResult();

        IHttpContextAccessor _httpAccessor;
        private readonly ITicketDAL _ticketDAL;
        private readonly ISaveGetFiles _saveFile;
        private readonly ILoggingService _logServ;
        const string PageName = "TicketService";

        public TicketService(IHttpContextAccessor httpAccessor, ITicketDAL ticketDAL, ISaveGetFiles saveFile, ILoggingService logServ)
        {
            _httpAccessor = httpAccessor;
            _ticketDAL = ticketDAL;
            _saveFile = saveFile;
            _logServ = logServ;
        }


        public ReturnResult FillQuestionStatus(int? userId)
        {
            IEnumerable<Enums.TicketStatus> questionstatusTypes = Enum.GetValues(typeof(Enums.TicketStatus))
                                                     .Cast<Enums.TicketStatus>();
            result.body = (from action in questionstatusTypes
                           select new SharedSelectListViewModel
                           {
                               Text = action.ToString(),
                               Value = ((int)action).ToString()
                           }).ToList();
            return result;
        }


        public async Task<ReturnResult> UpdateTicketStatus(int id,int status)
        {
            var question = new TicketListModel
            {
                Id = id,
                Status = status,
                UpdatedBy = _httpAccessor.GetUserIdFromJWT()
            };

            try
            {
                result.body = await _ticketDAL.UpdateTicketStatus(question);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateTicketStatus";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }

        public async Task<ReturnResult> GetTicketList(int id = 0)
        {
            var roleName = _httpAccessor.GetRoleFromJWT();

                if (roleName.ToLower().Contains("operator"))
                    return await GetOperatorTicketList();

                try
                {
                    result.body = await _ticketDAL.GetTicketList(id);
                }
                catch (Exception ex)
                {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetTicketList";
                await _logServ.LogError();
                
                    result.result = 0;
                }
            
            return result;
        }


        public async Task<ReturnResult> GetTicketListSales(int id = 0)
        {
            try
            {
                result.body = await _ticketDAL.GetTicketListForSales(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetTicketListSales";
                await _logServ.LogError();
                
                result.result = 0;
            }

            return result;
        }


        public async Task<ReturnResult> GetTicketDetails(int id = 0)
        {
            
                var ticketList = new TicketListModel();
            try
            {
                ticketList = await _ticketDAL.GetTicketDetails(id);
                var commentList = await _ticketDAL.GetTicketcomments(id);

                if(ticketList == null)
                {
                    result.result = 0;
                    result.error = "There is no matching record";
                    return result;
                }

                ticketList.comments = (IEnumerable<TicketComments>)commentList;

                result.body = ticketList;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetTicketDetails";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetOperatorTicketList(int operatorId = 0)
        {
            if(operatorId == 0)
            {
                operatorId = _httpAccessor.GetOperatorFromJWT();
            }

            try
            {
                result.body = await _ticketDAL.GetOperatorTicketList(operatorId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOperatorTicketList";
                await _logServ.LogError();
                
                result.result = 0;
            }

            return result;
        }


        public async Task<ReturnResult> AddComment(TicketComments model)
        {
            int userId = _httpAccessor.GetUserIdFromJWT();
            var ticket = new TicketListModel();
            ticket.UserId = model.UserId;
            ticket.Id = model.QuestionId;
            ticket.UpdatedBy = userId;
            ticket.Status = 2;
            try
            {
                // If updated by and userid are same will update as user.
                // If not will update as Admin or other than original user.
                var x = await _ticketDAL.UpdateTicketByUser(ticket);

                model.UserId = userId;

                model.CommentId = await _ticketDAL.AddNewComment(model);
               

                if (model.CommentImage != null && model.CommentImage.Length > 0)
                {
                    var extension = Path.GetExtension(model.CommentImage.FileName);
                    var commlower = extension.ToLower();
                    string[] exts = { ".png", ".jpeg", ".jpg", ".tif" };
                    if (!exts.Contains(extension.ToLower()))
                    {
                        result.result = 0;
                        result.error = "Please upload an image file only.";
                        return result;
                    }

                    var path = await SaveCommentImage(model, extension, userId);

                    if (path == "fail")
                    {
                        result.result = 0;
                        result.error = "The file could not be uploaded.";
                        return result;
                    }
                    else
                    {
                        model.ImageFile = path;
                        var z = await _ticketDAL.AddNewCommentImage(model);
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddComment";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
            return result;

        }

        private async Task<string> SaveCommentImage(TicketComments model, string extension, int userId)
        {
            try
            {
                // Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                // string filename = unixTimestamp.ToString() + "_" + userId.ToString() + extension;

                /// TODO: Need to sort file saving out
                string directoryName = "QuestionComment";

                var filename = await _saveFile.SaveFileToSite(directoryName, model.CommentImage);

                return directoryName + filename;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "SaveCommentImage";
                await _logServ.LogError();
                
                return "fail";
            }
        }

    }
}
