using ProductManagement.Contracts.Models;

namespace ProductManagement.Core.Services
{
	public interface IProductService
	{
		Products GetAllProducts();
	}
}
