﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IConnectionStringService
    {
        Task<string> GetSingleConnectionString(int Id);
        Task<IEnumerable<string>> GetConnectionStrings(int Id=0);
    }
}