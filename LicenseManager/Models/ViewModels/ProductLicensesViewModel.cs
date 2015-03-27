namespace LicenseManager.Models.ViewModels
{
    public class ProductLicensesViewModel
    {
        public ProductLicensesViewModel(Product product)
        {
            Product = product;
        }

        /// <summary>
        /// The product itself.
        /// </summary>
        public Product Product { get; set; }
    }
}