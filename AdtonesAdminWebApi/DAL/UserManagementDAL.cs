using AdtonesAdminWebApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    
    public class CountryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public bool Active { get; set; }
    }
    public class UserManagementDAL : IUserManagementDAL
    {
        private readonly IBaseTestDAL _configuration;

        public UserManagementDAL(IBaseTestDAL config)
        {
            _configuration = config;
        }

        public List<CountryModel> GetList()
        {
            var listCountryModel = new List<CountryModel>();

            try
            {
                var x = "SELECT Id,Name FROM Country";
                SqlDataReader rdr = _configuration.GetAll(x);
                    while (rdr.Read())
                {
                    listCountryModel.Add(new CountryModel
                    {
                        Id = Convert.ToInt32(rdr[0]),
                        Name = rdr[1].ToString(),
                        // Active = Convert.ToBoolean(rdr[2])
                    });
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listCountryModel;
        }


        //public List<CountryModel> GetList()
        //{
        //    var listCountryModel = new List<CountryModel>();

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            SqlCommand cmd = new SqlCommand("SELECT Id,Name FROM Country", con);
        //            // cmd.CommandType = CommandType.StoredProcedure;
        //            con.Open();
        //            SqlDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                listCountryModel.Add(new CountryModel
        //                {
        //                    Id = Convert.ToInt32(rdr[0]),
        //                    Name = rdr[1].ToString(),
        //                    // Active = Convert.ToBoolean(rdr[2])
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return listCountryModel;
        //}

    }
}