using EcommerseClient.Models;

namespace EcommerseClient.Services
{
    public class CheckoutService 
    {
        public static string BaseUrl = "http://localhost:5008/";

        public static CheckoutModel Checkout(UserInfo userInfo)
        {
            return new HttpRequests().ThePost<UserInfo, CheckoutModel>(userInfo, "/api/checkout", BaseUrl);
        }
    }
}
