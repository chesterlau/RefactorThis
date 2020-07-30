using ProductManagement.Contracts.Models;
using System.Collections.Generic;

namespace ProductManagement.Contracts.Dtos
{
    public class GetProductsByNameResponse
    {
        public List<Product> Items { get; set; }
    }
}
