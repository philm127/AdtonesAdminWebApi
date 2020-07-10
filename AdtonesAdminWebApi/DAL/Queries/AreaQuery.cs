using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class AreaQuery
    {
        public static string LoadAreaDataTable => @"SELECT a.AreaId, a.AreaName,a.CountryId,c.Name as CountryName 
                                                                FROM Areas AS a INNER JOIN Country AS c
                                                                ON a.CountryId=c.Id
                                                                WHERE a.IsActive=1
                                                                ORDER BY a.AreaId DESC";


        public static string AddArea => @"INSERT INTO Areas(AreaName,IsActive,CountryId) 
                                    VALUES(@AreaName,true,@CountryId)";


        public static string GetAreaById => @"SELECT a.AreaId, a.AreaName,a.CountryId,c.Name as CountryName,a.IsActive 
                                                            FROM Areas AS a INNER JOIN Country AS c
                                                            ON a.CountryId=c.Id
                                                            WHERE a.AreaId = @areaid";


        public static string DeleteArea => @"DELETE FROM Areas WHERE a.AreaId = @areaid";


        public static string UpdateArea => @"UPDATE Areas SET AreaName=@AreaName,IsActive=@IsActive,CountryId=@CountryId WHERE AreaId=@AreaId";


        public static string CheckAreaExists => @"SELECT COUNT(1) FROM Areas WHERE LOWER(AreaName) = @areaname AND CountryId=@countryId";
    }

}
