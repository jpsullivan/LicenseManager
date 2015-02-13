using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace LicenseManager.Infrastructure.Extensions
{
    public static class UrlExtensions
    {
        // Shorthand for current url
        public static string Current(this UrlHelper url)
        {
            return url.RequestContext.HttpContext.Request.RawUrl;
        }

        public static string Absolute(this UrlHelper url, string path)
        {
            UriBuilder builder = GetCanonicalUrl(url);
            builder.Path = path;
            return builder.Uri.AbsoluteUri;
        }

        #region Base Pages

        public static string Home(this UrlHelper url)
        {
            return url.RouteUrl(RouteNames.Dashboard);
        }

        public static string Account(this UrlHelper url)
        {
            return url.Action("Account", "Users");
        }

        public static string Admin(this UrlHelper url)
        {
            return url.Action("Index", "Home");
        }

        #endregion

        #region Authentication

        public static string LogOn(this UrlHelper url)
        {
            return url.Action("LogOn", "Authentication");
        }

        public static string LogOn(this UrlHelper url, string returnUrl)
        {
            return url.Action("LogOn", "Authentication", new { returnUrl = returnUrl });
        }

        public static string LogOff(this UrlHelper url)
        {
            string returnUrl = url.Current();
            // If we're logging off from the Admin Area, don't set a return url
            if (String.Equals(url.RequestContext.RouteData.DataTokens["area"].ToStringOrNull(), "Admin", StringComparison.OrdinalIgnoreCase))
            {
                returnUrl = String.Empty;
            }
            return url.Action("LogOff", "Authentication", new { returnUrl });
        }

        #endregion

        #region Customers

        public static string Customers(this UrlHelper url)
        {
            return url.RouteUrl(RouteNames.Customers);
        }

        public static string NewCustomer(this UrlHelper url)
        {
            return url.RouteUrl(RouteNames.CustomerNew);
        }

        #endregion

        #region Licenses

        public static string NewLicense(this UrlHelper url)
        {
            return url.RouteUrl(RouteNames.LicenseNew);
        }

        #endregion

        #region Products

        public static string ShowProduct(this UrlHelper url, int productId, string productName)
        {
            return url.RouteUrl(RouteNames.ProductShow, new { id = productId, name = productName });
        }

        #endregion

        #region Users

        public static string UserAccount(this UrlHelper url)
        {
            return url.RouteUrl("User-Account");
        }

        public static string ConfirmationUrl(this UrlHelper url, string action, string controller, string username, string token, object routeValues = null)
        {
            var rvd = routeValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routeValues);
            rvd["username"] = username;
            rvd["token"] = token;
            return url.Action(
                action,
                controller,
                rvd,
                url.RequestContext.HttpContext.Request.Url.Scheme,
                url.RequestContext.HttpContext.Request.Url.Host);
        }

        #endregion

        #region Admin Pages

        public static string AdminUsers(this UrlHelper url)
        {
            return url.Action("Index", "Users");
        }

        public static string AdminNewUser(this UrlHelper url)
        {
            return url.Action("New", "Users");
        }

        public static string AdminCustomers(this UrlHelper url)
        {
            return url.Action("Index", "Customers");
        }

        public static string AdminProducts(this UrlHelper url)
        {
            return url.Action("Index", "Products");
        }

        public static string AdminNewProduct(this UrlHelper url)
        {
            return url.Action("New", "Products");
        }

        #endregion

        private static UriBuilder GetCanonicalUrl(UrlHelper url)
        {
            UriBuilder builder = new UriBuilder(url.RequestContext.HttpContext.Request.Url);
            builder.Query = String.Empty;
            if (builder.Host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                builder.Host = builder.Host.Substring(4);
            }
            return builder;
        }

        public static string EnsureTrailingSlash(this string urlPart)
        {
            return !urlPart.EndsWith("/", StringComparison.Ordinal) ? urlPart + '/' : urlPart;
        }
    }
}