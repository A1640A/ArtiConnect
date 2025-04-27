using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(ArtiConnect.Api.Startup))]
namespace ArtiConnect.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll); 

            // Web API konfigürasyonu
            var config = new HttpConfiguration();

            // Routing
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Swagger konfigürasyonu
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "ArtiConnect API");
                c.DescribeAllEnumsAsStrings();
                c.IncludeXmlComments(string.Format(@"{0}\ArtiConnect.XML",
                    System.AppDomain.CurrentDomain.BaseDirectory));
            })
            .EnableSwaggerUi(c =>
            {
                c.DocumentTitle("ArtiConnect API");
            });

            // JSON formatını ayarla
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            config.Filters.Add(new ApiLoggerAttribute());
            config.MessageHandlers.Add(new BufferPolicySelector());

            // API Logger middleware'ini ekle - Web API'den ÖNCE ekleyin
            //app.Use(typeof(ApiLoggerMiddleware));

            // OWIN middleware'leri
            app.UseWebApi(config);

            // Swagger UI için statik dosyaları servis et
            var physicalFileSystem = new PhysicalFileSystem(@".\SwaggerUI");
            var fileServerOptions = new FileServerOptions
            {
                RequestPath = new PathString(""),
                FileSystem = physicalFileSystem,
                EnableDirectoryBrowsing = true
            };
            app.UseFileServer(fileServerOptions);
        }

        public class BufferPolicySelector : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // Buffer the request content
                if (request.Content != null)
                {
                    await request.Content.LoadIntoBufferAsync();
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
