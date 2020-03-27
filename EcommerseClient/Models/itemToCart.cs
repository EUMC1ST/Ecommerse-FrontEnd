using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class itemToCart
    {
        public string idClient { get; set; }
        public string idProduct { get; set; }
        public int quantity { get; set; }
        public string currency { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
        public double total { get; set; }
    }
}
