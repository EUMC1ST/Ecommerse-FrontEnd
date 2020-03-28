using EcommerseClient.Models;

namespace EcommerseClient.Services
{
    public class CurrencyService
    {
        public static string BaseUrl = "http://localhost:5004/";

        public static double Conversion(CurrencyChange input)
        {
            return new HttpRequests().ThePost<CurrencyChange, double>(input, "api/currency/conversion", BaseUrl);
        }
    }
}
