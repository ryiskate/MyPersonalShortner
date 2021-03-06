﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MyPersonalShortner.Lib.Services;
using MyPersonalShortner.MvcApp.IoC;
using MyPersonalShortner.Lib.Domain.Repositories;
using MyPersonalShortner.Lib.Infrastructure.EntityFramework.Repositories;
using MyPersonalShortner.Lib.Domain.UrlConversion;

namespace MyPersonalShortner.MvcApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Home", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("Shortner", "{hash}", new { controller = "Shortner", action = "Index", hash = UrlParameter.Optional });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Shortner", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        // ReSharper disable InconsistentNaming
        protected void Application_Start()
        // ReSharper restore InconsistentNaming
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DependencyResolver.SetResolver(new UnityDependencyResolver(GetUnityContainer()));
        }

        private static string CharsForHash
        {
            get { return "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
        }

        private static IUnityContainer GetUnityContainer()
        {
            //Create UnityContainer          
            var container = new UnityContainer()
            .RegisterType<IControllerActivator, CustomControllerActivator>()
            .RegisterType<IShortnerService, ShortnerService>(new HttpContextLifetimeManager<IShortnerService>())
            .RegisterType<ILongUrlRepository, LongUrlRepository>(new HttpContextLifetimeManager<ILongUrlRepository>())
            .RegisterType<ICustomUrlRepository, CustomUrlRepository>(new HttpContextLifetimeManager<ICustomUrlRepository>())
            .RegisterType<IUrlConversion, Base10ToHash>(new HttpContextLifetimeManager<IUrlConversion>(), new InjectionConstructor(CharsForHash));

            return container;
        }
    }
}