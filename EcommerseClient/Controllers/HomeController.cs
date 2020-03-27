using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerseClient.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace EcommerseClient.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client;
        public HomeController()
        {
            client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Inde(string currency = "USD")
        {
            string pathController = "api/ProductCatalogService?pageNumber=1";
            client.BaseAddress = new Uri("http://localhost:5001/");
            var response = client.GetAsync(pathController);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<ProductCatalog>(readresult);

            if (currency != null && currency != "USD")
            {
                for (int i = 0; i < resultadoFinal.products.Count; i++)
                {
                    resultadoFinal.products[i] = CurrencyConverter(resultadoFinal.products[i], currency, new HttpClient());
                }
            }
            return PartialView("_partIndex", resultadoFinal);
        }

        [HttpGet]
        public IActionResult BuyProducts(string id, string currency)
        {
            HttpClient clienttt = new HttpClient();
            string pathController = "api/ProductCatalogService/" + id;
            clienttt.BaseAddress = new Uri("http://localhost:5001/");
            var response = clienttt.GetAsync(pathController);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<Producto>(readresult);

            //-- convierte currency
            if (currency != null && currency != "USD")
            {
                resultadoFinal = CurrencyConverter(resultadoFinal, currency, new HttpClient());
            }
            return PartialView(resultadoFinal);
        }

        [HttpGet]
        public IActionResult Recomendation(string id)
        {
            HttpClient clienttt = new HttpClient();
            string pathController = "api/RecomendationService/" + id;
            clienttt.BaseAddress = new Uri("http://localhost:5007/");
            var response = clienttt.GetAsync(pathController);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<List<Producto>>(readresult);
            return PartialView(resultadoFinal);
        }

        public static Producto CurrencyConverter(Producto obj, string currency, HttpClient CurrencyClient)
        {
            string path = "api/currency/conversion";
            CurrencyClient.BaseAddress = new Uri("http://localhost:5004/");
            string json = JsonConvert.SerializeObject(
                new CurrencyChange()
                {
                    CurrencyCode = "USD",
                    CurrencyType = currency,
                    Nano = obj.priceUsd.nanos,
                    Units = obj.priceUsd.units
                });
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = CurrencyClient.PostAsync(path, httpcontent);
            resp.Wait();
            var resul = resp.Result;
            var readresul = resul.Content.ReadAsStringAsync().Result;
            string resulFin = JsonConvert.DeserializeObject<double>(readresul).ToString();
            string[] separators = { "." };
            string[] words = resulFin.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                obj.priceUsd.currencyCode = currency;
                obj.priceUsd.units = int.Parse(words[0]);
                obj.priceUsd.nanos = Int32.Parse(words[1].Remove(3));
            }
            catch (Exception)
            {
                obj.priceUsd.units = 69;
                obj.priceUsd.nanos = 69;
            }
            return obj;
        }


        [HttpPost]
        [Route("api/cart")]
        public IActionResult AddProductToCart(itemToCart info)
        {
            new CartService().Cart(new AddProductToCart()
            {
                idClient = info.idClient,
                idProduct = info.idProduct,
                quantity = info.quantity
            });

            //string r = HttpContext.Request.Cookies["user_id"];
            //List<itemToCart> list = new List<itemToCart>();
            //if (r == null)
            //{
            //    list.Add(info);
            //    HttpContext.Response.Cookies.Append("user_id", JsonConvert.SerializeObject(list));
            //}
            //else
            //{
            //    list = JsonConvert.DeserializeObject<List<itemToCart>>(r);
            //    list.Add(info);
            //    HttpContext.Response.Cookies.Append("user_id", JsonConvert.SerializeObject(list));
            //}
            return Ok();
        }

        public IActionResult TheCart()
        {
            return View();
        }

        [HttpPost]
        public double Shipping(Total total)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5003/");
            string json = JsonConvert.SerializeObject(total.eltotal); //----
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/shipping/estimate/" + total.eltotal , httpcontent);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<ShippingCost>(readresult);
            return resultadoFinal.calculatedShippingCost;
        }

        [HttpPost]
        public IActionResult CheckOut(UserInfo userinfo)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5008/");
            string json = JsonConvert.SerializeObject(userinfo); //----
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("/api/checkout", httpcontent);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<CheckoutModel>(readresult);
            return PartialView(resultadoFinal);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
