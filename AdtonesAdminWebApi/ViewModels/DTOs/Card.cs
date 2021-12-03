using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs
{
    public class Card
    {
        public string Expiry { get; set; }
        public string MerchantSessionKey { get; set; }
        public string StatusCode { get; set; }
        public List<Errors> errors { get; set; }
    }

    public class CardIdentifiers
    {
        public string CardIdentifier { get; set; }
        public DateTime Expiry { get; set; }
        public string CardType { get; set; }
        public List<Errors> errors { get; set; }
        public string StatusDetail { get; set; }
    }


    public class Output
    {
        public string Status { get; set; }
        public string StatusDetail { get; set; }
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public List<Errors> errors { get; set; }
    }


    public class CardProcessedResult
    {
        public string TransactionId { get; set; }
        public string clientMessage { get; set; }
        public string errors { get; set; }
        public string errorCode { get; set; }
    }

    public class Errors
    {
        public string description { get; set; }
        public string property { get; set; }
        public string clientMessage { get; set; }
        public string code { get; set; }
    }
}
