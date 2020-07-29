using System;
using Xunit;
using Moq;
using ProductManagement.Data.Repositories;
using System.Collections.Generic;
using ProductManagement.Contracts.Models;
using Microsoft.Extensions.Logging;
using ProductManagement.Core.Services;
using System.Threading.Tasks;
using ProductManagement.Contracts.Dto;

namespace ProductManagement.Core.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetAllProductsWithOptionalNameFilter_When_No_Name_PassedIn_Returns_Products_Correctly()
        {
            //Assert
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                    Name = "Test product",
                    Description = "This is a desc",
                    Price = 100,
                    DeliveryPrice = 10
                }
            };

            mockProductRepository.Setup(m => m.GetAllProducts())
                .ReturnsAsync(products)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.GetAllProductsWithOptionalNameFilter(null);

            //Assert
            Assert.Equal(products[0].Id, result.Items[0].Id);
            Assert.Equal(products[0].Name, result.Items[0].Name);
            Assert.Equal(products[0].Description, result.Items[0].Description);
            Assert.Equal(products[0].Price, result.Items[0].Price);
            Assert.Equal(products[0].DeliveryPrice, result.Items[0].DeliveryPrice);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task GetAllProductsWithOptionalNameFilter_When_Name_PassedIn_Returns_Products_Correctly()
        {
            //Assert
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                    Name = "Test product",
                    Description = "This is a desc",
                    Price = 100,
                    DeliveryPrice = 10
                }
            };

            mockProductRepository.Setup(m => m.GetProductsLikeName(It.IsAny<string>()))
                .ReturnsAsync(products)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.GetAllProductsWithOptionalNameFilter("Test");

            //Assert
            Assert.Equal(products[0].Id, result.Items[0].Id);
            Assert.Equal(products[0].Name, result.Items[0].Name);
            Assert.Equal(products[0].Description, result.Items[0].Description);
            Assert.Equal(products[0].Price, result.Items[0].Price);
            Assert.Equal(products[0].DeliveryPrice, result.Items[0].DeliveryPrice);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task GetProductById_Returns_Product_Correctly()
        {
            //Assert
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            Product product = new Product
            {
                Id = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                Name = "Test product",
                Description = "This is a desc",
                Price = 100,
                DeliveryPrice = 10
            };

            mockProductRepository.Setup(m => m.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(product)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.GetProductById(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"));

            //Assert
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.DeliveryPrice, result.DeliveryPrice);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task CreateProduct_Creates_Product_Successfully()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync(true)
                .Verifiable();

            CreateProductRequest createProductRequest = new CreateProductRequest
            {
                Name = "Test product",
                Description = "This is a desc",
                Price = 100,
                DeliveryPrice = 10
            };

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.CreateProduct(createProductRequest);

            //Assert
            Assert.True(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task UpdateProduct_Returns_Unsuccessful_When_Product_Not_Found()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Product)
                .Verifiable();

            UpdateProductRequest updateProductRequest = new UpdateProductRequest
            {
                Name = "Test product",
                Description = "This is a desc",
                Price = 100,
                DeliveryPrice = 10
            };

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.UpdateProduct(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), updateProductRequest);

            //Assert
            Assert.False(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task UpdateProduct_Updates_Product_Successfully()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(new Product())
                .Verifiable();

            mockProductRepository.Setup(m => m.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(true)
                .Verifiable();

            UpdateProductRequest updateProductRequest = new UpdateProductRequest
            {
                Name = "Test product",
                Description = "This is a desc",
                Price = 100,
                DeliveryPrice = 10
            };

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.UpdateProduct(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), updateProductRequest);

            //Assert
            Assert.True(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

    }
}
