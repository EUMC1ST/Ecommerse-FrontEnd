using EcommerseClient.Models;
using System;

namespace EcommerseClient.Services
{
    public class CurrencyService
    {
        public static string BaseUrl = Environment.GetEnvironmentVariable("CurrencyService");
        public static string cur = "";
        public static int vista = 1;
        public static double Conversion(CurrencyChange input)
        {
            return new HttpRequests().ThePost<CurrencyChange, double>(input, "api/currency/conversion", BaseUrl);
        }
    }
}
