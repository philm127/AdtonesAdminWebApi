using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class AllocationList
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
    }


    public class SalesAdAllocationModel
    {
        public int user1 { get; set; }
        public int user2 { get; set; }

        public List<AllocationList> user1array { get; set; }
        public List<AllocationList> user2array { get; set; }
    }
}
