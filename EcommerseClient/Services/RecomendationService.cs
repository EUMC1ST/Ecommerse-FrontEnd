using EcommerseClient.Models;
using System.Collections.Generic;

namespace EcommerseClient.Services
{
    public class RecomendationService
    {
        public static string BaseUrl = "http://localhost:5007/";

        public static List<Producto> Recomended(string id)
        {
            return new HttpRequests().TheGet<List<Producto>>("api/RecomendationService/" + id, BaseUrl);
        }
    }
}
