using System;
using System.Collections.Generic;
using System.Linq;
using LicenseManager.Models;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Services
{
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// Gets a specific customer.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Customer"/> to fetch.</param>
        /// <returns>A single <see cref="Customer"/>.</returns>
        public Customer GetCustomer(int id)
        {
            return Current.DB.Customers.Get(id);
        }

        /// <summary>
        /// Gets a specific customer by their company name.
        /// </summary>
        /// <param name="name">The company name.</param>
        /// <returns>A single <see cref="Customer"/>.</returns>
        public Customer GetCustomer(string name)
        {
            const string sql = "select * from Customers where Name = @name";
            return Current.DB.Query<Customer>(sql, new {name}).FirstOrDefault();
        }

        /// <summary>
        /// Returns all of the customers.
        /// </summary>
        /// <returns>A list of <see cref="Customer"/> objects.</returns>
        public IEnumerable<Customer> GetCustomers()
        {
            return Current.DB.Customers.All();
        }

        /// <summary>
        /// Creates a new customer based on the provided <see cref="Customer"/> object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public Customer CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            var success = Current.DB.Customers.Insert(new
            {
                customer.Name,
                customer.BillingContact,
                customer.BillingContactEmail,
                customer.TechnicalContact,
                customer.TechnicalContactEmail,
                customer.IsHosted,
                CreatedUtc = DateTime.UtcNow
            });

            if (success == null)
            {
                // insert failed. todo: throw message here
                return customer;
            }

            customer.Id = success.Value;

            return customer;
        }

        /// <summary>
        /// Updated an existing customer based on the provided <see cref="Customer"/> object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public Customer UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a customer based on its ID.
        /// </summary>
        /// <param name="id">The ID of the customer to remove.</param>
        public void DeleteCustomer(int id)
        {
            Current.DB.Customers.Delete(id);
        }
    }
}