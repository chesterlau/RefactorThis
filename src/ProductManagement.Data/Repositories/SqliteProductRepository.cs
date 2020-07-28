using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Models;
using System;
using System.Collections.Generic;

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

		public Products GetAllProducts()
		{
			_logger.LogInformation($"Getting all products from the database");

			Products products = new Products
			{
				Items = new List<Product>()
			};

			var conn = _sqliteConnection;
			conn.Open();
			var cmd = conn.CreateCommand();
			cmd.CommandText = $"select * from Products";

			var rdr = cmd.ExecuteReader();
			while (rdr.Read())
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

			return products;
		}
	}
}
