﻿using ProductManagement.Contracts.Models;
using System.Collections.Generic;

namespace ProductManagement.Contracts.Dto
{
    public class GetAllProductsResponse
    {
        public List<Product> Items { get; set; }
    }
}
