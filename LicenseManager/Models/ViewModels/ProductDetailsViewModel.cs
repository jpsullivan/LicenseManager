namespace LicenseManager.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel(Product product)
        {
            Product = product;
        }

        /// <summary>
        /// The product itself.
        /// </summary>
        public Product Product { get; set; }
    }
}