using System;
using System.Collections.Generic;
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

    [Serializable] // step 2
    public class ProductNameSelectionViewModel : INewLicenseViewModel
    {
        public IEnumerable<Product> Products { get; private set; }

        public ProductNameSelectionViewModel()
        {
        }

        public ProductNameSelectionViewModel(IProductService productService)
        {
            Products = productService.GetProducts();
        }
            
        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
    }

    [Serializable] // step 3
    public class ProductVersionSelectionViewModel : INewLicenseViewModel
    {
        public IEnumerable<ProductVersion> Versions { get; private set; }

        public ProductVersionSelectionViewModel()
        {
        }

        public ProductVersionSelectionViewModel(IProductVersionService productVersionService, ICacheService cacheService)
        {
//            int productId = Convert.ToInt32(cacheService.GetItem("SelectedProductId"));
//            if (productId > 0)
//            {
//                Versions = productVersionService.GetVersions(productId);
//            }

            Versions = productVersionService.GetVersions(5);
        }

        [Required]
        [Display(Name = "Version")]
        public int VersionId { get; set; }
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