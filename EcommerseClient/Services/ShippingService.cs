using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerseClient.Models;
namespace EcommerseClient.Services
{
    public class ShippingService
    {
        public static string BaseUrl = Environment.GetEnvironmentVariable("ShippingService");

        public static ShippingCost Estimate(double total)
        {
            return new HttpRequests().ThePost<double, ShippingCost>(total, "api/shipping/estimate/" + total, BaseUrl);
        }
    }
}
