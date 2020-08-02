using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public interface IGeneralPemissionAssignment
    {
        (StringBuilder sbuild, SqlBuilder build) CheckGeneralFile(StringBuilder sb, SqlBuilder builder, string pais = null, string ops = null, string advs = null, string test = null);
    }


    public class GeneralPemissionAssignment : IGeneralPemissionAssignment
    {
        private readonly IWebHostEnvironment _env;

        public GeneralPemissionAssignment(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Takes the arrays from general permissions to allow access by country, operator, advertisers or all 3.
        /// </summary>
        /// <param name="sb">Stringbuilder of query so far</param>
        /// <param name="builder">A dapper builder holding parameters submitted so far</param>
        /// <param name="pais">This is to allow flexibility to the virtual table name when using country array</param>
        /// <param name="ops">This is to allow flexibility to the virtual table name when using operator array</param>
        /// <param name="advs">This is to allow flexibility to the virtual table name when using advertiser array</param>
        /// <param name="test">This will take op or null, op is for testing local json file for operator before we take data from table</param>
        /// <returns></returns>
        public (StringBuilder sbuild, SqlBuilder build) CheckGeneralFile(StringBuilder sb, SqlBuilder builder, string pais = null, string ops = null, string advs = null, string test = null)
        {

            var directoryName = string.Empty;
            var dir = "\\TempGenPermissions\\general.json";
            var otherpath = _env.ContentRootPath;
            if (test == "op")
                directoryName = System.IO.File.ReadAllText("C:\\Development\\Adtones-Admin\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\TempGenPermissions\\generalOp.json");//Path.Combine(otherpath, dir));
            else
                directoryName = System.IO.File.ReadAllText("C:\\Development\\Adtones-Admin\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\TempGenPermissions\\general.json");

            PermissionModel gen = JsonSerializer.Deserialize<PermissionModel>(directoryName);

            var els = gen.elements.ToList();

            int[] country = els.Find(x => x.name == "country").arrayId.ToArray();
            // operators plural as operator is a key word
            int[] operators = els.Find(x => x.name == "operator").arrayId.ToArray();
            int[] advertiser = els.Find(x => x.name == "advertiser").arrayId.ToArray();

            if (country.Length > 0 && pais != null)
            {
                sb.Append($" AND {pais}.CountryId IN @country ");
                builder.AddParameters(new { country = country.ToArray() });

            }
            if (operators.Length > 0 && ops != null)
            {
                sb.Append($" AND [ops].OperatorId IN @operators ");
                builder.AddParameters(new { operators = operators.ToArray() });
            }

            if (advertiser.Length > 0 && advs != null)
            {
                sb.Append($" AND {advs}.UserId IN @advertiser ");
                builder.AddParameters(new { advertisers = advertiser.ToArray() });
            }

            return (sb, builder);
        }
    }
}