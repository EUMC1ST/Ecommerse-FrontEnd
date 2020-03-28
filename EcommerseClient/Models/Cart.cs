using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class Cart
    {
        public string idClient { get; set; }
        public List<CartItem> items { get; set; }
    }
}
