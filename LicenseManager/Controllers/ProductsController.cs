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

        public ProductsController(IProductService productService)
        {
            ProductService = productService;
        }

        #endregion

        [IntraRoute("product/{id:INT}/{name}", Name = RouteNames.ProductShow)]
        public ActionResult Show(int id, string name)
        {
            var product = ProductService.GetProduct(id);
            var model = new ProductDetailsViewModel(product);
            return View(model);
        }

        [IntraRoute("product/{id:INT}/{name}/customer-forecasts", Name = RouteNames.ProductCustomerForecasts)]
        public ActionResult CustomerForecasts(int id, string name)
        {
            return View();
        }
    }
}