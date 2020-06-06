﻿

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IAdvertQuery
    {
        string GetAdvertResultSet { get; }
        string GetAdvertCategoryDataTable { get; }
        string GetAdvertDetail { get; }
        string UpdateAdvertStatus { get; }
        string InsertAdvertRejection { get; }
        string GetFtpDetails { get; }
        string UpdateMediaLoaded { get; }
    }


    public class AdvertQuery : IAdvertQuery
    {
        public string GetAdvertResultSet => @"SELECT ad.AdvertId,ad.UserId,ad.ClientId,ad.AdvertName,ad.Brand,cprof.SmsBody,cprof.EmailBody,
                                                ISNULL(cl.Name,'-') AS ClientName,ad.OperatorId,cad.CampaignProfileId,
                                                CONCAT(usr.FirstName,' ',usr.LastName) AS UserName, usr.Email,ad.CreatedDateTime AS CreatedDate,
                                                ad.Script,ad.Status,ad.MediaFileLocation AS MediaFile,ad.UploadedToMediaServer,
                                                CASE WHEN ad.MediaFileLocation IS NULL THEN ad.MediaFileLocation 
                                                    ELSE CONCAT(@siteAddress,ad.MediaFileLocation) END AS MediaFileLocation,
                                                CASE WHEN ad.ScriptFileLocation IS NULL THEN ad.ScriptFileLocation 
                                                    ELSE CONCAT(@siteAddress,ad.ScriptFileLocation) END AS ScriptFileLocation
                                                FROM Advert AS ad LEFT JOIN Client AS cl ON ad.ClientId=cl.Id
                                                LEFT JOIN Users AS usr ON usr.UserId=ad.UserId
                                                LEFT JOIN CampaignAdverts AS cad ON cad.AdvertId=ad.AdvertId
                                                LEFT JOIN CampaignProfile AS cprof ON cprof.CampaignProfileId=cad.CampaignProfileId";


        public string GetAdvertDetail => GetAdvertResultSet + " Where ad.AdvertId=@Id";
        

        public string GetAdvertCategoryDataTable => @"SELECT AdvertCategoryId,ac.Name AS CategoryName,ac.CountryId, ISNULL(c.Name,'-') AS CountryName, ac.CreatedDate
                                                        FROM AdvertCategories AS ac INNER JOIN Country AS c ON c.Id = ac.CountryId;";


        public string UpdateAdvertStatus => @"UPDATE Advert SET Status=@Status,UpdatedBy=@UpdatedBy, UpdatedDateTime=GETDATE() WHERE ";


        public string InsertAdvertRejection => @"INSERT INTO AdvertRejections(AdvertId,UserId,CreatedDateRejectionReason ) VALUES(@AdvertId,@UserId,GETDATE(),@RejectionReason);";


        public string GetFtpDetails => @"SELECT OperatorFTPDetailId,Host,Port,UserName,Password,FtpRoot FROM OperatorFTPDetails WHERE OperatorId=@OperatorId";


        public string UpdateMediaLoaded => "UPDATE Advert SET UploadedToMediaServer = true WHERE AdvertId=@advertId;";
    }
}
