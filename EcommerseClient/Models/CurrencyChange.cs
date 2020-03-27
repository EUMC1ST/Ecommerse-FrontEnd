using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseClient.Models
{
    public class CurrencyChange
    {
        public string CurrencyCode { get; set; }
        public int Units { get; set; }
        public double Nano { get; set; }
        public string CurrencyType { get; set; }
    }
}
