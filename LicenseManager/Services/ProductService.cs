using System;
using System.Collections.Generic;
using LicenseManager.Models;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Services
{
    public class ProductService : IProductService
    {
        /// <summary>
        /// Retrieves a signle <see cref="Product"/> based on its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id)
        {
            return Current.DB.Products.Get(id);
        }

        /// <summary>
        /// Returns all products registered in LM.
        /// </summary>
        /// <returns>A collection of <see cref="Product"/> objects.</returns>
        public IEnumerable<Product> GetProducts()
        {
            return Current.DB.Products.All();
        }

        /// <summary>
        /// Adds a new <see cref="Product"/> to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            product.CreatedUtc = DateTime.UtcNow;
            product.LastUpdatedUtc = DateTime.UtcNow;

            var success = Current.DB.Products.Insert(new
            {
                product.Name,
                product.Description,
                product.Url,
                product.CreatedBy,
                product.CreatedUtc,
                product.LastUpdatedUtc
            });

            if (success == null)
            {
                // insert failed. todo: throw message here
                return product;
            }

            product.Id = success.Value;
            return product;
        }

        /// <summary>
        /// Updates an existing <see cref="Product"/> within the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product UpdateProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes a product from the database.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProduct(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}