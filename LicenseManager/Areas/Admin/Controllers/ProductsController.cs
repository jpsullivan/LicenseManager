using System;
using System.Linq;
using System.Web.Mvc;
using LicenseManager.Areas.Admin.ViewModels;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Infrastructure.Elmah;
using LicenseManager.Infrastructure.Extensions;
using LicenseManager.Models;
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

        [IntraRoute("admin/product/new", Name = "Admin-Product-New")]
        public ActionResult New()
        {
            return View();
        }

        [IntraRoute("admin/product/create", HttpVerbs.Post)]
        public ActionResult CreateProduct(NewProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            var products = ProductService.GetProducts();
            if (products.Any(product => product.Name == model.Name))
            {
                ModelState.AddModelError("Name", "A product with this name was already found.");
                return View("New", model);
            }

            try
            {
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Url = model.Url,
                    CreatedBy = GetCurrentUser().Username
                };

                ProductService.CreateProduct(product);
            }
            catch (Exception ex)
            {
                QuietLog.LogHandledException(ex);
            }

            return SafeRedirect(Url.AdminProducts());
        }
    }
}