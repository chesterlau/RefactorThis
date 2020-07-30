using System;

namespace ProductManagement.Contracts.Dtos
{
    public class GetProductOptionsByProductIdAndOptionsIdResponse
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
