using System.Collections.Generic;
using LicenseManager.Models;

namespace LicenseManager.Services.Interfaces
{
    public interface IProductVersionService
    {
        /// <summary>
        /// Retrieves a collection of product versions for the specified product.
        /// </summary>
        /// <param name="productId">The ID of the product to fetch versions for.</param>
        /// <returns></returns>
        IEnumerable<ProductVersion> GetVersions(int productId);

        /// <summary>
        /// Adds a new product version to the database.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="version"></param>
        /// <returns>The newly created <see cref="ProductVersion"/>.</returns>
        ProductVersion CreateVersion(int productId, string version);

        /// <summary>
        /// Updates an existing product version with the new version number/name.
        /// </summary>
        /// <param name="id">The product version ID to update.</param>
        /// <param name="version">The updated version.</param>
        /// <returns>The updated <see cref="ProductVersion"/>.</returns>
        ProductVersion UpdateVersion(int id, string version);

        /// <summary>
        /// Deletes a product version.
        /// </summary>
        /// <param name="id">The product version ID to be deleted.</param>
        void DeleteVersion(int id);
    }
}