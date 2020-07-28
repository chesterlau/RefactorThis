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
		private const string ConnectionString = "Data Source=App_Data/products.db";
		private readonly SqliteConnection _sqliteConnection;
		private readonly ILogger<SqliteProductRepository> _logger;

		public SqliteProductRepository(ILogger<SqliteProductRepository> logger)
		{
			_sqliteConnection = new SqliteConnection(ConnectionString);
			_logger = logger;
		}

		public async Task<Products> GetAllProducts()
		{
			_logger.LogInformation($"Getting all products from the database");

			Products products = new Products
			{
				Items = new List<Product>()
			};

			var conn = _sqliteConnection;
			await conn.OpenAsync();
			var cmd = conn.CreateCommand();
			cmd.CommandText = $"select * from Products";

			var rdr = await cmd.ExecuteReaderAsync();
			while (await rdr.ReadAsync())
			{
				products.Items.Add(
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
	}
}
