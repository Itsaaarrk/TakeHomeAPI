namespace TakeHomeAPI.Models
{
    public class Packaging
    {
        // Maps to dbo.Packaging(PackagingID)
        public int PackagingID { get; set; }

        // Foreign key to Product table
        public int ProductID { get; set; }

        // Self-referential foreign key for nested packaging; can be null.
        public int? ParentPackagingID { get; set; }

        // PackagingType (Box, Packet, etc.) – max 50 characters, not-null.
        public string PackagingType { get; set; }

        // PackagingName – optional descriptive name, max 100 characters.
        public string PackagingName { get; set; }

        // Navigation property to the related Product.
        public Product Product { get; set; }

        // Navigation: a Packaging may have a parent packaging.
        public Packaging ParentPackaging { get; set; }

        // Navigation: a Packaging may have multiple child packagings.
        public ICollection<Packaging> ChildPackagings { get; set; }

        // Navigation: a Packaging may contain many Items.
        public ICollection<Item> Items { get; set; }
    }
}
