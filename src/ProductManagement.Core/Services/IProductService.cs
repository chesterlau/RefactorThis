using ProductManagement.Contracts.Models;
using System.Threading.Tasks;

namespace ProductManagement.Core.Services
{
	public interface IProductService
	{
		Task<Products> GetAllProducts();
	}
}
