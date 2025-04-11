using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeHomeAPI.Data;
using TakeHomeAPI.Models;

namespace TakeHomeAPI.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/products")]

    public class ProductsV2Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsV2Controller(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/v2/products – returns products with nested packaging and item details
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            // Include Packagings and Items for each Product.
            // Note: For recursive children, using EF Core’s Include for multiple levels may be limited.
            var products = await _context.Products
               .Include(p => p.Packagings)
                   .ThenInclude(pack => pack.Items)
               .Include(p => p.Packagings)
                   .ThenInclude(pack => pack.ChildPackagings) // loads one level of children
               .ToListAsync();

            var result = products.Select(p => new ProductResponseV2
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Description = p.Description,
                Packagings = p.Packagings
                   .Where(pack => pack.ParentPackagingID == null) // Select only top-level packaging
                   .Select(pack => MapPackaging(pack))
                   .ToList()
            }).ToList();

            return Ok(result);
        }

        // Recursively map a Packaging entity to a PackagingResponse DTO.
        private PackagingResponse MapPackaging(Packaging pack)
        {
            return new PackagingResponse
            {
                PackagingID = pack.PackagingID,
                PackagingType = pack.PackagingType,
                PackagingName = pack.PackagingName,
                Items = pack.Items?.Select(i => new ItemResponse
                {
                    ItemID = i.ItemID,
                    ItemName = i.ItemName
                }).ToList() ?? new List<ItemResponse>(),
                ChildPackagings = pack.ChildPackagings?.Select(child => MapPackaging(child)).ToList() ?? new List<PackagingResponse>()
            };
        }
    }
}
