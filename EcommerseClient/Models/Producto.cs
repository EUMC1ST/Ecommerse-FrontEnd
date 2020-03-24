using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class Producto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string picture { get; set; }
        public PriceUsd priceUsd { get; set; }
        public List<string> categories { get; set; }
    }
}
