using System.Collections.Generic;
using LicenseManager.Models;

namespace LicenseManager.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a signle <see cref="Product"/> based on its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProduct(int id);

        /// <summary>
        /// Returns all products registered in LM.
        /// </summary>
        /// <returns>A collection of <see cref="Product"/> objects.</returns>
        IEnumerable<Product> GetProducts();

        /// <summary>
        /// Adds a new <see cref="Product"/> to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Product CreateProduct(Product product);

        /// <summary>
        /// Updates an existing <see cref="Product"/> within the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Product UpdateProduct(Product product);

        /// <summary>
        /// Removes a product from the database.
        /// </summary>
        /// <param name="id"></param>
        void DeleteProduct(int id);
    }
}