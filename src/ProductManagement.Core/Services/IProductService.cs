using ProductManagement.Contracts.Dto;
using ProductManagement.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace ProductManagement.Core.Services
{
    public interface IProductService
    {
        Task<GetAllProductsResponse> GetAllProducts();

        Task<GetProductByIdResponse> GetProductById(Guid id);
    }
}
