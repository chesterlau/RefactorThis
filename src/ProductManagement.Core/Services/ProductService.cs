using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductManagement.Contracts.Dto;
using ProductManagement.Contracts.Models;
using ProductManagement.Data.Repositories;
using System;
using System.Collections.Generic;
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

        public async Task<GetAllProductsResponse> GetAllProductsWithOptionalNameFilter(string name)
        {
            _logger.LogInformation($"Getting all products with name filter: {name}");

            List<Product> products;

            if(string.IsNullOrEmpty(name))
            {
                products = await _productRepository.GetAllProducts();
            }
            else
            {
                products = await _productRepository.GetProductsByLikeFilter("Name", name);
            }

            GetAllProductsResponse getAllProductsResponse = new GetAllProductsResponse
            {
                Items = products
            };

            _logger.LogInformation($"Retrieved {products.Count} products");

            return getAllProductsResponse;
        }

        public async Task<GetProductByIdResponse> GetProductById(Guid id)
        {
            _logger.LogInformation($"Getting product by id: {id}");

            var product = await _productRepository.GetProductByWhereFilter("Id", id);

            if(product == null)
            {
                _logger.LogInformation($"No product with id: {id} found");
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

        public async Task<CreateProductResponse> CreateProduct(CreateProductRequest createProductRequest)
        {
            _logger.LogInformation($"Creating a product: {JsonConvert.SerializeObject(createProductRequest)}");

            Product product = new Product
            {
                Id = Guid.NewGuid(),
                DeliveryPrice = createProductRequest.DeliveryPrice,
                Description = createProductRequest.Description,
                Price = createProductRequest.Price,
                Name = createProductRequest.Name
            };

            var result = await _productRepository.CreateProduct(product);

            var createProductResponse = new CreateProductResponse
            { 
                IsSuccessful = result
            };
            
            _logger.LogInformation($"Creating a product with IsSuccessful: {createProductResponse.IsSuccessful}");

            return createProductResponse;
        }
    }
}
