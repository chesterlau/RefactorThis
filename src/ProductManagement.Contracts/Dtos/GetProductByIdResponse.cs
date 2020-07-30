using System;

namespace ProductManagement.Contracts.Dtos
{
    public class GetProductByIdResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}
