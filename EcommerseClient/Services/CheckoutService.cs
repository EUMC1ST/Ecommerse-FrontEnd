using EcommerseClient.Models;
using System;

namespace EcommerseClient.Services
{
    public class CheckoutService 
    {
        public static string BaseUrl = Environment.GetEnvironmentVariable("CheckoutService");

        public static CheckoutModel Checkout(UserInfo userInfo)
        {
            return new HttpRequests().ThePost<UserInfo, CheckoutModel>(userInfo, "/api/checkout", BaseUrl);
        }
    }
}
