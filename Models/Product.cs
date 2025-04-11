namespace TakeHomeAPI.Models
{
    public class Product
    {
        // Maps to dbo.Product(ProductID)
        public int ProductID { get; set; }

        // Maps to dbo.Product(ProductName) – max 100 characters
        public required string ProductName { get; set; }

        // Maps to dbo.Product(Description) – max 255 characters, nullable
        public required string Description { get; set; }

        // Navigation: A product can have many packagings.
        public ICollection<Packaging> Packagings { get; set; }
    }
}
