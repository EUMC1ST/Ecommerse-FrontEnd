using EcommerseClient.Models;
using System;
using System.Collections.Generic;

namespace EcommerseClient.Services
{
    public class RecomendationService
    {
        public static string BaseUrl = Environment.GetEnvironmentVariable("RecomendationService");

        public static List<Producto> Recomended(string id)
        {
            return new HttpRequests().TheGet<List<Producto>>("api/RecomendationService/" + id, BaseUrl);
        }
    }
}
