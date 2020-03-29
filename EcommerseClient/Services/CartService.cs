﻿using EcommerseClient.Models;

namespace EcommerseClient.Services
{
    public class CartService
    {
        public static string BaseUrl = "http://localhost:5000/";
        public static void Cart(AddProductToCart input)
        {
            string output = new HttpRequests().ThePost<AddProductToCart>(input, "api/CartService", BaseUrl);
        }

        public static Cart GetCart(string userId)
        {
            return new HttpRequests().TheGet<Cart>("api/CartService/" + userId, BaseUrl);
        }
    }
}