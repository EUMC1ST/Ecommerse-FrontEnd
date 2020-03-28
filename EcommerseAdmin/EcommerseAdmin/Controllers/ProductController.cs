using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerseAdmin.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace EcommerseAdmin.Controllers
{
    public class ProductController : Controller
    {
        string uri = Environment.GetEnvironmentVariable("ProductCatalogUrl");

        public IActionResult Index()
        {
            return View("Index",SearchProducts());
        }

        public ActionResult Delete(string id)
        {
            var pathController = "api/ProductCatalogService/" + id;
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:5001/");
            client.BaseAddress = new Uri(uri);
            var response = client.DeleteAsync(pathController);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            //var resultadoFinal = JsonConvert.DeserializeObject<Producto>(readresult);
            return RedirectToAction("Index");

        }

        public ActionResult Details(string id)
        {
            var pathController = "api/ProductCatalogService/" + id;
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:5001/");
            client.BaseAddress = new Uri(uri);
            var response = client.GetAsync(pathController);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<Producto>(readresult);
            return View("Details", resultadoFinal);
        }


        public ActionResult Edit(string id)
        {
            var pathController = "api/ProductCatalogService/"+id;
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:5001/");
            client.BaseAddress = new Uri(uri);
            var response = client.GetAsync(pathController);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<Producto>(readresult);
            return View("Edit",resultadoFinal);
        }

        public ActionResult Save(Producto producto)
        {
            var client = new HttpClient();
            var pathController = "api/ProductCatalogService/";
            //client.BaseAddress = new Uri("http://localhost:5001/");
            client.BaseAddress = new Uri(uri);
            string json = JsonConvert.SerializeObject(producto); //----
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(pathController, httpcontent);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            //var resultadoFinal = JsonConvert.DeserializeObject<output>(readresult);
            return RedirectToAction("Index");
        }

        public IActionResult SearchProductByName(string name)
        {
            return View("Index",SearchProducts(name));
        }

        private ProductCatalog SearchProducts( string name =""){    
            string pathController;
            if (name != String.Empty)
            {
                pathController = "api/ProductCatalogService?name=" + name;

            }
            else
            {
                pathController = "api/ProductCatalogService?pageNumber=1";
            }

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:5001/");
            client.BaseAddress = new Uri(uri);
            var response = client.GetAsync(pathController);                                                                                                      
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<ProductCatalog>(readresult);
            return resultadoFinal;
        }

        public ActionResult Contact()
        {
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
