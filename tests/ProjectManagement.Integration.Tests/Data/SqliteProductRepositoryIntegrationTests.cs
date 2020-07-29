using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagement.Contracts.Models;
using ProductManagement.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagement.Integration.Tests.Data
{
    public class SqliteProductRepositoryIntegrationTests
    {
        [Fact]
        public async Task Test_Creation_And_Retrieval_Of_Products()
        {
            //Arrange
            Mock<ILogger<SqliteProductRepository>> mockLogger = new Mock<ILogger<SqliteProductRepository>>();

            SqliteProductRepository sqliteProductRepository = new SqliteProductRepository(BuildConfiguration(), mockLogger.Object);

            Product newProduct = new Product
            {
                DeliveryPrice = 20, 
                Description = "Test Desc", 
                Name = "Test Product", 
                Price = 1000 
            };

            await sqliteProductRepository.CreateProduct(newProduct);

            //Act
            var result = await sqliteProductRepository.GetAllProducts();

            //Assert
            Assert.Equal(newProduct.Name, result[0].Name);
            Assert.Equal(newProduct.Description, result[0].Description);            
            Assert.Equal(newProduct.Price, result[0].Price);
            Assert.Equal(newProduct.DeliveryPrice, result[0].DeliveryPrice);

            //Cleanup
            await sqliteProductRepository.DeleteAllData();
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
