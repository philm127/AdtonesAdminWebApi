using AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.Dto;
using AdtonesAdminWebApi.CrmApp.Core.Entities;
using AdtonesAdminWebApi.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.Queries
{
    public class GetAllSubscribersQuery : IRequest<ReturnResult>
    {
        public int elementId { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 20;
        public string sort { get; set; }
        public string direction { get; set; } = "ASC";

        public string search { get; set; }
    }

    //public class PageSearch
    //{
    //    public List<PageSearchModel> searchList { get; set; }
    //}

    //public class PageSearchModel
    //{
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //    public string Status { get; set; }
    //    public string Operator { get; set; }
    //    public string country { get; set; }
    //    public string TypeName { get; set; }
    //    public DateTime? DateFrom { get; set; }
    //    public DateTime? DateTo { get; set; }
    //    public double? NumberFrom { get; set; }
    //    public double? NumberTo { get; set; }
    //    public double? NumberFrom2 { get; set; }
    //    public double? NumberTo2 { get; set; }
    //    public double? NumberFrom3 { get; set; }
    //    public double? NumberTo3 { get; set; }
    //    public string Client { get; set; }
    //    public string fullName { get; set; }
    //    public string Payment { get; set; }
    //    public DateTime? responseFrom { get; set; }
    //    public DateTime? responseTo { get; set; }
    //}
}