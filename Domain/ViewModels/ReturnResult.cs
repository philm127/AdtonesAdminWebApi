using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ReturnResult
    {
        public int result { get; set; } = 1;
        public string error { get; set; }
        public Object body { get; set; }

        public int recordcount { get; set; }
    }
}
