using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Akron.Data.DataStructures;
using MongoDB.Bson.Serialization;

namespace Akron.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            BsonClassMap.RegisterClassMap<DataCollectionMetadata>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.CollectionName);

            });
        }
    }
}
