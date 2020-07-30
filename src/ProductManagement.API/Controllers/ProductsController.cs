using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Dto;
using ProductManagement.Core.Services;
using System;
using System.Threading.Tasks;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/products")]
        [ProducesResponseType(typeof(GetAllProductsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProducts([FromQuery] string name)
        {
            try
            {
                _logger.LogInformation($"[GET] /Products");

                var result = await _productService.GetAllProductsWithOptionalNameFilter(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpGet("/api/products/{id}")]
        [ProducesResponseType(typeof(GetProductByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                _logger.LogInformation($"[GET] /Products/{id}");

                var result = await _productService.GetProductById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpPost("/api/products/")]
        [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest createProductRequest)
        {
            try
            {
                _logger.LogInformation($"[POST] /Products");

                var result = await _productService.CreateProduct(createProductRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpPut("/api/products/{id}")]
        [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest updateProductRequest)
        {
            try
            {
                _logger.LogInformation($"[Put] /Products/{id}");

                var result = await _productService.UpdateProduct(id, updateProductRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpGet("/api/products/{id}/options")]
        [ProducesResponseType(typeof(GetProductOptionsByProductIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductOptionsByProductId(Guid id)
        {
            try
            {
                _logger.LogInformation($"[GET] /api/Products/{id}/options");

                var result = await _productService.GetProductOptionsByProductId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpGet("/api/products/{id}/options/{optionId}")]
        [ProducesResponseType(typeof(GetProductOptionsByProductIdAndOptionsIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductOptionsByProductId(Guid id, Guid optionId)
        {
            try
            {
                _logger.LogInformation($"[GET] /api/Products/{id}/options/{optionId}");

                var result = await _productService.GetProductOptionsByProductIdAndOptionsId(id, optionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpPost("/api/products/{id}/options")]
        [ProducesResponseType(typeof(CreateProductOptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), 400)]
        public async Task<IActionResult> CreateProductOption(Guid id, [FromBody] CreateProductOptionRequest createProductOptionRequest)
        {
            try
            {
                _logger.LogInformation($"[POST] /Products/{id}/options");

                var result = await _productService.CreateProductOption(id, createProductOptionRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpPut("/api/products/{id}/options/{optionId}")]
        [ProducesResponseType(typeof(UpdateProductOptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductOption(Guid id, Guid optionId, [FromBody] UpdateProductOptionRequest updateProductOptionRequest)
        {
            try
            {
                _logger.LogInformation($"[Put] /products/{id}/options/{optionId}");
                 
                var result = await _productService.UpdateProductOption(id, optionId, updateProductOptionRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpDelete("/api/products/{id}/options/{optionId}")]
        [ProducesResponseType(typeof(DeleteProductOptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), 400)]
        public async Task<IActionResult> DeleteProductOption(Guid id, Guid optionId)
        {
            try
            {
                _logger.LogInformation($"[DELETE] /products/{id}/options/{optionId}");

                var result = await _productService.DeleteProductOption(id, optionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        [HttpDelete("/api/products/{id}")]
        [ProducesResponseType(typeof(DeleteProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                _logger.LogInformation($"[DELETE] /products/{id}");

                var result = await _productService.DeleteProduct(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }
    }
}