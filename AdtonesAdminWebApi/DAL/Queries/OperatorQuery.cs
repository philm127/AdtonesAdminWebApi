using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class OperatorQuery
    {
        public static string LoadOperatorDataTable => @"SELECT OperatorId,OperatorName,ISNULL(co.Name, '-') AS CountryName,op.IsActive,
                                                        EmailCost,SmsCost,cu.CurrencyCode AS Currency
                                                        FROM Operators AS op LEFT JOIN Country co ON op.CountryId=co.Id
                                                        LEFT JOIN Currencies cu ON cu.CurrencyId = op.CurrencyId";


        public static string CheckIfOperatorExists => @"SELECT COUNT(1) FROM Operators WHERE LOWER(OperatorName) = @op;";


        public static string AddNewOperator => @"INSERT INTO Operators(OperatorName,CountryId,IsActive,EmailCost,SmsCost,CurrencyId, AdtoneServerOperatorId)
                                                    VALUES(@OperatorName,@CountryId,1,@EmailCost,@SmsCost,@CurrencyId, @AdtoneServerOperatorId);
                                                  SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string GetOperatorById => @"SELECT op.OperatorId,OperatorName,co.Name AS CountryName,cu.CurrencyCode,AdtoneServerOperatorId,
	                                                IsActive,EmailCost,SmsCost,op.CurrencyId,op.CountryId
                                                    FROM Operators AS op LEFT JOIN Country AS co ON op.CountryId=co.Id
                                                    LEFT JOIN Currencies AS cu ON op.CurrencyId=cu.CurrencyId WHERE OperatorId=@Id";


        public static string UpdateOperator => @"UPDATE Operators SET IsActive=@IsActive,EmailCost=@EmailCost,SmsCost=@SmsCost 
                                                    WHERE OperatorId=@OperatorId";


        public static string GetMaxAdvertResultSet => @"SELECT OperatorMaxAdvertId,KeyName,KeyValue,Addeddate AS CreatedDate,maxad.OperatorId,op.OperatorName
                                                        FROM OperatorMaxAdverts AS maxad INNER JOIN Operators AS op ON op.OperatorId=maxad.OperatorId";


        public static string AddOperatorMaxAdvert => @"INSERT INTO OperatorMaxAdverts(KeyName,KeyValue,Addeddate,Updateddate,OperatorId,AdtoneServerOperatorMaxAdvertId)
                                                            VALUES(@KeyName,@KeyValue,GETDATE(),GETDATE(),@OperatorId,@AdtoneServerOperatorMaxAdvertId);
                                                        SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string CheckIfMaxAdvertExists => @"SELECT COUNT(1) FROM OperatorMaxAdverts WHERE LOWER(KeyName) = @keyname AND OperatorId=@opid;";



        public static string GetOperatorMaxAdvertById => @"SELECT OperatorMaxAdvertId,KeyName,KeyValue,op.OperatorName,maxad.OperatorId
                                                         FROM OperatorMaxAdverts AS maxad INNER JOIN Operators AS op ON op.OperatorId=maxad.OperatorId
                                                          WHERE OperatorMaxAdvertId=@Id";


        public static string UpdateOperatorMaxAdvert => @"UPDATE OperatorMaxAdverts SET KeyValue=@KeyValue,Updateddate=GETDATE() ";


    }
}
