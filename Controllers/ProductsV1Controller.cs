using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TakeHomeAPI.Data;
using TakeHomeAPI.Models;

namespace TakeHomeAPI.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/products")]
    [SwaggerTag("Products")]

    public class ProductsV1Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsV1Controller(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/v1/products
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
               .Select(p => new ProductResponseV1
               {
                   ProductID = p.ProductID,
                   ProductName = p.ProductName,
                   Description = p.Description
               })
               .ToListAsync();

            return Ok(products);
        }

        // Optional: POST /api/v1/products to add a new product.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductResponseV1 productDto)
        {
            if (string.IsNullOrEmpty(productDto.ProductName))
                return BadRequest("Product name is required.");

            var product = new Product
            {
                ProductName = productDto.ProductName,
                Description = productDto.Description
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.ProductID }, productDto);
        }
    }
}
