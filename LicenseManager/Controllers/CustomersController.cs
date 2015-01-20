using System;
using System.Web.Mvc;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Infrastructure.Elmah;
using LicenseManager.Infrastructure.Extensions;
using LicenseManager.Models;
using LicenseManager.Models.ViewModels;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Controllers
{
    public class CustomersController : AppController
    {
        #region DI

        public ICustomerService CustomerService { get; protected set; }

        public CustomersController(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        #endregion

        [IntraRoute("customers", Name = RouteNames.Customers)]
        public ActionResult Index()
        {
            var customers = CustomerService.GetCustomers();
            var model = new CustomerListModel(customers);
            return View(model);
        }

        [IntraRoute("customer/new", Name = RouteNames.CustomerNew)]
        public ActionResult New()
        {
            return View();
        }

        [IntraRoute("customer/create", HttpVerbs.Post)]
        public ActionResult CreateCustomer(NewCustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            var foundCustomer = CustomerService.GetCustomer(model.Name);
            if (foundCustomer != null)
            {
                ModelState.AddModelError("Name", "A customer with this company name is already registered.");
                return View("New", model);
            }

            try
            {
                var customer = new Customer
                {
                    Name = model.Name,
                    BillingContact = model.BillingContact,
                    BillingContactEmail = model.BillingContactEmail,
                    TechnicalContact = model.TechnicalContact,
                    TechnicalContactEmail = model.TechnicalContactEmail,
                    IsHosted = model.IsHosted
                };

                CustomerService.CreateCustomer(customer);
            }
            catch (Exception ex)
            {
                QuietLog.LogHandledException(ex);
            }

            return SafeRedirect(Url.AdminCustomers());
        }
    }
}