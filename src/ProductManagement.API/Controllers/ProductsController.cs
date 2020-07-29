using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagement.API.Models;
using ProductManagement.Contracts.Dto;
using ProductManagement.Core.Services;

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
        [ProducesResponseType(typeof(GetAllProductsResponse), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpGet("/api/products/{id}")]
        [ProducesResponseType(typeof(GetProductByIdResponse), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpPost("/api/products/")]
        [ProducesResponseType(typeof(CreateProductResponse), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpPut("/api/products/{id}")]
        [ProducesResponseType(typeof(UpdateProductResponse), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpGet("/api/products/{id}/options")]
        [ProducesResponseType(typeof(GetProductOptionsByProductIdResponse), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpGet("/api/products/{id}/options/{optionId}")]
        [ProducesResponseType(typeof(GetProductOptionsByProductIdAndOptionsIdResponse), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpPost("/api/products/{id}/options")]
        [ProducesResponseType(typeof(CreateProductOptionResponse), 200)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpPut("/api/products/{id}/options/{optionId}")]
        [ProducesResponseType(typeof(UpdateProductOptionResponse), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        [HttpDelete("/api/products/{id}/options/{optionId}")]
        [ProducesResponseType(typeof(DeleteProductOptionResponse), 200)]
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
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        ////done
        //[HttpGet]
        //public Products Get()
        //{
        //    return new Products();
        //}

        ////done
        //[HttpGet("{id}")]
        //public Product Get(Guid id)
        //{
        //    var product = new Product(id);
        //    if (product.IsNew)
        //        throw new Exception();

        //    return product;
        //}

        ////done
        //[HttpPost]
        //public void Post(Product product)
        //{
        //    product.Save();
        //}

        ////done
        //[HttpPut("{id}")]
        //public void Update(Guid id, Product product)
        //{
        //    var orig = new Product(id)
        //    {
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        DeliveryPrice = product.DeliveryPrice
        //    };

        //    if (!orig.IsNew)
        //        orig.Save();
        //}

        //[HttpDelete("{id}")]
        //public void Delete(Guid id)
        //{
        //    var product = new Product(id);
        //    product.Delete();
        //}

        ////done
        //[HttpGet("{productId}/options")]
        //public ProductOptions GetOptions(Guid productId)
        //{
        //    return new ProductOptions(productId);
        //}

        ////done
        //[HttpGet("{productId}/options/{id}")]
        //public ProductOption GetOption(Guid productId, Guid id)
        //{
        //    var option = new ProductOption(id);
        //    if (option.IsNew)
        //        throw new Exception();

        //    return option;
        //}

        ////done
        //[HttpPost("{productId}/options")]
        //public void CreateOption(Guid productId, ProductOption option)
        //{
        //    option.ProductId = productId;
        //    option.Save();
        //}

        ////done
        //[HttpPut("{productId}/options/{id}")]
        //public void UpdateOption(Guid id, ProductOption option)
        //{
        //    var orig = new ProductOption(id)
        //    {
        //        Name = option.Name,
        //        Description = option.Description
        //    };

        //    if (!orig.IsNew)
        //        orig.Save();
        //}

        ////done
        //[HttpDelete("{productId}/options/{id}")]
        //public void DeleteOption(Guid id)
        //{
        //    var opt = new ProductOption(id);
        //    opt.Delete();
        //}
    }
}