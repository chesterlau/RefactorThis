using ProductManagement.Contracts.Dto;
using System;
using System.Threading.Tasks;

namespace ProductManagement.Core.Services
{
    public interface IProductService
    {
        Task<GetAllProductsResponse> GetAllProductsWithOptionalNameFilter(string name);
        Task<GetProductByIdResponse> GetProductById(Guid id);
        Task<CreateProductResponse> CreateProduct(CreateProductRequest createProductRequest);
        Task<UpdateProductResponse> UpdateProduct(Guid id, UpdateProductRequest updateProductRequest);
        Task<GetProductOptionsByProductIdResponse> GetProductOptionsByProductId(Guid id);
        Task<GetProductOptionsByProductIdAndOptionsIdResponse> GetProductOptionsByProductIdAndOptionsId(Guid productId, Guid optionId);
        Task<CreateProductOptionResponse> CreateProductOption(Guid productId, CreateProductOptionRequest createProductOptionRequest);
        Task<UpdateProductOptionResponse> UpdateProductOption(Guid productId, Guid optionId, UpdateProductOptionRequest updateProductOptionRequest);
    }
}
