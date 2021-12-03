using AdtonesAdminWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    
    
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        public Contacts Contacts { get; set; }
    }

    public class UserFullDetailsViewModel : UserDetailsViewModel
    {
        public CompanyDetails CompanyDetails { get; set; }
    }
}
