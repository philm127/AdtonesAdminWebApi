

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class AdvertQuery
    {


        public static string CheckAdvertNameExists => @"SELECT COUNT(1) FROM Advert WHERE LOWER(AdvertName)=@Id AND UserId=@UserId;";

    }
}
