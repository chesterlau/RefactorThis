using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Models;
using ProductManagement.Data.Repositories;
using System.Threading.Tasks;

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

		public async Task<Products> GetAllProducts()
		{
			_logger.LogInformation($"Getting all products");

			var products = await _productRepository.GetAllProducts();
			return products;
		}
	}
}
