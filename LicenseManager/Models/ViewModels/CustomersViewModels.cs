using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LicenseManager.Models.ViewModels
{
    public class CustomersViewModels
    {
    }

    public class CustomerListModel
    {
        public IEnumerable<Customer> Customers { get; set; }

        public CustomerListModel(IEnumerable<Customer> customers)
        {
            Customers = customers;
        }
    }

    public class NewCustomerModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Billing Contact")]
        public string BillingContact { get; set; }

        [Required]
        [Display(Name = "Billing Contact Email")]
        public string BillingContactEmail { get; set; }

        [Required]
        [Display(Name = "Technical Contact")]
        public string TechnicalContact { get; set; }

        [Required]
        [Display(Name = "Technical Contact Email")]
        public string TechnicalContactEmail { get; set; }

        [Required]
        [Display(Name = "Hosted?")]
        public bool IsHosted { get; set; }
    }
}