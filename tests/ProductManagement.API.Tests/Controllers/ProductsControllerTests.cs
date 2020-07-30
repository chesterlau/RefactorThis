using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using ProductManagement.API.Controllers;
using ProductManagement.Core.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Models;

namespace ProductManagement.API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProducts_Returns_OkResponse_Successfully()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Name = "Test Product 1",
                    Description = "Description 1",
                    Price = 2000M,
                    DeliveryPrice = 10M
                },
                new Product
                {
                    Id = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                    Name = "Test Product 2",
                    Description = "Description 2",
                    Price = 3000M,
                    DeliveryPrice = 30M
                },
            };

            mockProductService.Setup(m => m.GetAllProductsWithOptionalNameFilter(null))
                .ReturnsAsync(new GetAllProductsResponse
                {
                    Items = products
                })
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.GetProducts(null) as OkObjectResult;
            var responseObject = response.Value as GetAllProductsResponse;

            //Assert
            Assert.Equal(products[0].Id, responseObject.Items[0].Id);
            Assert.Equal(products[0].Name, responseObject.Items[0].Name);
            Assert.Equal(products[0].Description, responseObject.Items[0].Description);
            Assert.Equal(products[0].Price, responseObject.Items[0].Price);
            Assert.Equal(products[0].DeliveryPrice, responseObject.Items[0].DeliveryPrice);

            Assert.Equal(products[1].Id, responseObject.Items[1].Id);
            Assert.Equal(products[1].Name, responseObject.Items[1].Name);
            Assert.Equal(products[1].Description, responseObject.Items[1].Description);
            Assert.Equal(products[1].Price, responseObject.Items[1].Price);
            Assert.Equal(products[1].DeliveryPrice, responseObject.Items[1].DeliveryPrice);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task GetProducts_Returns_BadResponse_If_Exception_Is_Thrown()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Name = "Test Product 1",
                    Description = "Description 1",
                    Price = 2000M,
                    DeliveryPrice = 10M
                },
                new Product
                {
                    Id = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                    Name = "Test Product 2",
                    Description = "Description 2",
                    Price = 3000M,
                    DeliveryPrice = 30M
                },
            };

            mockProductService.Setup(m => m.GetAllProductsWithOptionalNameFilter(null))
                .ThrowsAsync(new Exception("Error occured!"))
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.GetProducts(null) as BadRequestObjectResult;
            var responseObject = response.Value as ApiResult;

            //Assert
            Assert.Equal("An error has occured", responseObject.Error);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task GetProductById_Returns_OkResponse_Successfully()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            Product product = new Product
            {
                Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                Name = "Test Product 1",
                Description = "Description 1",
                Price = 2000M,
                DeliveryPrice = 10M
            };

            mockProductService.Setup(m => m.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(new GetProductByIdResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    DeliveryPrice = product.DeliveryPrice
                })
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.GetProductById(Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3")) as OkObjectResult;
            var responseObject = response.Value as GetProductByIdResponse;

            //Assert
            Assert.Equal(product.Id, responseObject.Id);
            Assert.Equal(product.Name, responseObject.Name);
            Assert.Equal(product.Description, responseObject.Description);
            Assert.Equal(product.Price, responseObject.Price);
            Assert.Equal(product.DeliveryPrice, responseObject.DeliveryPrice);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task GetProductById_Returns_BadResponse_If_Exception_Is_Thrown()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            Product product = new Product
            {
                Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                Name = "Test Product 1",
                Description = "Description 1",
                Price = 2000M,
                DeliveryPrice = 10M
            };

            mockProductService.Setup(m => m.GetProductById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Error occured!"))
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.GetProductById(Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3")) as BadRequestObjectResult;
            var responseObject = response.Value as ApiResult;

            //Assert
            Assert.Equal("An error has occured", responseObject.Error);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task CreateProduct_Returns_OkResponse_Successfully()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            CreateProductRequest createProductRequest = new CreateProductRequest
            {
                Name = "Test Product 1",
                Description = "Description 1",
                Price = 2000M,
                DeliveryPrice = 10M
            };

            mockProductService.Setup(m => m.CreateProduct(It.IsAny<CreateProductRequest>()))
                .ReturnsAsync(new CreateProductResponse
                {
                    IsSuccessful = true
                })
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.CreateProduct(createProductRequest) as OkObjectResult;
            var responseObject = response.Value as CreateProductResponse;

            //Assert
            Assert.True(responseObject.IsSuccessful);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task CreateProduct_Returns_BadResponse_If_Exception_Is_Thrown()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            CreateProductRequest createProductRequest = new CreateProductRequest
            {
                Name = "Test Product 1",
                Description = "Description 1",
                Price = 2000M,
                DeliveryPrice = 10M
            };

            mockProductService.Setup(m => m.CreateProduct(It.IsAny<CreateProductRequest>()))
                .ThrowsAsync(new Exception("Error occured!"))
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.CreateProduct(createProductRequest) as BadRequestObjectResult;
            var responseObject = response.Value as ApiResult;

            //Assert
            Assert.Equal("An error has occured", responseObject.Error);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task UpdateProduct_Returns_OkResponse_Successfully()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            Guid productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");

            UpdateProductRequest updateProductRequest = new UpdateProductRequest
            {
                Name = "Test Product 1",
                Description = "Description 1",
                Price = 2000M,
                DeliveryPrice = 10M
            };

            mockProductService.Setup(m => m.UpdateProduct(productId, It.IsAny<UpdateProductRequest>()))
                .ReturnsAsync(new UpdateProductResponse
                {
                    IsSuccessful = true
                })
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.UpdateProduct(productId, updateProductRequest) as OkObjectResult;
            var responseObject = response.Value as UpdateProductResponse;

            //Assert
            Assert.True(responseObject.IsSuccessful);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task UpdateProduct_Returns_BadResponse_If_Exception_Is_Thrown()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            Guid productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");

            UpdateProductRequest updateProductRequest = new UpdateProductRequest
            {
                Name = "Test Product 1",
                Description = "Description 1",
                Price = 2000M,
                DeliveryPrice = 10M
            };

            mockProductService.Setup(m => m.UpdateProduct(productId, It.IsAny<UpdateProductRequest>()))
                .ThrowsAsync(new Exception("Error occured!"))
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.UpdateProduct(productId, updateProductRequest) as BadRequestObjectResult;
            var responseObject = response.Value as ApiResult;

            //Assert
            Assert.Equal("An error has occured", responseObject.Error);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task GetProductOptionsByProductId_Returns_OkResponse_Successfully()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            Guid productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");

            List<ProductOption> productOptions = new List<ProductOption>
            {
                new ProductOption
                {
                    Id = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                    ProductId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Description = "This is a product option 1",
                    Name = "Product Option 1"
                },
                new ProductOption
                {
                    Id = Guid.Parse("845403f2-5179-4efb-820e-c1c120ed5930"),
                    ProductId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Description = "This is a product option 1",
                    Name = "Product Option 1"
                }
            };

            mockProductService.Setup(m => m.GetProductOptionsByProductId(It.IsAny<Guid>()))
                .ReturnsAsync(new GetProductOptionsByProductIdResponse
                {
                    Items = productOptions
                })
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.GetProductOptionsByProductId(productId) as OkObjectResult;
            var responseObject = response.Value as GetProductOptionsByProductIdResponse;

            //Assert
            Assert.Equal(productOptions[0].Id, responseObject.Items[0].Id);
            Assert.Equal(productOptions[0].ProductId, responseObject.Items[0].ProductId);
            Assert.Equal(productOptions[0].Name, responseObject.Items[0].Name);
            Assert.Equal(productOptions[0].Description, responseObject.Items[0].Description);

            Assert.Equal(productOptions[1].Id, responseObject.Items[1].Id);
            Assert.Equal(productOptions[1].ProductId, responseObject.Items[1].ProductId);
            Assert.Equal(productOptions[1].Name, responseObject.Items[1].Name);
            Assert.Equal(productOptions[1].Description, responseObject.Items[1].Description);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

        [Fact]
        public async Task GetProductOptionsByProductId_Returns_BadResponse_If_Exception_Is_Thrown()
        {
            //Arrange
            Mock<ILogger<ProductsController>> mockLogger = new Mock<ILogger<ProductsController>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();

            Guid productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");

            mockProductService.Setup(m => m.GetProductOptionsByProductId(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Error occured!"))
                .Verifiable();

            ProductsController productsController = new ProductsController(mockLogger.Object, mockProductService.Object);

            //Act
            var response = await productsController.GetProductOptionsByProductId(productId) as BadRequestObjectResult;
            var responseObject = response.Value as ApiResult;

            //Assert
            Assert.Equal("An error has occured", responseObject.Error);

            mockLogger.VerifyAll();
            mockProductService.VerifyAll();
        }

    }
}
