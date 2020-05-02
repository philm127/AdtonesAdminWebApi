using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    /// <summary>
    /// Class Md5Encrypt.
    /// </summary>
    public static class Md5Encrypt
    {
        /// <summary>
        /// MD5s the encrypt password.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public static string Md5EncryptPassword(string data)
        {
            var encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(data);
            byte[] hashed = MD5.Create().ComputeHash(bytes);
            return Encoding.UTF8.GetString(hashed);
        }
    }
}