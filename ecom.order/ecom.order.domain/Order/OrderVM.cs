﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecom.order.domain.Order
{
    public class OrderVM
    {        
        public string? ProductId { get; set; }
        public string? CustomerId { get; set; }
        public int ProductCount { get; set; }     
    }
}
