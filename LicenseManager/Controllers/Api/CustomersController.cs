using System.Net;
using System.Web.Http;
using LicenseManager.Models;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Controllers.Api
{
    public class CustomersController : ApiController
    {
        #region DI

        public ICustomerService CustomerService { get; protected set; }

        public CustomersController(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        #endregion

        public Customer Post(Customer customer)
        {
            if (customer == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return CustomerService.CreateCustomer(customer);
        }

        public Customer Put(Customer customer)
        {
            if (customer == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return CustomerService.UpdateCustomer(customer);
        }

        public void Delete(int id)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            CustomerService.DeleteCustomer(id);
        }
    }
}
