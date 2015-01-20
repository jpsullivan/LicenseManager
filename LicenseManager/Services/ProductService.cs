using System.Collections.Generic;
using LicenseManager.Models;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Services
{
    public class ProductService : IProductService
    {
        public Product GetProduct(int id)
        {
            return Current.DB.Products.Get(id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return Current.DB.Products.All();
        }

        public Product CreateProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public Product UpdateProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteProduct(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}