using System;

namespace LicenseManager.Models
{
    public class Product
    {
        public int Id { get; set; }

        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Some information about this product.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The public URL of the product.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The username of the user who created this product.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// When this product was created
        /// </summary>
        public DateTime? CreatedUtc { get; set; }

        /// <summary>
        /// When this product was last updated.
        /// </summary>
        public DateTime? LastUpdatedUtc { get; set; }
    }
}