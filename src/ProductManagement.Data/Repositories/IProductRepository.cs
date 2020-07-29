using ProductManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Data.Repositories
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAllProducts();
		Task<Product> GetProductByWhereFilter(string column, object value);
		Task<List<Product>> GetProductsByLikeFilter(string column, object value);
		Task<bool> CreateProduct(Product product);
		Task<bool> UpdateProduct(Product product);
		Task<List<ProductOption>> GetProductOptionsByProductId(Guid productId);
		Task<ProductOption> GetProductOptionsByProductIdAndOptionsId(Guid productId, Guid optionId);
	}
}
