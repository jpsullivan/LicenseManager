using System.Threading.Tasks;
using System.Web.Mvc;
using LicenseManager.Areas.Admin.ViewModels;
using LicenseManager.Authentication;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Infrastructure.Exceptions;
using LicenseManager.Infrastructure.Extensions;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Areas.Admin.Controllers
{
    public class UsersController : AdminControllerBase
    {
        #region DI

        public AuthenticationService AuthService { get; protected set; }
        public IUserService UserService { get; protected set; }

        public UsersController(AuthenticationService authService, IUserService userService)
        {
            AuthService = authService;
            UserService = userService;
        }

        #endregion

        [IntraRoute("admin/users", Name = "Admin-Users")]
        public ActionResult Index()
        {
            var users = UserService.GetUsers();
            var model = new UserListModel(users);
            return View(model);
        }

        [IntraRoute("admin/user/new", Name = "Admin-User-New")]
        public ActionResult New()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [IntraRoute("admin/user/create", HttpVerbs.Post)]
        public async Task<ActionResult> CreateUser(NewUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            var foundUser = UserService.GetUser(model.Username, model.EmailAddress);
            if (foundUser != null)
            {
                ModelState.AddModelError("Username", "A user with this username or email address is already registered.");
                return View("New", model);
            }

            if (model.Password != model.PasswordConfirmation)
            {
                ModelState.AddModelError("Password", "Passwords did not match.");
                return View("New", model);
            }

            AuthenticatedUser user;
            try
            {
                user = await AuthService.Register(model.Username, model.EmailAddress, model.FullName, CredentialBuilder.CreatePbkdf2Password(model.Password));
            }
            catch (EntityException ex)
            {
                ModelState.AddModelError("Register", ex.Message);
                return View("New", model);
            }

            return SafeRedirect(Url.AdminUsers());
        }
    }
}