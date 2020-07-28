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
        private readonly SqliteConnection _sqliteConnection;
        private readonly ILogger<SqliteProductRepository> _logger;

        public SqliteProductRepository(ILogger<SqliteProductRepository> logger)
        {
            _sqliteConnection = new SqliteConnection(ConnectionString);
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            _logger.LogInformation($"Getting all products from the database");

            var products = new List<Product>();

            var conn = _sqliteConnection;
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

            await conn.CloseAsync();

            return products;
        }

        public async Task<Product> GetProductByWhereFilter(string column, object value)
        {
            _logger.LogInformation($"Getting product from database with where filter column: {column} and value: {value}");

            var conn = _sqliteConnection;
            await conn.OpenAsync();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select * from Products where {column} = '{value}' collate nocase";

            var rdr = await cmd.ExecuteReaderAsync();

            if (!await rdr.ReadAsync())
                return null;

            var product = new Product
            {
                Id = Guid.Parse(rdr["Id"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                Price = decimal.Parse(rdr["Price"].ToString()),
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
            };

            await conn.CloseAsync();

            return product;
        }

        public async Task<List<Product>> GetProductsByLikeFilter(string column, object value)
        {
            _logger.LogInformation($"Getting products from database with like filter column: {column} and value: {value}");

            var products = new List<Product>();

            var conn = _sqliteConnection;
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

            await conn.CloseAsync();

            return products;
        }

        public async Task<bool> CreateProduct(Product product)
        {
            _logger.LogInformation($"Creating a product in the database");

            var conn = _sqliteConnection;
            await conn.OpenAsync();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"insert into Products (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})";

            await conn.OpenAsync();
            var rowsAdded = await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return rowsAdded > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _logger.LogInformation($"Updating a product in the database");

            var conn = _sqliteConnection;
            await conn.OpenAsync();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"update Products set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}' collate nocase";

            await conn.OpenAsync();
            var rowsUpdated = await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return rowsUpdated > 0;
        }
    }
}
