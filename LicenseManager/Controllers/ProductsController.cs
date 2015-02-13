using System.Web.Mvc;
using LicenseManager.Infrastructure.Attributes;
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

        [IntraRoute("product/{id:INT}/{name}/versions", Name = RouteNames.ProduceLicenses)]
        public ActionResult Versions(int id, string name)
        {
            var product = ProductService.GetProduct(id);
            var versions = ProductVersionService.GetVersions(product.Id);
            var model = new ProductLicensesViewModel(product, versions);
            return View(model);
        }
    }
}