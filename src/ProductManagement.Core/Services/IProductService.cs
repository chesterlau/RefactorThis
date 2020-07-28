using ProductManagement.Contracts.Dto;
using System;
using System.Threading.Tasks;

namespace ProductManagement.Core.Services
{
    public interface IProductService
    {
        Task<GetAllProductsResponse> GetAllProductsWithOptionalNameFilter(string name);

        Task<GetProductByIdResponse> GetProductById(Guid id);
    }
}
