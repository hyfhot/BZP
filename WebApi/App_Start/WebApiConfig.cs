using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApi.Lib;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var jsonFormatter = new JsonMediaTypeFormatter();
            //optional: set serializer settings here
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "SecurityApi",
                routeTemplate: "api/Security/{action}",
                defaults: new { controller = "Security" }
            );

            config.Routes.MapHttpRoute(
                name: "HelperApi",
                routeTemplate: "api/Helper/{action}",
                defaults: new { controller = "Helper" }
            );

            config.Routes.MapHttpRoute(
                name: "InitApi",
                routeTemplate: "api/Init/{action}",
                defaults: new { controller = "Init" }
            );

            //config.Routes.MapHttpRoute(
            //    name: "ActionApi",
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
