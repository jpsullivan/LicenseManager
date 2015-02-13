using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LicenseManager.Models;

namespace LicenseManager.Areas.Admin.ViewModels
{
    public class ProductsViewModels
    {
        public ProductListModel ProductList { get; set; }
    }

    public class ProductListModel
    {
        public IEnumerable<Product> Products { get; set; }

        public ProductListModel(IEnumerable<Product> products)
        {
            Products = products;
        }
    }

    public class NewProductModel
    {
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Product Url")]
        public string Url { get; set; }
    }
}