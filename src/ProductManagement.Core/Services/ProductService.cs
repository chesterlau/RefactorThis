using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Dto;
using ProductManagement.Contracts.Models;
using ProductManagement.Data.Repositories;
using System;
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

        public async Task<GetAllProductsResponse> GetAllProducts()
        {
            _logger.LogInformation($"Getting all products");

            var products = await _productRepository.GetAllProducts();

            GetAllProductsResponse getAllProductsResponse = new GetAllProductsResponse
            {
                Items = products
            };

            return getAllProductsResponse;
        }

        public async Task<GetProductByIdResponse> GetProductById(Guid id)
        {
            _logger.LogInformation($"Getting all products by id: {id}");

            var product = await _productRepository.GetProductByWhereFilter("Id", id);

            if(product == null)
            {
                return null;
            }

            var getProductByIdResponse = new GetProductByIdResponse
            {
                DeliveryPrice = product.DeliveryPrice,
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
            
            return getProductByIdResponse;
        }
    }
}
