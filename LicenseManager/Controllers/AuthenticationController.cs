using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using LicenseManager.Authentication;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Infrastructure.Extensions;
using LicenseManager.Models;
using LicenseManager.Models.ViewModels;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Controllers
{
    public class AuthenticationController : AppController
    {
        #region IoC

        public AuthenticationService AuthService { get; protected set; }
        public IUserService UserService { get; protected set; }
        public IMessageService MessageService { get; protected set; }

        // For sub-classes to initialize services themselves
        protected AuthenticationController()
        {
        }

        public AuthenticationController(
            AuthenticationService authService,
            IUserService userService,
            IMessageService messageService)
        {
            AuthService = authService;
            UserService = userService;
            MessageService = messageService;
        }

        #endregion

        /// <summary>
        /// Sign In/Register view
        /// </summary>
        [RequireSsl]
        [IntraRoute("account/logon")]
        public virtual ActionResult LogOn(string returnUrl)
        {
            // I think it should be obvious why we don't want the current URL to be the return URL here ;)
            ViewData[Constants.ReturnUrlViewDataKey] = returnUrl;

            if (Request.IsAuthenticated)
            {
                TempData["Message"] = Strings.AlreadyLoggedIn;
                return SafeRedirect(returnUrl);
            }

            return LogOnView();
        }

        [RequireSsl]
        [ValidateAntiForgeryToken]
        [IntraRoute("account/signin"), AcceptVerbs(HttpVerbs.Post)]
        public virtual async Task<ActionResult> SignIn(LogOnViewModel model, string returnUrl, bool linkingAccount)
        {
            // I think it should be obvious why we don't want the current URL to be the return URL here ;)
            ViewData[Constants.ReturnUrlViewDataKey] = returnUrl;

            if (Request.IsAuthenticated)
            {
                TempData["Message"] = Strings.AlreadyLoggedIn;
                return SafeRedirect(returnUrl);
            }

            if (!ModelState.IsValid)
            {
                return LogOnView(model);
            }

            var user = await AuthService.Authenticate(model.SignIn.UserNameOrEmail, model.SignIn.Password);

            if (user == null)
            {
                ModelState.AddModelError(
                    "SignIn",
                    Strings.UsernameAndPasswordNotFound);

                return LogOnView(model);
            }

            if (linkingAccount)
            {
                // Link with an external account
                user = await AssociateCredential(user, returnUrl);
                if (user == null)
                {
                    return ExternalLinkExpired();
                }
            }

            // Now log in!
            AuthService.CreateSession(OwinContext, user.User);
            return SafeRedirect(returnUrl);
        }

        [IntraRoute("account/logoff")]
        public virtual ActionResult LogOff(string returnUrl)
        {
            OwinContext.Authentication.SignOut();
            return SafeRedirect(returnUrl);
        }

        [Route("users/account/authenticate/{provider}")]
        public virtual ActionResult Authenticate(string returnUrl, string provider)
        {
            return AuthService.Challenge(
                provider,
                Url.Action("LinkExternalAccount", "Authentication", new { ReturnUrl = returnUrl }));
        }

        [Route("users/account/authenticate/return")]
        public async virtual Task<ActionResult> LinkExternalAccount(string returnUrl)
        {
            // Extract the external login info
            var result = await AuthService.AuthenticateExternalLogin(OwinContext);
            if (result.ExternalIdentity == null)
            {
                // User got here without an external login cookie (or an expired one)
                // Send them to the logon action
                return ExternalLinkExpired();
            }

            if (result.Authentication != null)
            {
                AuthService.CreateSession(OwinContext, result.Authentication.User);
                return SafeRedirect(returnUrl);
            }
            else
            {
                // Gather data for view model
                var authUI = result.Authenticator.GetUI();
                var email = result.ExternalIdentity.GetClaimOrDefault(ClaimTypes.Email);
                var name = result
                    .ExternalIdentity
                    .GetClaimOrDefault(ClaimTypes.Name);

                // Check for a user with this email address
                User existingUser = null;
                if (!String.IsNullOrEmpty(email))
                {
                    existingUser = UserService.GetUser(email);
                }

                var external = new AssociateExternalAccountViewModel()
                {
                    ProviderAccountNoun = authUI.AccountNoun,
                    AccountName = name,
                    FoundExistingUser = existingUser != null
                };

                var model = new LogOnViewModel()
                {
                    External = external,
                    SignIn = new SignInViewModel()
                    {
                        UserNameOrEmail = email
                    },
                    Register = new RegisterViewModel()
                    {
                        EmailAddress = email
                    }
                };

                return LogOnView(model);
            }
        }

        private async Task<AuthenticatedUser> AssociateCredential(AuthenticatedUser user, string returnUrl)
        {
            var result = await AuthService.ReadExternalLoginCredential(OwinContext);
            if (result.ExternalIdentity == null)
            {
                // User got here without an external login cookie (or an expired one)
                // Send them to the logon action
                return null;
            }

            await AuthService.AddCredential(user.User, result.Credential);

            // Notify the user of the change
            MessageService.SendCredentialAddedNotice(user.User, result.Credential);

            return new AuthenticatedUser(user.User, result.Credential);
        }

        private List<AuthenticationProviderViewModel> GetProviders()
        {
            return (from p in AuthService.Authenticators.Values
                    where p.BaseConfig.Enabled
                    let ui = p.GetUI()
                    where ui != null
                    select new AuthenticationProviderViewModel()
                    {
                        ProviderName = p.Name,
                        UI = ui
                    }).ToList();
        }

        private ActionResult ExternalLinkExpired()
        {
            // User got here without an external login cookie (or an expired one)
            // Send them to the logon action with a message
            TempData["Message"] = Strings.ExternalAccountLinkExpired;
            return RedirectToAction("LogOn");
        }

        private ActionResult LogOnView()
        {
            return LogOnView(new LogOnViewModel
            {
                SignIn = new SignInViewModel(),
                Register = new RegisterViewModel()
            });
        }

        private ActionResult LogOnView(LogOnViewModel existingModel)
        {
            // Fill the providers list
            existingModel.Providers = GetProviders();

            // Reinitialize any nulled-out sub models
            existingModel.SignIn = existingModel.SignIn ?? new SignInViewModel();
            existingModel.Register = existingModel.Register ?? new RegisterViewModel();

            return View("LogOn", existingModel);
        }
    }
}