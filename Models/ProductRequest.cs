namespace TakeHomeAPI.Models
{
    public class ProductRequest
    {
        public required string Name { get; set; }
        public int? PackagingId { get; set; }
    }
}
