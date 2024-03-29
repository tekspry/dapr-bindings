﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecom.product.domain.Product
{
    public class ProductForDescriptionGeneration
    {
        public string Name { get; set; } = String.Empty;
        public int Price { get; set; }
        public string Seller { get; set; } = String.Empty;
        public string? AvailableSince { get; set; }
        public string ShortDescription { get; set; } = String.Empty;
        public int Quantity { get; set; }
        public string ProductDescription { get; set; } = String.Empty;
    }
}
