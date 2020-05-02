using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public class EncryptionHelper
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="EncryptionHelper"/> class from being created.
        /// </summary>
        private EncryptionHelper()
        {
        }

        /// <summary>
        /// Encrypts the query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>NameValueCollection.</returns>
        public static NameValueCollection EncryptQueryString(NameValueCollection queryString)
        {
            var newQueryString = new NameValueCollection();
            string nm = String.Empty;
            string val = String.Empty;

            foreach (string name in queryString)
            {
                nm = name;
                val = queryString[name];
                newQueryString.Add(Hex(nm), Hex(val));
            }

            return newQueryString;
        }

        /// <summary>
        /// Decrypts the query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>NameValueCollection.</returns>
        public static NameValueCollection DecryptQueryString(NameValueCollection queryString)
        {
            var newQueryString = new NameValueCollection();
            string nm;
            string val;

            foreach (string name in queryString)
            {
                nm = DeHex(name);
                val = DeHex(queryString[name]);
                newQueryString.Add(nm, val);
            }

            return newQueryString;
        }

        /// <summary>
        /// Encrypts the single value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string EncryptSingleValue(string input)
        {
            return Hex(input);
        }

        /// <summary>
        /// Decrypts the single value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string DecryptSingleValue(string input)
        {
            return DeHex(input);
        }

        /// <summary>
        /// Des the hexadecimal.
        /// </summary>
        /// <param name="hexstring">The hexstring.</param>
        /// <returns>System.String.</returns>
        public static string DeHex(string hexstring)
        {
            string ret = String.Empty;
            var sb = new StringBuilder(hexstring.Length / 2);
            for (int i = 0; i <= hexstring.Length - 1; i = i + 2)
            {
                sb.Append((char)int.Parse(hexstring.Substring(i, 2), NumberStyles.HexNumber));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Hexadecimals the specified s data.
        /// </summary>
        /// <param name="sData">The s data.</param>
        /// <returns>System.String.</returns>
        public static string Hex(string sData)
        {
            string temp = String.Empty;
            ;
            string newdata = String.Empty;
            var sb = new StringBuilder(sData.Length * 2);
            for (int i = 0; i < sData.Length; i++)
            {
                if ((sData.Length - (i + 1)) > 0)
                {
                    temp = sData.Substring(i, 2);
                    if (temp == @"\n") newdata += "0A";
                    else if (temp == @"\b") newdata += "20";
                    else if (temp == @"\r") newdata += "0D";
                    else if (temp == @"\c") newdata += "2C";
                    else if (temp == @"\\") newdata += "5C";
                    else if (temp == @"\0") newdata += "00";
                    else if (temp == @"\t") newdata += "07";
                    else
                    {
                        sb.Append(String.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
                        i--;
                    }
                }
                else
                {
                    sb.Append(String.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
                }
                i++;
            }
            return sb.ToString();
        }
    }
}