﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseManager;
using LicenseManager.Controllers;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using StackExchange.Profiling.SqlFormatters;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MiniProfilerPackage), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(MiniProfilerPackage), "PostStart")]

namespace LicenseManager
{
    public static class MiniProfilerPackage
    {
        private class ProxySafeUserProvider : IUserProvider
        {
            public string GetUser(HttpRequest request)
            {
                return AppController.GetRemoteIP(request.ServerVariables);
            }
        }

        public static void PreStart()
        {
            WebRequestProfilerProvider.Settings.UserProvider = new ProxySafeUserProvider();

            MiniProfiler.Settings.SqlFormatter = new SqlServerFormatter();
            //MiniProfiler.Settings.Storage = new SqlServerStorage(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

            // Make sure the MiniProfiler handles BeginRequest and EndRequest
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));

            // Setup profiler for Controllers via a Global ActionFilter
            GlobalFilters.Filters.Add(new ProfilingActionFilter());

            //MiniProfiler.Settings.Results_List_Authorize = request => Current.User.IsAdministrator;
            MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right;
        }

        public static void PostStart()
        {
            // Intercept ViewEngines to profile all partial views and regular views.
            // If you prefer to insert your profiling blocks manually you can comment this out
            List<IViewEngine> copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (IViewEngine item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }
    }

    public class MiniProfilerStartupModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
#if (DEBUG || DEBUGMINIFIED)
            context.BeginRequest += (sender, e) => MiniProfiler.Start();
            context.EndRequest += (sender, e) => MiniProfiler.Stop();
#endif
        }

        public void Dispose() { }
    }
}