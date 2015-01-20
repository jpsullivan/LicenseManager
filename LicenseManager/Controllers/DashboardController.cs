using System.Web.Mvc;
using LicenseManager.Infrastructure.Attributes;

namespace LicenseManager.Controllers
{
    public class DashboardController : AppController
    {
        [IntraRoute("", Name = RouteNames.Dashboard)]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}