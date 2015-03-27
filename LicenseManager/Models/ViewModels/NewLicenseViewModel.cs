using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Models.ViewModels
{
    [Serializable]
    public class NewLicenseViewModel
    {
        public int CurrentStepIndex { get; set; }
        public IList<INewLicenseViewModel> Steps { get; set; }

        public NewLicenseViewModel(IList<INewLicenseViewModel> steps)
        {
            Steps = steps;
        }
    }

    [Serializable] // step 1
    public class CustomerSelectionViewModel : INewLicenseViewModel
    {
        public IEnumerable<Customer> Customers { get; protected set; }

        public CustomerSelectionViewModel()
        {
        }

        public CustomerSelectionViewModel(ICustomerService customerService)
        {
            Customers = customerService.GetCustomers();
        }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
    }

//    [Serializable] // step 2
//    public class ProductNameSelectionViewModel : INewLicenseViewModel
//    {
//        public IEnumerable<Product> Products { get; private set; }
//
//        public ProductNameSelectionViewModel()
//        {
//        }
//
//        public ProductNameSelectionViewModel(IProductService productService)
//        {
//            Products = productService.GetProducts();
//        }
//            
//        [Required]
//        [Display(Name = "Product")]
//        public int ProductId { get; set; }
//    }

    [Serializable] // step 2
    public class FeatureSelectionViewModel : INewLicenseViewModel
    {
        public FeatureSelectionViewModel()
        {
            // populate the form with these default values
            MaxProfiles = 1;
            MaxDesktopClients = 1;
            MaxMobileClients = 1;
            MaxWebClients = 1;
            MaxRequestForms = 3;
        }

        [Required]
        [Display(Name = "Max Desktop Clients")]
        public int MaxProfiles { get; set; }

        [Required]
        [Display(Name = "Max Desktop Clients")]
        public int MaxDesktopClients { get; set; }

        [Required]
        [Display(Name = "Max Mobile Clients")]
        public int MaxMobileClients { get; set; }

        [Required]
        [Display(Name = "Max Web Clients")]
        public int MaxWebClients { get; set; }

        [Required]
        [Display(Name = "Max Request Forms")]
        public int MaxRequestForms { get; set; }
    }

    [Serializable] // step 3
    public class LicenseExpirationViewModel : INewLicenseViewModel
    {
        public LicenseExpirationViewModel()
        {
        }

        public DateTime ExpirationDate { get; set; }
    }

    [Serializable]
    public class LicenseDetailsViewModel : INewLicenseViewModel
    {
    }

    // marker interface
    public interface INewLicenseViewModel
    {
    }
}