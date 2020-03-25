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

namespace EcommerseClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string currency)
        {
            string pathController = "api/ProductCatalogService?pageNumber=1";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5001/");
            var response = client.GetAsync(pathController);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<ProductCatalog>(readresult);

            if (currency != null && currency != "USD")
            {
                foreach (var item in resultadoFinal.products)
                {
                    HttpClient clientt = new HttpClient();
                    string path = "api/currency/conversion";
                    clientt.BaseAddress = new Uri("http://localhost:5004/");
                    string json = JsonConvert.SerializeObject(
                        new CurrencyChange() {CurrencyCode = "USD", CurrencyType = currency, Nano = item.priceUsd.nanos,
                        Units = item.priceUsd.units});
                    var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
                    var resp = clientt.PostAsync(path, httpcontent);
                    resp.Wait();
                    var resul = resp.Result;
                    var readresul = resul.Content.ReadAsStringAsync().Result;
                    string resulFin = JsonConvert.DeserializeObject<double>(readresul).ToString();
                    string[] separators = { "." };
                    string[] words = resulFin.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        item.priceUsd.currencyCode = currency;
                        item.priceUsd.units = int.Parse(words[0]);
                        item.priceUsd.nanos = Int32.Parse(words[1]);
                    }
                    catch (Exception)
                    {
                        item.priceUsd.units = 69;
                        item.priceUsd.nanos = 69;
                    }
                }
            }

            return View(resultadoFinal);
        }

        public IActionResult BuyProduct()
        {
            return View();
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
