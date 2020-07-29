using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Data.Repositories
{
    public class SqliteProductRepository : IProductRepository
    {
        private readonly ILogger<SqliteProductRepository> _logger;
        private readonly IConfiguration _configuration;

        public SqliteProductRepository(IConfiguration configuration, ILogger<SqliteProductRepository> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            _logger.LogInformation($"Getting all products from the database");

            var products = new List<Product>();

            using(var conn = CreateConnection())
            {
                await conn.OpenAsync();

                var cmd = conn.CreateCommand();
                cmd.CommandText = $"select * from Products";

                var rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    products.Add(
                        new Product
                        {
                            Id = Guid.Parse(rdr["Id"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                            Price = decimal.Parse(rdr["Price"].ToString()),
                            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
                        }
                    );
                }
            }

            return products;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            _logger.LogInformation($"Getting product by id from database");

            Product product = new Product();

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"select * from Products where id = '{id}' collate nocase";

                var rdr = await cmd.ExecuteReaderAsync();

                if (!await rdr.ReadAsync())
                    return null;

                product.Id = Guid.Parse(rdr["Id"].ToString());
                product.Name = rdr["Name"].ToString();
                product.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                product.Price = decimal.Parse(rdr["Price"].ToString());
                product.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
            }

            return product;
        }

        public async Task<List<Product>> GetProductsLikeName(string productName)
        {
            _logger.LogInformation($"Getting products from database with name like {productName}");

            var products = new List<Product>();

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"select * from Products where lower(name) like '%{productName}%'";

                var rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    products.Add(
                        new Product
                        {
                            Id = Guid.Parse(rdr["Id"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                            Price = decimal.Parse(rdr["Price"].ToString()),
                            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
                        }
                    );
                }
            }

            return products;
        }

        public async Task<bool> CreateProduct(Product product)
        {
            _logger.LogInformation($"Creating a product in the database");

            product.Id = Guid.NewGuid();

            int rowsAdded = 0;
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"insert into Products (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})";

                rowsAdded = await cmd.ExecuteNonQueryAsync();
            }

            return rowsAdded > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _logger.LogInformation($"Updating a product in the database");

            int rowsUpdated = 0;
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"update Products set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}' collate nocase";

                rowsUpdated = await cmd.ExecuteNonQueryAsync();
            }

            return rowsUpdated > 0;
        }

        public async Task<List<ProductOption>> GetProductOptionsByProductId(Guid productId)
        {
            _logger.LogInformation($"Getting product options from the database");

            var productOptions = new List<ProductOption>();

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"select * from productoptions where productid = '{productId}' collate nocase";

                var rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    productOptions.Add(
                        new ProductOption
                        {
                            Id = Guid.Parse(rdr["Id"].ToString()),
                            ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString()
                        }
                    );
                }
            }

            return productOptions;
        }

        public async Task<ProductOption> GetProductOptionsByProductIdAndOptionsId(Guid productId, Guid optionId)
        {
            _logger.LogInformation($"Getting product options by productid and options id from the database");

            var productOption = new ProductOption();

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"select * from productoptions where id = '{optionId}' collate nocase and productid = '{productId}' collate nocase";

                var rdr = await cmd.ExecuteReaderAsync();

                if (!await rdr.ReadAsync())
                    return null;

                productOption.Id = Guid.Parse(rdr["Id"].ToString());
                productOption.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                productOption.Name = rdr["Name"].ToString();
                productOption.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
            }

            return productOption;
        }

        public async Task<bool> CreateProductOption(ProductOption productOption)
        {
            _logger.LogInformation($"Creating a productOption in the database");

            productOption.Id = Guid.NewGuid();

            int rowsAdded = 0;
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"insert into productoptions (id, productid, name, description) values ('{productOption.Id}', '{productOption.ProductId}', '{productOption.Name}', '{productOption.Description}')";

                rowsAdded = await cmd.ExecuteNonQueryAsync();
            }

            return rowsAdded > 0;
        }

        public async Task<bool> UpdateProductOption(ProductOption productOption)
        {
            _logger.LogInformation($"Updating a product option in the database");

            int rowsUpdated = 0;
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"update productoptions set name = '{productOption.Name}', description = '{productOption.Description}' where id = '{productOption.Id}' collate nocase";

                rowsUpdated = await cmd.ExecuteNonQueryAsync();
            }

            return rowsUpdated > 0;
        }

        public async Task<bool> DeleteProductOption(Guid optionid)
        {
            _logger.LogInformation($"Deleting a product option in the database");

            int rowsDeleted = 0;
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"delete from productoptions where id = '{optionid}' collate nocase";

                rowsDeleted = await cmd.ExecuteNonQueryAsync();
            }

            return rowsDeleted > 0;
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            _logger.LogInformation($"Deleting a product in the database");

            int rowsDeleted = 0;
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"delete from Products where id = '{productId}' collate nocase";

                rowsDeleted = await cmd.ExecuteNonQueryAsync();
            }

            return rowsDeleted > 0;
        }

        /// <summary>
        /// Note that this is a destructive operation and it is only used for integration test cleanups
        /// </summary>
        public async Task DeleteAllData()
        {
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"delete from products";

                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = $"delete from productoptions";

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_configuration["ConnectionString"]);
        }
    }
}
