using ProductManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Data.Repositories
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAllProducts();
		Task<Product> GetProductById(Guid id);
		Task<List<Product>> GetProductsByLikeFilter(string column, object value);
		Task<bool> CreateProduct(Product product);
		Task<bool> UpdateProduct(Product product);
		Task<List<ProductOption>> GetProductOptionsByProductId(Guid productId);
		Task<ProductOption> GetProductOptionsByProductIdAndOptionsId(Guid productId, Guid optionId);
		Task<bool> CreateProductOption(ProductOption productOption);
		Task<bool> UpdateProductOption(ProductOption productOption);
		Task<bool> DeleteProductOption(Guid id);
	}
}
