using System.Collections.Generic;
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
}