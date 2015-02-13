namespace LicenseManager.Models
{
    public class ProductVersion
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Version { get; set; }
    }
}