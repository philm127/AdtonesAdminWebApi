

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IAdvertQuery
    {
        string GetAdvertResultSet { get; }
        string GetAdvertCategoryDataTable { get; }
        string GetAdvertDetail { get; }
        string UpdateAdvertStatus { get; }
        string InsertAdvertRejection { get; }
    }


    public class AdvertQuery : IAdvertQuery
    {
        public string GetAdvertResultSet => @"SELECT ad.AdvertId,ad.UserId,ad.ClientId,ad.AdvertName,ad.Brand,ad.MediaFileLocation,ISNULL(cl.Name,'-') AS ClientName,
                                                CONCAT(usr.FirstName,' ',usr.LastName) AS UserName, usr.Email,ad.CreatedDateTime AS CreatedDate,ad.Script,ad.Status,
                                                CASE WHEN ad.ScriptFileLocation IS NULL THEN ad.ScriptFileLocation 
                                                    ELSE @siteAddress + ad.ScriptFileLocation END AS ScriptFileLocation
                                                FROM Advert AS ad LEFT JOIN Client AS cl ON ad.ClientId=cl.Id
                                                LEFT JOIN Users AS usr ON usr.UserId=ad.UserId;";


        public string GetAdvertDetail => GetAdvertResultSet + " Where ad.AdvertId=@Id";
        

        public string GetAdvertCategoryDataTable => @"SELECT AdvertCategoryId,ac.Name,ac.CountryId, ISNULL(c.Name,'-') AS CountryName, ac.CreatedDate
                                                        FROM AdvertCategories AS ac INNER JOIN Country AS c ON c.Id = ac.CountryId;";


        public string UpdateAdvertStatus => @"UPDATE Advert SET Status=@Status,@UpdatedBy=UpdatedBy, UpdatedDateTime=GETDATE() WHERE ";


        public string InsertAdvertRejection => @"INSERT INTO AdvertRejections(AdvertId,UserId,CreatedDateRejectionReason ) VALUES(@AdvertId,@UserId,GETDATE(),@RejectionReason);";

    }
}
