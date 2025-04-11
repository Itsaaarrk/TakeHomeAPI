using System.ComponentModel.DataAnnotations;

namespace TakeHomeAPI.Models
{
    public class ProductResponseV2
    {
        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The field must contain digits only.")]
        public int ProductID { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "ProductName must be at least 4 characters.")]
        public required string ProductName { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Description must be at least 4 characters.")]
        public required string Description { get; set; }

        // Only include top-level packaging (i.e. where ParentPackagingID is null)
        public List<PackagingResponse> Packagings { get; set; }
    }
}
