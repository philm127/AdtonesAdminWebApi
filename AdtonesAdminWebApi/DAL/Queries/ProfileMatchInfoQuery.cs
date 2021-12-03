using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class ProfileMatchInfoQuery
    {

        public static string UpdateProfileInfoLabel => @"UPDATE ProfileMatchLabels SET ProfileLabel=@ProfileLabel,
                                                        UpdatedDate=GETDATE() WHERE Id=@Id";


        public static string InsertProfileInfoLabel => @"INSERT INTO ProfileMatchLabels(ProfileLabel,ProfileMatchInformationId,CreatedDate,UpdatedDate) 
                                                        VALUES(@ProfileLabel,@ProfileMatchInformationId,GETDATE(),GETDATE());";


        public static string DeleteMatchLabel => @"DELETE FROM ProfileMatchLabels WHERE Id=@id";



    }
}
