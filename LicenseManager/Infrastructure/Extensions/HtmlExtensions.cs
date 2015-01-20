using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace LicenseManager.Infrastructure.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString ValidationSummaryFor(this HtmlHelper html, string key)
        {
            var toRemove = html.ViewData.ModelState.Keys
                .Where(k => !String.Equals(k, key, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var copy = new ModelStateDictionary(html.ViewData.ModelState);
            foreach (var k in toRemove)
            {
                html.ViewData.ModelState.Remove(k);
            }
            var str = html.ValidationSummary();

            // Restore the old model state
            foreach (var k in toRemove)
            {
                html.ViewData.ModelState[k] = copy[k];
            }

            return str;
        }
    }
}