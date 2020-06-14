using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class PagingSearchClass
    {
        public int elementId { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 20;
        public string sort { get; set; } = "PlayLengthTicks";
        public string direction { get; set; } = "ASC";

        public string search { get; set; }
    }

    public class PageSearch
    {
        public List<PageSearchModel> searchList { get; set; }
    }

    public class PageSearchModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Operator { get; set; }
        public string country { get; set; }
        public string TypeName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? NumberFrom { get; set; }
        public int? NumberTo { get; set; }
        public int? NumberFrom2 { get; set; }
        public int? NumberTo2 { get; set; }
        public int? NumberFrom3 { get; set; }
        public int? NumberTo3 { get; set; }
        public string Client { get; set; }
        public string fullName { get; set; }
        public string Payment { get; set; }
        public DateTime? responseFrom { get; set; }
        public DateTime? responseTo { get; set; }
    }
}
