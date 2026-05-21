using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Models.DTOs.Req;

namespace ProductCatalog.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        //[Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterDto filter)
        {
            var response = await _productService.GetPagedProductsAsync(filter);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequestDto request)
        {
            var response = await _productService.CreateProductAsync(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequestDto request)
        {
            var response = await _productService.UpdateProductAsync(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}