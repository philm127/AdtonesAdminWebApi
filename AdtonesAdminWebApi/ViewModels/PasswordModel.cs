﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class PasswordModel
    {
        public int Userid { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
