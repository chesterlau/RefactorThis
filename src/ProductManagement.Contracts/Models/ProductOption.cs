using System;
using System.Text.Json.Serialization;

namespace ProductManagement.Contracts.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
