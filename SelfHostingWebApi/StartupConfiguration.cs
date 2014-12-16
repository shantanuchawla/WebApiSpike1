using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Formatting;
using WebApiContrib.Formatting;
using WebApiSpike1.Controllers;

namespace SelfHostingWebApi
{
   public  class StartupConfiguration
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.

       Type ProductsControllerType = typeof(ProductsController);

        public void Configuration(IAppBuilder appBuilder) 
        { 
            // Configure Web API for self-host. 
            
            HttpConfiguration config = new HttpConfiguration();
            config.Formatters.Add(new BsonMediaTypeFormatter());
            config.Formatters.Add(new ProtoBufFormatter());
            ProtoBufFormatter.Model.UseImplicitZeroDefaults = true;
            config.Routes.MapHttpRoute( 
                name: "DefaultApi", 
                routeTemplate: "api/{controller}/{id}", 
                defaults: new { id = RouteParameter.Optional } 
            ); 

            appBuilder.UseWebApi(config); 
        } 
    }
}
