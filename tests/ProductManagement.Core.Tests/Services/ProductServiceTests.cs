using System;
using Xunit;
using Moq;
using ProductManagement.Data.Repositories;
using System.Collections.Generic;
using ProductManagement.Contracts.Models;
using Microsoft.Extensions.Logging;
using ProductManagement.Core.Services;
using System.Threading.Tasks;
using ProductManagement.Contracts.Dtos;

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

        [Fact]
        public async Task GetProductOptionsByProductId_Returns_ProductOptions_Correctly()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            List<ProductOption> productOptions = new List<ProductOption>
            {
                new ProductOption 
                {
                    Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    ProductId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                    Name = "Mac Red",
                    Description = "This is mac red"
                }
            };

            mockProductRepository.Setup(m => m.GetProductOptionsByProductId(It.IsAny<Guid>()))  
                .ReturnsAsync(productOptions)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.GetProductOptionsByProductId(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"));

            //Assert
            Assert.Equal(productOptions[0].Id, result.Items[0].Id);
            Assert.Equal(productOptions[0].ProductId, result.Items[0].ProductId);
            Assert.Equal(productOptions[0].Name, result.Items[0].Name);
            Assert.Equal(productOptions[0].Description, result.Items[0].Description);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task GetProductOptionsByProductIdAndOptionsId_Returns_ProductOption_Correctly()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            ProductOption productOption = new ProductOption
            {
                Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                ProductId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
                Name = "Mac Red",
                Description = "This is mac red"
            };

            mockProductRepository.Setup(m => m.GetProductOptionsByProductIdAndOptionsId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(productOption)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.GetProductOptionsByProductIdAndOptionsId(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"));

            //Assert
            Assert.Equal(productOption.Id, result.Id);
            Assert.Equal(productOption.Name, result.Name);
            Assert.Equal(productOption.Description, result.Description);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task CreateProductOption_Return_Unsuccessful_When_Product_Not_Found()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Product)
                .Verifiable();

            CreateProductOptionRequest createProductOptionRequest = new CreateProductOptionRequest
            {
                Name = "Test Product Option",
                Description = "This is a desc"
            };

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.CreateProductOption(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), createProductOptionRequest);

            //Assert
            Assert.False(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task CreateProductOption_Creates_ProductOption_Successfully()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(new Product())
                .Verifiable();

            mockProductRepository.Setup(m => m.CreateProductOption(It.IsAny<ProductOption>()))  
                .ReturnsAsync(true)
                .Verifiable();

            CreateProductOptionRequest createProductOptionRequest = new CreateProductOptionRequest
            {
                Name = "Test Product Option",
                Description = "This is a desc"
            };

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.CreateProductOption(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), createProductOptionRequest);

            //Assert
            Assert.True(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task UpdateProductOption_Returns_Unsuccessful_When_ProductOption_Not_Found()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductOptionsByProductIdAndOptionsId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(null as ProductOption)
                .Verifiable();

            UpdateProductOptionRequest createProductOptionRequest = new UpdateProductOptionRequest
            {
                Name = "Test Product Option",
                Description = "This is a desc"
            };

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.UpdateProductOption(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"), createProductOptionRequest);

            //Assert
            Assert.False(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task UpdateProductOption_Updates_ProductOption_Successfully()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductOptionsByProductIdAndOptionsId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new ProductOption())
                .Verifiable();

            mockProductRepository.Setup(m => m.UpdateProductOption(It.IsAny<ProductOption>()))
                .ReturnsAsync(true)
                .Verifiable();

            UpdateProductOptionRequest createProductOptionRequest = new UpdateProductOptionRequest
            {
                Name = "Test Product Option",
                Description = "This is a desc"
            };

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.UpdateProductOption(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"), createProductOptionRequest);

            //Assert
            Assert.True(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task DeleteProductOption_Returns_Unsuccessful_When_ProductOption_Not_Found()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductOptionsByProductIdAndOptionsId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(null as ProductOption)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.DeleteProductOption(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"));

            //Assert
            Assert.False(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task DeleteProductOption_Deletes_ProductOption_Successfully()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductOptionsByProductIdAndOptionsId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new ProductOption())
                .Verifiable();

            mockProductRepository.Setup(m => m.DeleteProductOption(It.IsAny<Guid>()))
                .ReturnsAsync(true)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.DeleteProductOption(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"), Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"));

            //Assert
            Assert.True(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

        [Fact]
        public async Task DeleteProduct_Deletes_Product_Successfully()
        {
            //Arrange
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<ILogger<ProductService>> mockLogger = new Mock<ILogger<ProductService>>();

            mockProductRepository.Setup(m => m.GetProductOptionsByProductId(It.IsAny<Guid>()))
                .ReturnsAsync(new List<ProductOption> { new ProductOption(), new ProductOption() })
                .Verifiable();

            mockProductRepository.SetupSequence(m => m.DeleteProductOption(It.IsAny<Guid>()))
                .ReturnsAsync(true)
                .ReturnsAsync(true);

            mockProductRepository.Setup(m => m.DeleteProduct(It.IsAny<Guid>())) 
                .ReturnsAsync(true)
                .Verifiable();

            ProductService productService = new ProductService(mockProductRepository.Object, mockLogger.Object);

            //Act
            var result = await productService.DeleteProduct(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"));

            //Assert
            Assert.True(result.IsSuccessful);

            mockProductRepository.VerifyAll();
            mockLogger.VerifyAll();
        }

    }
}
