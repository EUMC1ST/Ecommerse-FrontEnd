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
        public IActionResult Index()
        {
            GenerateCookie();
            return View();
        }
        public void GenerateCookie()
        {
            if (HttpContext.Request.Cookies["UserId"] == null)
            {
                string info = Guid.NewGuid().ToString();
                HttpContext.Response.Cookies.Append("UserId", info);
            }
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
                idClient = HttpContext.Request.Cookies["UserId"],
                idProduct = info.idProduct,
                quantity = info.quantity
            });
            return Ok();
        }

        public IActionResult TheCart()
        {
            Cart cart = CartService.GetCart(HttpContext.Request.Cookies["UserId"]);
            return View(cart);
        }

        [HttpPost]
        public double Shipping(Total total)
        {
            ShippingCost resultadoFinal = ShippingService.Estimate(total.eltotal);
            return resultadoFinal.calculatedShippingCost;
        }

        [HttpPost]
        public IActionResult CheckOut(UserInfo userinfo)
        {
            userinfo.UserId = HttpContext.Request.Cookies["UserId"];
            userinfo.CurrencyExchange = "USD";
            CheckoutModel checkoutModel = CheckoutService.Checkout(userinfo);
            return View(checkoutModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
