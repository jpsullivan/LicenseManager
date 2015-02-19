namespace LicenseManager.Models
{
    public class ProductFeature
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int VersionId { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        public string DataType { get; set; }

        public string DefaultValue { get; set; }
    }
}