using System;
using System.Collections.Generic;
using LicenseManager.Models;
using LicenseManager.Services.Interfaces;
using Microsoft.Ajax.Utilities;

namespace LicenseManager.Services
{
    public class ProductVersionService : IProductVersionService
    {
        /// <summary>
        /// Retrieves a single <see cref="ProductVersion"/>.
        /// </summary>
        /// <param name="id">The product version ID.</param>
        /// <returns></returns>
        public ProductVersion GetVersion(int id)
        {
            return Current.DB.ProductVersions.Get(id);
        }

        /// <summary>
        /// Retrieves a collection of product versions for the specified product.
        /// </summary>
        /// <param name="productId">The ID of the product to fetch versions for.</param>
        /// <returns></returns>
        public IEnumerable<ProductVersion> GetVersions(int productId)
        {
            if (productId == 0)
            {
                throw new ArgumentException("Product ID must be greater than zero.", "productId");
            }

            const string sql = "select * from ProductVersions where ProductId = @productId Order By CreatedUtc DESC";
            return Current.DB.Query<ProductVersion>(sql, new {productId});
        }

        /// <summary>
        /// Adds a new product version to the database.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="version"></param>
        /// <returns>The newly created <see cref="ProductVersion"/>.</returns>
        public ProductVersion CreateVersion(int productId, string version)
        {
            if (productId == 0)
            {
                throw new ArgumentException("Product ID must be greater than zero.", "productId");
            }

            if (version.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("version");
            }

            var createdUtc = DateTime.UtcNow;
            var success = Current.DB.ProductVersions.Insert(new
            {
                productId,
                version,
                createdUtc
            });

            if (success == null)
            {
                return null;
            }

            var id = success.Value;
            return GetVersion(id);
        }

        /// <summary>
        /// Adds a new product version to the database.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public ProductVersion CreateVersion(ProductVersion version)
        {
            return CreateVersion(version.ProductId, version.Version);
        }

        /// <summary>
        /// Updates an existing product version with the new version number/name.
        /// </summary>
        /// <param name="id">The product version ID to update.</param>
        /// <param name="version">The updated version.</param>
        /// <returns>The updated <see cref="ProductVersion"/>.</returns>
        public ProductVersion UpdateVersion(int id, string version)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a product version.
        /// </summary>
        /// <param name="id">The product version ID to be deleted.</param>
        public void DeleteVersion(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Product version ID must be greater than zero.", "id");
            }

            Current.DB.ProductVersions.Delete(id);
        }
    }
}