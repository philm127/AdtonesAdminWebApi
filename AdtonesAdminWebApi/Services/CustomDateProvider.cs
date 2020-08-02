using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AdtonesAdminWebApi.Services
{
    public class CustomDateProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is DateTime)) throw new NotSupportedException();

            var dt = (DateTime)arg;

            string suffix;

            if (new[] { 11, 12, 13 }.Contains(dt.Day))
            {
                suffix = "<sup>th</sup>";
            }
            else if (dt.Day % 10 == 1)
            {
                suffix = "<sup>st</sup>";
            }
            else if (dt.Day % 10 == 2)
            {
                suffix = "<sup>nd</sup>";
            }
            else if (dt.Day % 10 == 3)
            {
                suffix = "<sup>rd</sup>";
            }
            else
            {
                suffix = "<sup>th</sup>";
            }

            return HttpUtility.HtmlDecode(string.Format("{1}{2} {0:MMMM} {0:yyyy}", arg, dt.Day, suffix));
        }
    }
}