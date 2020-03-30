using EcommerseClient.Models;
using System;
using System.Collections.Generic;

namespace EcommerseClient.Services
{
    public class CartService
    {
        public static string BaseUrl = Environment.GetEnvironmentVariable("CartService");
        public static void Cart(AddProductToCart input)
        {
            string output = new HttpRequests().ThePost<AddProductToCart>(input, "api/CartService", BaseUrl);
        }

        public static Cart GetCart(string userId)
        {
            try
            {
                return new HttpRequests().TheGet<Cart>("api/CartService/" + userId, BaseUrl);
            }
            catch (Exception e)
            {
                return new Cart() { items = new List<CartItem>() };
            }
            
        }

        public static string DeleteCart(string userId)
        {
            return new HttpRequests().TheDelete("api/CartService/" + userId, BaseUrl);
        }
    }
}
