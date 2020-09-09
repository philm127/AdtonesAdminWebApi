using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class ProfileMatchInfoQuery
    {
        public static string LoadDataTable => @"SELECT prof.Id,prof.ProfileName,
                                                prof.ProfileType,prof.CountryId, c.Name AS CountryName,prof.IsActive 
                                                FROM ProfileMatchInformations AS prof 
                                                LEFT JOIN Country AS c ON prof.CountryId=c.Id 
                                                LEFT JOIN Operators AS op ON op.CountryId=prof.CountryId ";


        public static string GetProfileInfo => @"SELECT p.Id, p.ProfileName,p.IsActive,
                                                p.ProfileType,p.CountryId
                                                FROM ProfileMatchInformations p
                                                WHERE p.Id=@id";


        public static string GetProfileInfoLabels => @"SELECT Id,ProfileLabel,ProfileMatchInformationId,
                                                    FORMAT(CreatedDate, 'd', 'en-gb') as CreatedDate
                                                    FROM ProfileMatchLabels
                                                    WHERE ProfileMatchInformationId=@id";


        public static string CheckIfProfileExists => @"SELECT COUNT(1) FROM ProfileMatchInformations 
                            WHERE LOWER(ProfileName) = @profilename
                            AND CountryId=@countryId AND LOWER(ProfileType)=@profileType";



        public static string AddProfileInfo => @"INSERT INTO ProfileMatchInformations(ProfileName,IsActive,CountryId,
                                                            CreatedDate,UpdatedDate,ProfileType)
                                                 VALUES(@ProfileName,@IsActive,@CountryId,GETDATE(),GETDATE(),@ProfileType);
                                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";



        public static string UpdateProfileInfo => @"UPDATE ProfileMatchInformations SET IsActive=@IsActive,
                                                    UpdatedDate=GETDATE(),ProfileType=@ProfileType
                                                    WHERE Id=@Id";


        public static string UpdateProfileInfoLabel => @"UPDATE ProfileMatchLabels SET ProfileLabel=@ProfileLabel,
                                                        UpdatedDate=GETDATE() WHERE Id=@Id";


        public static string InsertProfileInfoLabel => @"INSERT INTO ProfileMatchLabels(ProfileLabel,ProfileMatchInformationId,CreatedDate,UpdatedDate) 
                                                        VALUES(@ProfileLabel,@ProfileMatchInformationId,GETDATE(),GETDATE());";


        public static string DeleteMatchLabel => @"DELETE FROM ProfileMatchLabels WHERE Id=@id";



    }
}
