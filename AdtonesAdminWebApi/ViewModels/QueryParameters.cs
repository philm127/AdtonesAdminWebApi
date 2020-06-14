using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class QueryParameters
    {
        private const int maxPageCount = 50;
        public int pageNumber { get; set; } = 1;

        private int _pageCount = maxPageCount;
        public int pageSize
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }

        public string filter { get; set; }

        public string sortOrder { get; set; } = "Name";
    }
}
