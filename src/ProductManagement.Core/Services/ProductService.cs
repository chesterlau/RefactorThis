using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductManagement.Contracts.Dto;
using ProductManagement.Contracts.Models;
using ProductManagement.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
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

            var product = await _productRepository.GetProductById(id);

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
            
            _logger.LogInformation($"Created a product with IsSuccessful: {createProductResponse.IsSuccessful}");

            return createProductResponse;
        }

        public async Task<UpdateProductResponse> UpdateProduct(Guid id, UpdateProductRequest updateProductRequest)
        {
            _logger.LogInformation($"Updating product with id: {id}");

            var product = await _productRepository.GetProductById(id);

            if(product == null)
            {
                _logger.LogInformation($"Could not find product with id: {id}");

                return new UpdateProductResponse 
                {
                    IsSuccessful = false
                };
            }

            product.Name = updateProductRequest.Name;
            product.Description = updateProductRequest.Description;
            product.Price = updateProductRequest.Price;
            product.DeliveryPrice = updateProductRequest.DeliveryPrice;

            var result = await _productRepository.UpdateProduct(product);

            var updateProductResponse = new UpdateProductResponse
            { 
                IsSuccessful = result
            };

            _logger.LogInformation($"Updated a product with IsSuccessful: {updateProductResponse.IsSuccessful}");

            return updateProductResponse;
        }

        public async Task<GetProductOptionsByProductIdResponse> GetProductOptionsByProductId(Guid id)
        {
            _logger.LogInformation($"Getting product options by product id: {id}");

            var productOptions = await _productRepository.GetProductOptionsByProductId(id);

            var getProductOptionsByProductIdResponse = new GetProductOptionsByProductIdResponse
            {
                Items = productOptions
            };

            _logger.LogInformation($"Retrieved {productOptions.Count} product options");

            return getProductOptionsByProductIdResponse;
        }

        public async Task<GetProductOptionsByProductIdAndOptionsIdResponse> GetProductOptionsByProductIdAndOptionsId(Guid productId, Guid optionId)
        {
            var productOption = await _productRepository.GetProductOptionsByProductIdAndOptionsId(productId, optionId);

            if (productOption == null)
            {
                _logger.LogInformation($"No product options found with optionsId: {optionId} and productId: {productId}");
                return null;
            }

            var getProductOptionsByProductIdAndOptionsIdResponse = new GetProductOptionsByProductIdAndOptionsIdResponse
            {
                Name = productOption.Name,
                Description = productOption.Description,
                Id = productOption.Id,
                ProductId = productOption.ProductId
            };

            return getProductOptionsByProductIdAndOptionsIdResponse;
        }

        public async Task<CreateProductOptionResponse> CreateProductOption(Guid productId, CreateProductOptionRequest createProductOptionRequest)
        {
            _logger.LogInformation($"Creating a product: {JsonConvert.SerializeObject(createProductOptionRequest)}");

            //Find the product first
            var product = await _productRepository.GetProductById(productId);

            if(product == null)
            {
                _logger.LogInformation($"No product with id: {productId} found. Can't add product to product option.");
                return new CreateProductOptionResponse { IsSuccessful = false };
            }

            var productOption = new ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Description = createProductOptionRequest.Description,
                Name = createProductOptionRequest.Name
            };

            var result = await _productRepository.CreateProductOption(productOption);

            var createProductOptionResponse = new CreateProductOptionResponse
            {
                IsSuccessful = result
            };

            _logger.LogInformation($"Created a product option with IsSuccessful: {createProductOptionResponse.IsSuccessful}");

            return createProductOptionResponse;
        }

        public async Task<UpdateProductOptionResponse> UpdateProductOption(Guid productId, Guid optionId, UpdateProductOptionRequest updateProductOptionRequest)
        {
            _logger.LogInformation($"Updating product option with productId: {productId} and optionId: {optionId}");

            var productOption = await _productRepository.GetProductOptionsByProductIdAndOptionsId(productId, optionId);

            if (productOption == null)
            {
                _logger.LogInformation($"Could not find product option with productId: {productId} and optionId: {optionId}");

                return new UpdateProductOptionResponse
                {
                    IsSuccessful = false
                };
            }

            productOption.Name = updateProductOptionRequest.Name;
            productOption.Description = updateProductOptionRequest.Description;

            var result = await _productRepository.UpdateProductOption(productOption);

            var updateProductOptionResponse = new UpdateProductOptionResponse
            {
                IsSuccessful = result
            };

            _logger.LogInformation($"Updated a product with IsSuccessful: {updateProductOptionResponse.IsSuccessful}");

            return updateProductOptionResponse;
        }

        public async Task<DeleteProductOptionResponse> DeleteProductOption(Guid productId, Guid optionId)
        {
            var productOption = await _productRepository.GetProductOptionsByProductIdAndOptionsId(productId, optionId);

            if(productOption == null)
            {
                _logger.LogInformation($"Could not find product option with productId: {productId} and optionId: {optionId}");

                return new DeleteProductOptionResponse
                {
                    IsSuccessful = false
                };
            }

            var result = await _productRepository.DeleteProductOption(optionId);

            var deleteProductOptionResponse = new DeleteProductOptionResponse
            {
                IsSuccessful = result
            };

            return deleteProductOptionResponse;
        }

        public async Task<DeleteProductResponse> DeleteProduct(Guid productId)
        {
            _logger.LogInformation($"Deleting a product with productId: {productId}");

            var productOptions = await _productRepository.GetProductOptionsByProductId(productId);

            foreach(var productOption in productOptions)
            {
                var deleteProductOptionResult = await _productRepository.DeleteProductOption(productOption.Id);

                if(!deleteProductOptionResult)
                {
                    _logger.LogWarning($"Fail to delete product option with optionId: {productOption.Id} and productId: {productOption.ProductId}");
                }
                else
                {
                    _logger.LogInformation($"Delete product option with optionId: {productOption.Id} and productId: {productOption.ProductId}");
                }
            }

            var deletionProductResult = await _productRepository.DeleteProduct(productId);

            var deleteProductResponse = new DeleteProductResponse
            {
                IsSuccessful = deletionProductResult
            };

            return deleteProductResponse;
        }
    }
}
