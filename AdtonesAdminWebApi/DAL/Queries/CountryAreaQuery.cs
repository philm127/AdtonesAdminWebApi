using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class CountryAreaQuery
    {

        public static string LoadCountryDataTable => @"SELECT c.Id,c.Name,c.ShortName,c.CountryCode,c.CreatedDate,c.Status,
                                                        ISNULL(t.TaxPercantage,0) AS TaxPercentage 
                                                        FROM Country AS c INNER JOIN CountryTax AS t ON t.CountryId=c.Id ";


        public static string GetCountry => @"SELECT c.Id,c.Name,ShortName,c.CountryCode,c.CreatedDate,c.Status,
                                            t.TaxPercantage AS TaxPercentage,TermAndConditionFileName
                                            FROM Country AS c INNER JOIN CountryTax AS t ON t.CountryId=c.Id
                                            WHERE c.Id=@id";


        public static string CheckCountryExists => @"SELECT COUNT(1) FROM Country WHERE LOWER(Name) = @name";


        public static string AddCountry => @"INSERT INTO Country(UserId,Name,ShortName,CreatedDate,UpdatedDate,Status,
                                                    TermAndConditionFileName,CountryCode,AdtoneServeCountryId)
                                            VALUES(@UserId,@Name,@ShortName, GETDATE(), GETDATE(),1, 
                                                @TermAndConditionFileName, @CountryCode,@AdtoneServeCountryId);
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string AddTax => @"INSERT INTO CountryTax(UserId,CountryId,TaxPercantage,CreatedDate,UpdatedDate,Status)
                                            VALUES(@UserId,@CountryId,@TaxPercantage, GETDATE(), GETDATE(),1);";


        public static string UpdateCountry => @"UPDATE Country SET UserId = @UserId, Name = @Name, ShortName = @ShortName, 
                                                            UpdatedDate = GETDATE(),CountryCode = @CountryCode,
                                                            TermAndConditionFileName = @TermAndConditionFileName WHERE Id=@Id;";


        public static string UpdateTax => @"Update CountryTax SET UserId=@UserId,TaxPercantage=@TaxPercantage,
                                            UpdatedDate=GETDATE() WHERE CountryId=@CountryId;";


        public static string LoadAreaDataTable => @"SELECT ad.AreaId, ad.AreaName,ad.CountryId,c.Name as CountryName 
                                                                FROM Areas AS ad INNER JOIN Country AS c
                                                                ON ad.CountryId=c.Id
                                                                WHERE ad.IsActive=1";


        public static string AddArea => @"INSERT INTO Areas(AreaName,IsActive,CountryId) 
                                            VALUES(@AreaName,1,@CountryId)";


        public static string GetAreaById => @"SELECT a.AreaId, a.AreaName,a.CountryId,c.Name as CountryName,a.IsActive 
                                                            FROM Areas AS a INNER JOIN Country AS c
                                                            ON a.CountryId=c.Id
                                                            WHERE a.AreaId = @areaid";


        public static string DeleteArea => @"DELETE FROM Areas WHERE AreaId = @areaid";


        public static string UpdateArea => @"UPDATE Areas SET AreaName=@AreaName,IsActive=1 WHERE AreaId=@AreaId";


        public static string CheckAreaExists => @"SELECT COUNT(1) FROM Areas WHERE LOWER(AreaName) = @areaname AND CountryId=@countryId";
    }

}
