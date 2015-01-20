using System.Web.Mvc;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Models.ViewModels;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Areas.Admin.Controllers
{
    public class CustomersController : AdminControllerBase
    {
        #region DI

        public ICustomerService CustomerService { get; protected set; }

        public CustomersController(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        #endregion

        [IntraRoute("admin/customers", Name = "Admin-Customers")]
        public ActionResult Index()
        {
            var customers = CustomerService.GetCustomers();
            var model = new CustomerListModel(customers);
            return View(model);
        }
    }
}