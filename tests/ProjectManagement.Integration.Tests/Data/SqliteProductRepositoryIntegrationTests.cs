using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagement.Contracts.Models;
using ProductManagement.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagement.Integration.Tests.Data
{
    public class SqliteProductRepositoryIntegrationTests : IDisposable
    {
        [Fact]
        public async Task GetAllProducts_Returns_Correct_Products()
        {
            //Arrange
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            Product newProduct1 = new Product
            {
                DeliveryPrice = 20, 
                Description = "Test Desc", 
                Name = "Test Product", 
                Price = 1000 
            };

            Product newProduct2 = new Product
            {
                DeliveryPrice = 50,
                Description = "Test Desc 2",
                Name = "Test Product 2",
                Price = 300
            };

            await sqliteProductRepository.CreateProduct(newProduct1);
            await sqliteProductRepository.CreateProduct(newProduct2);

            //Act
            var result = await sqliteProductRepository.GetAllProducts();

            //Assert
            Assert.Equal(newProduct1.Name, result[0].Name);
            Assert.Equal(newProduct1.Description, result[0].Description);            
            Assert.Equal(newProduct1.Price, result[0].Price);
            Assert.Equal(newProduct1.DeliveryPrice, result[0].DeliveryPrice);

            Assert.Equal(newProduct2.Name, result[1].Name);
            Assert.Equal(newProduct2.Description, result[1].Description);
            Assert.Equal(newProduct2.Price, result[1].Price);
            Assert.Equal(newProduct2.DeliveryPrice, result[1].DeliveryPrice);
        }

        [Fact]
        public async Task GetProductById_Returns_Correct_Product()
        {
            //Arrange
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            Product newProduct1 = new Product
            {
                DeliveryPrice = 20,
                Description = "Test Desc",
                Name = "Test Product",
                Price = 1000
            };

            await sqliteProductRepository.CreateProduct(newProduct1);
            var products = await sqliteProductRepository.GetAllProducts();

            //Act
            var result = await sqliteProductRepository.GetProductById(products[0].Id);

            //Assert
            Assert.Equal(newProduct1.Name, result.Name);
            Assert.Equal(newProduct1.Description, result.Description);
            Assert.Equal(newProduct1.Price, result.Price);
            Assert.Equal(newProduct1.DeliveryPrice, result.DeliveryPrice);
        }

        [Fact]
        public async Task GetProductsLikeName_Returns_Correct_Product()
        {
            //Arrange
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            Product newProduct1 = new Product
            {
                DeliveryPrice = 20,
                Description = "Test Desc",
                Name = "Macbook",
                Price = 1000
            };

            Product newProduct2 = new Product
            {
                DeliveryPrice = 50,
                Description = "Test Desc 2",
                Name = "Samsung",
                Price = 300
            };

            await sqliteProductRepository.CreateProduct(newProduct1);
            await sqliteProductRepository.CreateProduct(newProduct2);

            //Act
            var result = await sqliteProductRepository.GetProductsLikeName("Sam");

            //Assert
            Assert.Equal(newProduct2.Name, result[0].Name);
            Assert.Equal(newProduct2.Description, result[0].Description);
            Assert.Equal(newProduct2.Price, result[0].Price);
            Assert.Equal(newProduct2.DeliveryPrice, result[0].DeliveryPrice);
        }

        [Fact]
        public async Task UpdateProduct_Updates_Product_Correctly()
        {
            //Arrange
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            Product newProduct1 = new Product
            {
                DeliveryPrice = 20,
                Description = "Test Desc",
                Name = "Test Product",
                Price = 1000
            };

            await sqliteProductRepository.CreateProduct(newProduct1);
            var products = await sqliteProductRepository.GetAllProducts();

            Product updatedProduct = new Product
            {
                Id = products[0].Id,
                DeliveryPrice = 200,
                Description = "Test Desc 1",
                Name = "Test Product 1",
                Price = 3000
            };

            //Act
            await sqliteProductRepository.UpdateProduct(updatedProduct);
            var result = await sqliteProductRepository.GetProductById(updatedProduct.Id);

            //Assert
            Assert.Equal(updatedProduct.Id, result.Id);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Description, result.Description);
            Assert.Equal(updatedProduct.Price, result.Price);
            Assert.Equal(updatedProduct.DeliveryPrice, result.DeliveryPrice);
        }

        [Fact]
        public async Task DeleteProduct_Deletes_Product_Correctly()
        {
            //Arrange
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            Product newProduct1 = new Product
            {
                DeliveryPrice = 20,
                Description = "Test Desc",
                Name = "Test Product",
                Price = 1000
            };

            await sqliteProductRepository.CreateProduct(newProduct1);
            var products = await sqliteProductRepository.GetAllProducts();

            //Act
            await sqliteProductRepository.DeleteProduct(products[0].Id);
            var result = await sqliteProductRepository.GetAllProducts();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductOptionsByProductId_Returns_ProductOptions_Correctly()
        {
            //Arrange
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            ProductOption productOption1 = new ProductOption
            {
                Name = "Mac Red",
                Description = "This is a Mac red",
                ProductId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef")
            };

            ProductOption productOption2 = new ProductOption
            {
                Name = "Mac Blue",
                Description = "This is a Mac blue",
                ProductId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef")
            };

            await sqliteProductRepository.CreateProductOption(productOption1);
            await sqliteProductRepository.CreateProductOption(productOption2);

            //Act
            var result = await sqliteProductRepository.GetProductOptionsByProductId(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"));

            //Assert
            Assert.Equal(productOption1.ProductId, result[0].ProductId);
            Assert.Equal(productOption1.Name, result[0].Name);
            Assert.Equal(productOption1.Description, result[0].Description);

            Assert.Equal(productOption2.ProductId, result[1].ProductId);
            Assert.Equal(productOption2.Name, result[1].Name);
            Assert.Equal(productOption2.Description, result[1].Description);
        }

        public void Dispose()
        {
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            sqliteProductRepository.DeleteAllData().Wait();
        }

        private IConfiguration BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory())
               .AddInMemoryCollection(new[] { new KeyValuePair<string, string>("ConnectionString", "Data Source=../../../TestData/products_clean.db") })
              .Build();

            return configuration;
        }

    }
}
