﻿

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class AdvertQuery
    {
        public static string GetAdvertResultSet => @"SELECT ad.AdvertId,ad.UserId,ad.ClientId,ad.AdvertName,ad.Brand,cprof.SmsBody,cprof.EmailBody,
                                                ISNULL(cl.Name,'-') AS ClientName,ad.OperatorId,cad.CampaignProfileId,
                                                CONCAT(usr.FirstName,' ',usr.LastName) AS UserName, usr.Email,ad.CreatedDateTime AS CreatedDate,
                                                ad.Script,ad.Status,ad.MediaFileLocation,ad.UploadedToMediaServer,SoapToneCode,
                                                CASE WHEN ad.MediaFileLocation IS NULL THEN ad.MediaFileLocation 
                                                    ELSE CONCAT(@siteAddress,ad.MediaFileLocation) END AS MediaFile,
                                                CASE WHEN ad.ScriptFileLocation IS NULL THEN ad.ScriptFileLocation 
                                                    ELSE CONCAT(@siteAddress,ad.ScriptFileLocation) END AS ScriptFileLocation,ad.SoapToneId
                                                FROM Advert AS ad LEFT JOIN Client AS cl ON ad.ClientId=cl.Id
                                                LEFT JOIN Users AS usr ON usr.UserId=ad.UserId
                                                LEFT JOIN CampaignAdverts AS cad ON cad.AdvertId=ad.AdvertId
                                                LEFT JOIN CampaignProfile AS cprof ON cprof.CampaignProfileId=cad.CampaignProfileId";

        

        public static string GetAdvertCategoryDataTable => @"SELECT AdvertCategoryId,ad.Name AS CategoryName,ad.CountryId, ISNULL(c.Name,'-') AS CountryName, ad.CreatedDate
                                                        FROM AdvertCategories AS ad INNER JOIN Country AS c ON c.Id = ad.CountryId
                                                        LEFT JOIN Operators AS op ON op.CountryId=ad.CountryId";


        public static string UpdateAdvertStatus => @"UPDATE Advert SET Status=@Status,UpdatedBy=@UpdatedBy, UpdatedDateTime=GETDATE() WHERE ";


        public static string InsertAdvertRejection => @"INSERT INTO AdvertRejections(AdvertId,UserId,CreatedDateRejectionReason ) VALUES(@AdvertId,@UserId,GETDATE(),@RejectionReason);";


        public static string GetFtpDetails => @"SELECT OperatorFTPDetailId,Host,Port,UserName,Password,FtpRoot FROM OperatorFTPDetails WHERE OperatorId=@OperatorId";


        public static string UpdateMediaLoaded => "UPDATE Advert SET UploadedToMediaServer = true WHERE AdvertId=@advertId;";


        public static string RejectAdvertReason => @"INSERT INTO AdvertRejections(UserId,AdvertId,RejectionReason,CreatedDate,AdtoneServerAdvertRejectionId)
                                                                    VALUES(@UserId,@AdvertId,@RejectionReason,GETDATE(),@AdtoneServerAdvertRejectionId);
                                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string DeleteAdvertCategory => @"DELETE FROM AdvertCategories WHERE ";


        public static string UpdateAdvertCategory => @"UPDATE AdvertCategories SET CountryId=@countryId, Name=@name, UpdatedDate=GETDATE()
                                                                                                                                WHERE ";


        public static string AddAdvertCategory => @"INSERT INTO AdvertCategories (CountryId,Name,CreatedDate,UpdatedDate,AdtoneServerAdvertCategoryId)
                                                        VALUES(@countryId,@name,GETDATE(),GETDATE(),@Id);
                                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
    }
}
