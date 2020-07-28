using ProductManagement.Contracts.Models;
using System.Threading.Tasks;

namespace ProductManagement.Data.Repositories
{
	public interface IProductRepository
	{
		Task<Products> GetAllProducts();
	}
}
