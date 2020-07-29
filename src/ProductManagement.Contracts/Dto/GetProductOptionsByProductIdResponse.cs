using ProductManagement.Contracts.Models;
using System.Collections.Generic;

namespace ProductManagement.Contracts.Dto
{
    public class GetProductOptionsByProductIdResponse
    {
        public List<ProductOption> Items { get; set; }
    }
}
