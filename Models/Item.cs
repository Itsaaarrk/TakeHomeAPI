namespace TakeHomeAPI.Models
{
    public class Item
    {
        // Maps to dbo.Item(ItemID)
        public int ItemID { get; set; }

        // Foreign key pointing to Packaging
        public int PackagingID { get; set; }

        // Item name – max 100 characters.
        public string ItemName { get; set; }

        // Navigation property to the associated Packaging.
        public Packaging Packaging { get; set; }
    }
}
