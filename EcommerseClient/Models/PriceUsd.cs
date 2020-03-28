using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class PriceUsd
    {
        public string currencyCode { get; set; }
        public int units { get; set; }
        public int nanos { get; set; }
    }
}
