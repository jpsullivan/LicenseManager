using System.Web.Mvc;
using LicenseManager.Areas.Admin.ViewModels;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Areas.Admin.Controllers
{
    public class ProductsController : AdminControllerBase
    {
        #region DI

        public IProductService ProductService { get; protected set; }

        public ProductsController(IProductService customerService)
        {
            ProductService = customerService;
        }

        #endregion

        [IntraRoute("admin/products", Name = "Admin-Products")]
        public ActionResult Index()
        {
            var products = ProductService.GetProducts();
            var model = new ProductListModel(products);
            return View(model);
        }
    }
}