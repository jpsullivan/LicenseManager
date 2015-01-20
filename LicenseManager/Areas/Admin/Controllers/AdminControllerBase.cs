using System.Web.Http;
using LicenseManager.Controllers;

namespace LicenseManager.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminControllerBase : AppController
    {
    }
}