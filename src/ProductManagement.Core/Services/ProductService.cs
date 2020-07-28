using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Models;
using ProductManagement.Data.Repositories;

namespace ProductManagement.Core.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly ILogger<ProductService> _logger;

		public ProductService(
			IProductRepository productRepository,
			ILogger<ProductService> logger)
		{
			_productRepository = productRepository;
			_logger = logger;
		}

		public Products GetAllProducts()
		{
			_logger.LogInformation($"Getting all products");

			var products = _productRepository.GetAllProducts();
			return products;
		}
	}
}
