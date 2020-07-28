namespace ProductManagement.Contracts.Dto
{
	public class CreateProductRequest
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public decimal DeliveryPrice { get; set; }
	}
}
