using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Data.Repositories
{
    public class SqliteProductRepository : IProductRepository
    {
        //TODO put in IConfiguration
        private const string ConnectionString = "Data Source=App_Data/products.db";
        private readonly ILogger<SqliteProductRepository> _logger;

        public SqliteProductRepository(ILogger<SqliteProductRepository> logger)
        {
            _logger = logger;
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

        public async Task<Product> GetProductByWhereFilter(string column, object value)
        {
            _logger.LogInformation($"Getting product from database with where filter column: {column} and value: {value}");

            Product product = new Product();

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"select * from Products where {column} = '{value}' collate nocase";

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

        public async Task<List<Product>> GetProductsByLikeFilter(string column, object value)
        {
            _logger.LogInformation($"Getting products from database with like filter column: {column} and value: {value}");

            var products = new List<Product>();

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"select * from Products where lower({column}) like '%{value.ToString().ToLower()}%'";

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

            int rowsAdded = 0;
            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"insert into Products (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})";

                await conn.OpenAsync();
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

                await conn.OpenAsync();
                rowsUpdated = await cmd.ExecuteNonQueryAsync();
            }

            return rowsUpdated > 0;
        }

        private SqliteConnection CreateConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}
