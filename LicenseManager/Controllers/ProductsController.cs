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

        public ProductsController(IProductService customerService)
        {
            ProductService = customerService;
        }

        #endregion

        [IntraRoute("product/{id:INT}/{name}", Name = RouteNames.ProductShow)]
        public ActionResult Show(int id, string name)
        {
            var product = ProductService.GetProduct(id);
            var model = new ProductDetailsViewModel(product);
            return View(model);
        }
    }
}