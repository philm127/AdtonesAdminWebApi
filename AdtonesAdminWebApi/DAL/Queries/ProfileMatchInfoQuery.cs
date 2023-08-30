using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class ProfileMatchInfoQuery
    {


        public static string DeleteMatchLabel => @"DELETE FROM ProfileMatchLabels WHERE Id=@id";



    }
}
