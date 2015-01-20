using System.Web.Mvc;
using LicenseManager.Infrastructure.Attributes;

namespace LicenseManager.Areas.Admin.Controllers
{
    public class HomeController : AdminControllerBase
    {
        [IntraRoute("admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}