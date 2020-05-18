using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IAreaQuery
    {
        string LoadAreaDataTable { get; }
        string AddArea { get; }
        string GetAreaById { get; }
        string DeleteArea { get; }
        string UpdateArea { get; }
    }


    public class AreaQuery : IAreaQuery
    {
        public string LoadAreaDataTable => @"SELECT a.AreaId, a.AreaName,a.CountryId,c.Name as CountryName 
                                                                FROM Areas AS a INNER JOIN Country AS c
                                                                ON a.CountryId=c.Id
                                                                WHERE a.IsActive=1
                                                                ORDER BY a.AreaId DESC";


        public string AddArea => @"INSERT INTO Areas(AreaName,IsActive,CountryId) 
                                    VALUES(@AreaName,true,@CountryId)";


        public string GetAreaById => @"SELECT a.AreaId, a.AreaName,a.CountryId,c.Name as CountryName,a.IsActive 
                                                            FROM Areas AS a INNER JOIN Country AS c
                                                            ON a.CountryId=c.Id
                                                            WHERE a.AreaId = @areaid";


        public string DeleteArea => @"DELETE FROM Areas WHERE a.AreaId = @areaid";


        public string UpdateArea => @"UPDATE Areas SET AreaName=@AreaName,IsActive=@IsActive,CountryId=@CountryId WHERE AreaId=@AreaId";


    }

}
