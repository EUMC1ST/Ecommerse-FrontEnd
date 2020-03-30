using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EcommerseClient.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using EcommerseClient.Services;

namespace EcommerseClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string currency = "")
        {
            GenerateCookie();
            var resultadoFinal = ProductCatalogService.Catalog();
            if (currency == "" && CurrencyService.cur == "")
            {
                CurrencyService.cur = "USD";
            }
            if(currency != "")
            {
                CurrencyService.cur = currency;
            }
            return View(resultadoFinal);
        }

        [HttpGet]
        public IActionResult SearchByName(string name)
        {
            var Found = ProductCatalogService.CatalogByName(name);
            return PartialView("Index", Found);
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
        public IActionResult BuyProducts(string id, string currency)
        {
            Producto Theproduct = ProductCatalogService.Info(id);
            if (currency == "")
            {
                currency = "USD";
            }
            CurrencyService.cur = currency;
            return PartialView(Theproduct);
        }

        [HttpPost]
        public IActionResult CheckOut(UserInfo userinfo)
        {
            userinfo.UserId = HttpContext.Request.Cookies["UserId"];
            userinfo.CurrencyExchange = CurrencyService.cur;
            CheckoutModel checkoutModel = CheckoutService.Checkout(userinfo);
            return View(checkoutModel);
        }


    }
}
