using ProductManagement.Contracts.Models;

namespace ProductManagement.Data.Repositories
{
	public interface IProductRepository
	{
		Products GetAllProducts();
	}
}
