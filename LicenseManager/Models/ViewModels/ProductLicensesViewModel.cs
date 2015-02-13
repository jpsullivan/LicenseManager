using System.Collections.Generic;

namespace LicenseManager.Models.ViewModels
{
    public class ProductLicensesViewModel
    {
        public ProductLicensesViewModel(Product product, IEnumerable<ProductVersion> productVersions)
        {
            Product = product;
            ProductVersions = productVersions;
        }

        /// <summary>
        /// The product itself.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// The product versions for the specified <see cref="Product"/>.
        /// </summary>
        public IEnumerable<ProductVersion> ProductVersions { get; set; }
    }
}