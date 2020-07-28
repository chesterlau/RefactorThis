using ProductManagement.Contracts.Models;
using System.Collections.Generic;

namespace ProductManagement.Contracts.Dto
{
    public class GetProductsByNameResponse
    {
        public List<Product> Items { get; set; }
    }
}
