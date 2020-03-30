using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class itemToCart
    {
        public string idProduct { get; set; }
        public int quantity { get; set; }
        public string currency { get; set; }
    }
}
