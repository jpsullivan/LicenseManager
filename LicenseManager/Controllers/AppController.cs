using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LicenseManager.Infrastructure;
using LicenseManager.Infrastructure.Extensions;
using LicenseManager.Models;
using LicenseManager.Services;
using LicenseManager.Services.Interfaces;
using Microsoft.Owin;
using Ninject;
using StackExchange.Profiling;

namespace LicenseManager.Controllers
{
    public abstract class AppController : Controller
    {
        private IUserService UserService { get; set; }
        private IOwinContext _overrideContext;

        protected AppController()
        {
            UserService = Container.Kernel.TryGet<UserService>();
            LicenseManagerContext = new LicenseManagerContext(this);
        }

        private readonly Current _current = new Current();

        private IDisposable _betweenInitializeAndActionExecuting,
                            _betweenActionExecutingAndExecuted,
                            _betweenActionExecutedAndResultExecuting,
                            _betweenResultExecutingAndExecuted;

        private readonly Func<string, IDisposable> _startStep = name => MiniProfiler.Current.Step(name);
        private readonly Action<IDisposable> _stopStep = s => { if (s != null) s.Dispose(); };

#if DEBUG
        private Stopwatch _watch;
#endif

        protected override void Initialize(RequestContext requestContext)
        {
            _betweenInitializeAndActionExecuting = _startStep("Initialize");

#if DEBUG
            _watch = new Stopwatch();
            _watch.Start();
#endif

            _current.Controller = this; // allow code to easily find this controller
            base.Initialize(requestContext);
        }

        public IOwinContext OwinContext
        {
            get { return _overrideContext ?? HttpContext.GetOwinContext(); }
            set { _overrideContext = value; }
        }

        public LicenseManagerContext LicenseManagerContext { get; private set; }

        public new ClaimsPrincipal User
        {
            get { return base.User as ClaimsPrincipal; }
        }

        protected internal virtual T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }

        protected internal User GetCurrentUser()
        {
            return OwinContext.GetCurrentUser();
        }

        /// <summary>
        /// When a client IP can't be determined
        /// </summary>
        public const string UnknownIP = "0.0.0.0";

        private static readonly Regex IPAddress = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}$",
                                                            RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Returns true if this is a private network IP
        /// http://en.wikipedia.org/wiki/Private_network
        /// </summary>
        private static bool IsPrivateIP(string s)
        {
            return (s.StartsWith("192.168.") || s.StartsWith("10.") || s.StartsWith("127.0.0."));
        }

        /// <summary>
        /// Retrieves the IP address of the current request -- handles proxies and private networks
        /// </summary>
        public static string GetRemoteIP(NameValueCollection serverVariables)
        {
            string ip = serverVariables["REMOTE_ADDR"]; // could be a proxy -- beware
            string ipForwarded = serverVariables["HTTP_X_FORWARDED_FOR"];

            // check if we were forwarded from a proxy
            if (String.IsNullOrEmpty(ipForwarded)) return String.IsNullOrEmpty(ip) ? ip : UnknownIP;

            ipForwarded = IPAddress.Match(ipForwarded).Value;
            if (String.IsNullOrEmpty(ipForwarded) && !IsPrivateIP(ipForwarded))
            {
                ip = ipForwarded;
            }

            return String.IsNullOrEmpty(ip) ? ip : UnknownIP;
        }

        /// <summary>
        /// Answers the current request's user's ip address; checks for any forwarding proxy
        /// </summary>
        public string GetRemoteIP()
        {
            return GetRemoteIP(Request.ServerVariables);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                _stopStep(_betweenInitializeAndActionExecuting);
                _betweenActionExecutingAndExecuted = _startStep("OnActionExecuting");
            }
            base.OnActionExecuting(filterContext);
        }

#if (DEBUG || DEBUGMINIFIED)
        /// <summary>
        /// Fires after the controller finishes execution
        /// </summary>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                _stopStep(_betweenActionExecutingAndExecuted);
                _betweenActionExecutedAndResultExecuting = _startStep("OnActionExecuted");
            }

            base.OnActionExecuted(filterContext);
        }
#endif

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                _stopStep(_betweenActionExecutedAndResultExecuting);
                _betweenResultExecutingAndExecuted = _startStep("OnResultExecuting");
            }
            base.OnResultExecuting(filterContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            _stopStep(_betweenResultExecutingAndExecuted);

            using (MiniProfiler.Current.Step("OnResultExecuted"))
            {
                base.OnResultExecuted(filterContext);
            }
        }

        /// <summary>
        /// Safely redirects to the specified return url.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        protected internal virtual ActionResult SafeRedirect(string returnUrl)
        {
            return new SafeRedirectResult(returnUrl, Url.Home());
        }
    }

    public class LicenseManagerContext
    {
        private Lazy<User> _currentUser;

        public ConfigurationService Config { get; private set; }
        public User CurrentUser { get { return _currentUser.Value; } }

        public LicenseManagerContext(AppController ctrl)
        {
            Config = Container.Kernel.TryGet<ConfigurationService>();
            _currentUser = new Lazy<User>(() => ctrl.OwinContext.GetCurrentUser());
        }
    }
}