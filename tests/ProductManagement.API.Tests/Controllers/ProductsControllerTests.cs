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
    }
}
