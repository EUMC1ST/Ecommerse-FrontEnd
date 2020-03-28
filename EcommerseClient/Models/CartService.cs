using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class CartService
    {
        public void Cart(AddProductToCart input)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            string json = JsonConvert.SerializeObject(input); //----
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/CartService", httpcontent);
            response.Wait();
            var result = response.Result;
            Console.WriteLine(result);
        }
    }
}
