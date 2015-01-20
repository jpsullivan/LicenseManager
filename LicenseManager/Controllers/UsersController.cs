using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LicenseManager.Authentication;
using LicenseManager.Configuration;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Infrastructure.Extensions;
using LicenseManager.Models;
using LicenseManager.Models.ViewModels;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Controllers
{
    public class UsersController : AppController
    {
        #region IoC

        public IUserService UserService { get; protected set; }
        public IMessageService MessageService { get; protected set; }
        public IAppConfiguration Config { get; protected set; }
        public AuthenticationService AuthService { get; protected set; }

        public UsersController(
            IUserService userService,
            IMessageService messageService,
            IAppConfiguration config,
            AuthenticationService authService)
        {
            UserService = userService;
            MessageService = messageService;
            Config = config;
            AuthService = authService;
        }

        #endregion

        [Authorize]
        [IntraRoute("account", Name = "User-Account")]
        public virtual ActionResult Account()
        {
            return AccountView(new AccountViewModel());
        }

        public virtual ActionResult ForgotPassword()
        {
            // We don't want Login to have us as a return URL
            // By having this value present in the dictionary BUT null, we don't put "returnUrl" on the Login link at all
            ViewData[Constants.ReturnUrlViewDataKey] = null;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // We don't want Login to have us as a return URL
            // By having this value present in the dictionary BUT null, we don't put "returnUrl" on the Login link at all
            ViewData[Constants.ReturnUrlViewDataKey] = null;

            if (ModelState.IsValid)
            {
                var user = await AuthService.GeneratePasswordResetToken(model.Email, Constants.DefaultPasswordResetTokenExpirationHours * 60);
                if (user != null)
                {
                    return SendPasswordResetEmail(user, forgotPassword: true);
                }

                ModelState.AddModelError("Email", "Could not find anyone with that email.");
            }

            return View(model);
        }

        public virtual ActionResult PasswordSent()
        {
            // We don't want Login to have us as a return URL
            // By having this value present in the dictionary BUT null, we don't put "returnUrl" on the Login link at all
            ViewData[Constants.ReturnUrlViewDataKey] = null;

            ViewBag.Email = TempData["Email"];
            ViewBag.Expiration = Constants.DefaultPasswordResetTokenExpirationHours;
            return View();
        }

        public virtual ActionResult ResetPassword(bool forgot)
        {
            // We don't want Login to have us as a return URL
            // By having this value present in the dictionary BUT null, we don't put "returnUrl" on the Login link at all
            ViewData[Constants.ReturnUrlViewDataKey] = null;

            ViewBag.ResetTokenValid = true;
            ViewBag.ForgotPassword = forgot;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IntraRoute("account/forgotpassword/{username}/{token}")]
        public virtual async Task<ActionResult> ResetPassword(string username, string token, PasswordResetViewModel model, bool forgot)
        {
            // We don't want Login to have us as a return URL
            // By having this value present in the dictionary BUT null, we don't put "returnUrl" on the Login link at all
            ViewData[Constants.ReturnUrlViewDataKey] = null;

            var cred = await AuthService.ResetPasswordWithToken(username, token, model.NewPassword);
            ViewBag.ResetTokenValid = cred != null;
            ViewBag.ForgotPassword = forgot;

            if (!ViewBag.ResetTokenValid)
            {
                ModelState.AddModelError("", "The Password Reset Token is not valid or expired.");
                return View(model);
            }

            if (cred != null && !forgot)
            {
                // Setting a password, so notify the user
                MessageService.SendCredentialAddedNotice(cred.User, cred);
            }

            return RedirectToAction("PasswordChanged");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangePassword(AccountViewModel model)
        {
            var user = GetCurrentUser();

            var oldPassword = user.Credentials.FirstOrDefault(
                c => c.Type.StartsWith(CredentialTypes.Password.Prefix, StringComparison.OrdinalIgnoreCase));

            if (oldPassword == null)
            {
                // User is requesting a password set email
                await AuthService.GeneratePasswordResetToken(user, Constants.DefaultPasswordResetTokenExpirationHours * 60);
                return SendPasswordResetEmail(user, forgotPassword: false);
            }
            else
            {
                if (!ModelState.IsValidField("ChangePassword"))
                {
                    return AccountView(model);
                }

                if (!(await AuthService.ChangePassword(user, model.ChangePassword.OldPassword, model.ChangePassword.NewPassword)))
                {
                    ModelState.AddModelError("ChangePassword.OldPassword", Strings.CurrentPasswordIncorrect);
                    return AccountView(model);
                }

                TempData["Message"] = Strings.PasswordChanged;

                return RedirectToAction("Account");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("account/RemoveCredential/password")]
        public virtual Task<ActionResult> RemovePassword()
        {
            var user = GetCurrentUser();
            var passwordCred = user.Credentials.SingleOrDefault(
                c => c.Type.StartsWith(CredentialTypes.Password.Prefix, StringComparison.OrdinalIgnoreCase));

            return RemoveCredential(user, passwordCred, Strings.PasswordRemoved);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("account/RemoveCredential/{credentialType}")]
        public virtual Task<ActionResult> RemoveCredential(string credentialType)
        {
            var user = GetCurrentUser();
            var cred = user.Credentials.SingleOrDefault(
                c => String.Equals(c.Type, credentialType, StringComparison.OrdinalIgnoreCase));

            return RemoveCredential(user, cred, Strings.CredentialRemoved);
        }

        public virtual ActionResult PasswordChanged()
        {
            return View();
        }

        private async Task<ActionResult> RemoveCredential(User user, Credential cred, string message)
        {
            // Count login credentials
            if (CountLoginCredentials(user) <= 1)
            {
                TempData["Message"] = Strings.CannotRemoveOnlyLoginCredential;
            }
            else if (cred != null)
            {
                await AuthService.RemoveCredential(user, cred);

                // Notify the user of the change
                MessageService.SendCredentialRemovedNotice(user, cred);

                TempData["Message"] = message;
            }
            return RedirectToAction("Account");
        }

        private ActionResult AccountView(AccountViewModel model)
        {
            // Load Credential info
            var user = GetCurrentUser();
            var creds = user.Credentials.Select(c => AuthService.DescribeCredential(c)).ToList();

            model.Credentials = creds;
            return View("Account", model);
        }

        private static int CountLoginCredentials(User user)
        {
            return user.Credentials.Count(c =>
                c.Type.StartsWith(CredentialTypes.Password.Prefix, StringComparison.OrdinalIgnoreCase) ||
                c.Type.StartsWith(CredentialTypes.ExternalPrefix, StringComparison.OrdinalIgnoreCase));
        }

        private ActionResult SendPasswordResetEmail(User user, bool forgotPassword)
        {
            var resetPasswordUrl = Url.ConfirmationUrl(
                "ResetPassword",
                "Users",
                user.Username,
                user.PasswordResetToken,
                new { forgot = forgotPassword });
            MessageService.SendPasswordResetInstructions(user, resetPasswordUrl, forgotPassword);

            TempData["Email"] = user.EmailAddress;
            return RedirectToAction("PasswordSent");
        }
    }
}