using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Caching.Memory;

namespace AdtonesAdminWebApi.DAL
{
    /// <summary>
    /// Robbed and adapted from Anatoly
    /// </summary>
    public static class CurrencyDefaults
    {
        public const string DefaultCurrencySymbol = "$";
        public const string DefaultCurrencyCode = "USD";
    }

    public class CurrencyDAL : ICurrencyDAL
    {

        private readonly Dictionary<int, Currency> _countryidToCur = new Dictionary<int, Currency>();
        private readonly Dictionary<int, Currency> _curIdToCur = new Dictionary<int, Currency>();
        private readonly Dictionary<int, Currency> _userIdToCur = new Dictionary<int, Currency>();
        private readonly IUserManagementDAL _userDAL;
        private readonly IExecutionCommand _executers;
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private IMemoryCache cache;
        public string _dbQuery { get; private set; }

        public CurrencyDAL(IUserManagementDAL userDAL, IExecutionCommand executers,
                            IConfiguration configuration, IMemoryCache cache)
        {
            _userDAL = userDAL;
            _executers = executers;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            this.cache = cache;
        }


        /// <summary>
        /// Returns Currency for specified User. Async implementation.
        /// If user has preferred Currency code selected on a web page - it will be used, otherwise will be user User's original Country currency.
        /// </summary>
        /// <param name="userId">UserId for which to lookup currency.</param>
        /// <returns>Currency to be used for conversion.</returns>
        public async Task<Currency> GetDisplayCurrencyCodeForUserAsync(int userId)
        {
            Currency result;
                        if (_userIdToCur.TryGetValue(userId, out result))
                return result;

            var contact = await _userDAL.getContactByUserId(userId);
            if (contact?.CurrencyId.HasValue ?? false)
            {
                result = await GetCurrencyUsingCurrencyIdAsync(contact.CurrencyId);
                _userIdToCur[userId] = result;
                return result;
            }
            result = await GetCurrencyUsingCountryIdAsync(contact?.CountryId);
            _userIdToCur[userId] = result;
            return result;

        }

        public async Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CurrencyQuery.GetCurrencyByCurrencyId);
            builder.AddParameters(new { currencyId = currencyId.Value });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<Currency>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<Currency> GetCurrencyUsingCountryIdAsync(int? countryId)
        {
            if (!countryId.HasValue)
               return await GetCurrencyFromEitherCacheOrDbAsync(CurrencyQuery.GetCurrencyByCurrency, new { CurrencyCode = CurrencyDefaults.DefaultCurrencyCode },0, _countryidToCur);
            return await GetCurrencyFromEitherCacheOrDbAsync(CurrencyQuery.GetCurrencyByCountry, new { CountryId = countryId.Value }, countryId.Value, _countryidToCur);
        }


        

        //public async Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId)
        //{
        //    if (!currencyId.HasValue)
        //        return await GetCurrencyFromEitherCacheOrDbAsync(CurrencyQuery.GetCurrencyByCurrency, new { CurrencyCode = CurrencyDefaults.DefaultCurrencyCode }, 0, _curIdToCur);
        //    return await GetCurrencyFromEitherCacheOrDbAsync(CurrencyQuery.GetCurrencyByCurrency, new { CurrencyCode = currencyId.Value }, currencyId.Value, _curIdToCur);
        //}

        private async Task<Currency> GetCurrencyFromEitherCacheOrDbAsync(string sqlQuery, object param, int lookup, Dictionary<int, Currency> cacheSource)
        {
            Dictionary<int, Currency> cache = cacheSource;
            Currency result;
            if (!cache.TryGetValue(lookup, out result))
            {
                result = await GetCurrencyFromDbAsync(sqlQuery,param);
                cache[lookup] = result;
            }

            return result;
        }


        private async Task<Currency> GetCurrencyFromDbAsync(string sqlQuery,object param)
        {
            return await _executers.ExecuteCommand(_connStr,
                                        conn => conn.QueryFirstOrDefault<Currency>(
                                        sqlQuery,param));
        }
    }
}