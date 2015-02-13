using LicenseManager.Models;

namespace LicenseManager.Infrastructure.DB
{
    public class LicenseManagerDatabase : Dapper.Database<LicenseManagerDatabase>
    {
        public Table<Customer> Customers { get; private set; } 
        public Table<Credential> Credentials { get; private set; }
        public Table<Product> Products { get; private set; }
        public Table<ProductVersion> ProductVersions { get; set; } 
        public Table<User> Users { get; private set; }
    }
}