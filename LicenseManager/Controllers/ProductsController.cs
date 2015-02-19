using System;
using System.Linq;
using System.Web.Mvc;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Infrastructure.Elmah;
using LicenseManager.Infrastructure.Extensions;
using LicenseManager.Models;
using LicenseManager.Models.ViewModels;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Controllers
{
    public class ProductsController : AppController
    {
        #region DI

        public IProductService ProductService { get; protected set; }
        public IProductVersionService ProductVersionService { get; protected set; }

        public ProductsController(IProductService productService, IProductVersionService productVersionService)
        {
            ProductService = productService;
            ProductVersionService = productVersionService;
        }

        #endregion

        [IntraRoute("product/{id:INT}/{name}", Name = RouteNames.ProductShow)]
        public ActionResult Show(int id, string name)
        {
            var product = ProductService.GetProduct(id);
            var versions = ProductVersionService.GetVersions(product.Id);
            var model = new ProductDetailsViewModel(product, versions);
            return View(model);
        }

        [IntraRoute("product/{id:INT}/{name}/customer-forecasts", Name = RouteNames.ProductCustomerForecasts)]
        public ActionResult CustomerForecasts(int id, string name)
        {
            return View();
        }

        [IntraRoute("product/{id:INT}/{name}/versions", Name = RouteNames.ProductVersions)]
        public ActionResult Versions(int id, string name)
        {
            var product = ProductService.GetProduct(id);
            var versions = ProductVersionService.GetVersions(product.Id);
            var model = new ProductLicensesViewModel(product, versions);
            return View(model);
        }

        [IntraRoute("product/{id:INT}/{name}/versions/new", Name = RouteNames.NewProductVersion)]
        public ActionResult NewVersion(int id, string name)
        {
            return View();
        }

        [IntraRoute("product/{id:INT}/{name}/version/create", HttpVerbs.Post, Name = RouteNames.CreateProductVersion)]
        public ActionResult CreateVersion(int id, string name, NewProductVersionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewVersion", model);
            }
            
            var product = ProductService.GetProduct(id);
            var versions = ProductVersionService.GetVersions(product.Id);
            if (versions.Any(v => v.Version == model.Version))
            {
                ModelState.AddModelError("Name", "This product alraedy has a version with this name.");
                return View("NewVersion", model);
            }

            try
            {
                var version = new ProductVersion(product.Id, model.Version);
                ProductVersionService.CreateVersion(version);
            }
            catch (Exception ex)
            {
                QuietLog.LogHandledException(ex);
            }

            return SafeRedirect(Url.ShowProduct(id, name.UrlFriendly()));
        }
    }
}