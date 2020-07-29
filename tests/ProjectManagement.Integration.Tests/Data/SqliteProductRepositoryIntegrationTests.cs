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
        public async Task Test_Creation_And_Retrieval_Of_Products()
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
