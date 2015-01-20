using System.Collections.Generic;
using LicenseManager.Models;

namespace LicenseManager.Services.Interfaces
{
    public interface ICustomerService
    {
        /// <summary>
        /// Gets a specific customer.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Customer"/> to fetch.</param>
        /// <returns>A single <see cref="Customer"/>.</returns>
        Customer GetCustomer(int id);

        /// <summary>
        /// Gets a specific customer by their company name.
        /// </summary>
        /// <param name="name">The company name.</param>
        /// <returns>A single <see cref="Customer"/>.</returns>
        Customer GetCustomer(string name);

        /// <summary>
        /// Returns all of the customers.
        /// </summary>
        /// <returns>A list of <see cref="Customer"/> objects.</returns>
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        /// Creates a new customer based on the provided <see cref="Customer"/> object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Customer CreateCustomer(Customer customer);

        /// <summary>
        /// Updated an existing customer based on the provided <see cref="Customer"/> object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Customer UpdateCustomer(Customer customer);

        /// <summary>
        /// Removes a customer based on its ID.
        /// </summary>
        /// <param name="id">The ID of the customer to remove.</param>
        void DeleteCustomer(int id);
    }
}