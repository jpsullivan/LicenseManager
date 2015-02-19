using System;

namespace LicenseManager.Models
{
    public class ProductVersion
    {
        public ProductVersion()
        {
        }

        public ProductVersion(int productId, string version)
        {
            ProductId = productId;
            Version = version;
        }

        public ProductVersion(int id, int productId, string version, DateTime createdUtc)
        {
            Id = id;
            ProductId = productId;
            Version = version;
            CreatedUtc = createdUtc;
        }

        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Version { get; set; }

        public DateTime? CreatedUtc { get; set; }
    }
}