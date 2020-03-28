using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Credit_number_Card { get; set; }
        public string Credit_Card_Month { get; set; }
        public string Credit_Card_Year { get; set; }
        public int CVV { get; set; }
        public string Name { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string CurrencyExchange { get; set; }
    }
}
