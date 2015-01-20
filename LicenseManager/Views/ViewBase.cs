using System;
using System.Web.Mvc;
using LicenseManager.Controllers;
using LicenseManager.Models;
using LicenseManager.Services;

namespace LicenseManager.Views
{
    public abstract class ViewBase : WebViewPage
    {
        private readonly Lazy<LicenseManagerContext> _context;

        public LicenseManagerContext LicenseManagerContext
        {
            get { return _context.Value; }
        }

        public ConfigurationService Config
        {
            get { return LicenseManagerContext.Config; }
        }

        public User CurrentUser
        {
            get { return LicenseManagerContext.CurrentUser; }
        }

        protected ViewBase()
        {
            _context = new Lazy<LicenseManagerContext>(GetContextThunk(this));
        }

        internal static Func<LicenseManagerContext> GetContextThunk(WebViewPage self)
        {
            return () =>
            {
                var ctrl = self.ViewContext.Controller as AppController;
                if (ctrl == null)
                {
                    throw new InvalidOperationException("Viewbase should only be used on views for actions on AppControllers");
                }
                return ctrl.LicenseManagerContext;
            };
        }
    }

    public abstract class ViewBase<T> : WebViewPage<T>
    {
        private Lazy<LicenseManagerContext> _context;

        public LicenseManagerContext LicenseManagerContext
        {
            get { return _context.Value; }
        }

        public ConfigurationService Config
        {
            get { return LicenseManagerContext.Config; }
        }

        public User CurrentUser
        {
            get { return LicenseManagerContext.CurrentUser; }
        }

        protected ViewBase()
        {
            _context = new Lazy<LicenseManagerContext>(ViewBase.GetContextThunk(this));
        }
    }
}