namespace TakeHomeAPI.Models
{
    public class PackagingResponse
    {

        public int PackagingID { get; set; }
        public string PackagingType { get; set; }
        public string PackagingName { get; set; }
        public List<ItemResponse> Items { get; set; }
        public List<PackagingResponse> ChildPackagings { get; set; }
    }
}
