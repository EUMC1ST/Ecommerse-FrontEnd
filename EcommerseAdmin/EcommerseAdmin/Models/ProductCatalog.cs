﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerseAdmin.Models
{
    public class ProductCatalog
    {
        public List<Producto> products { get; set; }
        public int totalItems { get; set; }
    }
}
