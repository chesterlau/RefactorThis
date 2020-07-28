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

        [HttpGet]
        [ProducesResponseType(typeof(ProductManagement.Contracts.Models.Products), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"[GET] /Products");
                var products = _productService.GetAllProducts();

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught!");
                return BadRequest(new ApiResult { Title = "An error has occured" });
            }
        }

        //[HttpGet] (Done)
        //public Products Get()
        //{
        //    return new Products();
        //}

        //[HttpGet("{id}")]
        //public Product Get(Guid id)
        //{
        //    var product = new Product(id);
        //    if (product.IsNew)
        //        throw new Exception();

        //    return product;
        //}

        //[HttpPost]
        //public void Post(Product product)
        //{
        //    product.Save();
        //}

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

        //[HttpGet("{productId}/options")]
        //public ProductOptions GetOptions(Guid productId)
        //{
        //    return new ProductOptions(productId);
        //}

        //[HttpGet("{productId}/options/{id}")]
        //public ProductOption GetOption(Guid productId, Guid id)
        //{
        //    var option = new ProductOption(id);
        //    if (option.IsNew)
        //        throw new Exception();

        //    return option;
        //}

        //[HttpPost("{productId}/options")]
        //public void CreateOption(Guid productId, ProductOption option)
        //{
        //    option.ProductId = productId;
        //    option.Save();
        //}

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

        //[HttpDelete("{productId}/options/{id}")]
        //public void DeleteOption(Guid id)
        //{
        //    var opt = new ProductOption(id);
        //    opt.Delete();
        //}
    }
}