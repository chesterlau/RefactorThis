using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Core.Services;
using System;
using System.Threading.Tasks;

namespace ProductManagement.API.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// Gets the all the products
        /// </summary>
        /// <param name="name">Optional query of to filter by product name</param>
        /// <returns>The products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetAllProductsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProducts([FromQuery] string name)
        {
            try
            {
                _logger.LogInformation($"[GET] /products");

                var result = await _productService.GetAllProductsWithOptionalNameFilter(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        /// <summary>
        /// Gets the product by the product identifier
        /// </summary>
        /// <param name="id">The product idenfitier</param>
        /// <returns>The product</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetProductByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetProductByIdResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                _logger.LogInformation($"[GET] /products/{id}");

                var result = await _productService.GetProductById(id);

                if(result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }
        
        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="createProductRequest">The details of the new product</param>
        /// <returns>Result to indicate whether the creation was successful</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest createProductRequest)
        {
            try
            {
                _logger.LogInformation($"[POST] /products");

                var result = await _productService.CreateProduct(createProductRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="id">The product identifier to update</param>
        /// <param name="updateProductRequest">The details of the product to update</param>
        /// <returns>Result to indicate whether the update was successful</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest updateProductRequest)
        {
            try
            {
                _logger.LogInformation($"[Put] /products/{id}");

                var result = await _productService.UpdateProduct(id, updateProductRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        /// <summary>
        /// Gets the product options by the product identifier
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <returns>The product options associated to the product</returns>
        [HttpGet("{id}/options")]
        [ProducesResponseType(typeof(GetProductOptionsByProductIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductOptionsByProductId(Guid id)
        {
            try
            {
                _logger.LogInformation($"[GET] /products/{id}/options");

                var result = await _productService.GetProductOptionsByProductId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        /// <summary>
        /// Gets the product option by product identifier and product option identifier
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <param name="optionId">The product option identifier</param>
        /// <returns>The product option that is associated to the product identifier and product options identifier</returns>
        [HttpGet("{id}/options/{optionId}")]
        [ProducesResponseType(typeof(GetProductOptionsByProductIdAndOptionsIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetProductOptionsByProductIdAndOptionsIdResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductOptionByProductIdAndOptionId(Guid id, Guid optionId)
        {
            try
            {
                _logger.LogInformation($"[GET] /products/{id}/options/{optionId}");

                var result = await _productService.GetProductOptionsByProductIdAndOptionsId(id, optionId);

                if(result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        /// <summary>
        /// Creates a new product option
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <param name="createProductOptionRequest">The details of the new product option</param>
        /// <returns>Result to indicate whether the creation was successful</returns>
        [HttpPost("{id}/options")]
        [ProducesResponseType(typeof(CreateProductOptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult), 400)]
        public async Task<IActionResult> CreateProductOption(Guid id, [FromBody] CreateProductOptionRequest createProductOptionRequest)
        {
            try
            {
                _logger.LogInformation($"[POST] /products/{id}/options");

                var result = await _productService.CreateProductOption(id, createProductOptionRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Error = "An error has occured" });
            }
        }

        /// <summary>
        /// Updates a product option
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <param name="optionId">The product option identifier</param>
        /// <param name="updateProductOptionRequest">The details of the product option to udpate</param>
        /// <returns>Result to indicate whether the creation was successful</returns>
        [HttpPut("{id}/options/{optionId}")]
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

        /// <summary>
        /// Deletes a product option
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <param name="optionId">The product option identifier</param>
        /// <returns>Result to indicate whether the deletion was successful</returns>
        [HttpDelete("{id}/options/{optionId}")]
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

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <returns>Result to indicate whether the deletion was successful</returns>
        [HttpDelete("{id}")]
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