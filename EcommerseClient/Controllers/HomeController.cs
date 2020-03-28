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
using EcommerseClient.Services;

namespace EcommerseClient.Controllers
{
    public class HomeController : Controller
    {
        //HttpClient client;
        //public HomeController()
        //{
        //    client = new HttpClient();
        //}

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies["UserId"] == null)
            {
                string info = Guid.NewGuid().ToString();
                HttpContext.Response.Cookies.Append("UserId", info);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Products(string currency = "USD")
        {
            var resultadoFinal = ProductCatalogService.Catalog();
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
            Producto Theproduct = ProductCatalogService.Info(id);
            //-- convierte currency
            if (currency != null && currency != "USD")
            {
                Theproduct = CurrencyConverter(Theproduct, currency, new HttpClient());
            }
            return PartialView(Theproduct);
        }

        [HttpGet]
        public IActionResult Recomendation(string id)
        {
            List<Producto> recomended = RecomendationService.Recomended(id);
            return PartialView(recomended);
        }

        public static Producto CurrencyConverter(Producto obj, string currency, HttpClient CurrencyClient)
        {
            CurrencyChange currencyChange = new CurrencyChange()
            {
                CurrencyCode = "USD",
                CurrencyType = currency,
                Nano = obj.priceUsd.nanos,
                Units = obj.priceUsd.units
            };

            double newCurrency = CurrencyService.Conversion(currencyChange);
            string resulFin = newCurrency + "";
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
            CartService.Cart(new AddProductToCart()
            {
                idClient = info.idClient,
                idProduct = info.idProduct,
                quantity = info.quantity
            });
            return Ok();
        }

        public IActionResult TheCart()
        {
            return View();
        }

        [HttpPost]
        public double Shipping(Total total)
        {
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:5003/");
            //string json = JsonConvert.SerializeObject(total.eltotal); //----
            //var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = client.PostAsync("api/shipping/estimate/" + total.eltotal, httpcontent);
            //response.Wait();
            //var result = response.Result;
            //var readresult = result.Content.ReadAsStringAsync().Result;
            //var resultadoFinal = JsonConvert.DeserializeObject<ShippingCost>(readresult);
            ShippingCost resultadoFinal = ShippingService.Estimate(total.eltotal);
            return resultadoFinal.calculatedShippingCost;
        }

        [HttpPost]
        public IActionResult CheckOut(UserInfo userinfo)
        {
            CheckoutModel checkoutModel = CheckoutService.Checkout(userinfo);
            return PartialView(checkoutModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
