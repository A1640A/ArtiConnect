using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(ArtiConnect.Api.Startup))]
namespace ArtiConnect.Api
{
    public class Startup
    {
        // DLL yükleme için P/Invoke fonksiyonları
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        // DLL yükleme durumunu izlemek için statik değişken
        public static bool IsDllLoaded { get; private set; } = true;
        public static string DllLoadError { get; private set; } = null;

        public void Configuration(IAppBuilder app)
        {
            // DLL'i yükleme işlemi
            //try
            //{
            //    // Olası DLL yollarını kontrol et
            //    string[] potentialPaths = new string[]
            //    {
            //        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PAYGO_PCPOSOKC.dll"),
            //        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "PAYGO_PCPOSOKC.dll"),
            //        Path.Combine(Environment.CurrentDirectory, "PAYGO_PCPOSOKC.dll"),
            //        // Ek yollar eklenebilir
            //    };

            //    string dllPath = null;
            //    foreach (string path in potentialPaths)
            //    {
            //        System.Diagnostics.Debug.WriteLine($"DLL aranıyor: {path}");
            //        if (File.Exists(path))
            //        {
            //            dllPath = path;
            //            System.Diagnostics.Debug.WriteLine($"DLL bulundu: {path}");
            //            break;
            //        }
            //    }

            //    if (dllPath == null)
            //    {
            //        DllLoadError = "PAYGO_PCPOSOKC.dll hiçbir konumda bulunamadı.";
            //        System.Diagnostics.Debug.WriteLine(DllLoadError);
            //    }
            //    else
            //    {
            //        // DLL'i manuel olarak yükle
            //        IntPtr hLib = LoadLibrary(dllPath);
            //        if (hLib == IntPtr.Zero)
            //        {
            //            int errorCode = Marshal.GetLastWin32Error();
            //            //DllLoadError = $"DLL yüklenemedi. Hata kodu: {errorCode}";
            //            System.Diagnostics.Debug.WriteLine(DllLoadError);
            //            IsDllLoaded = true;
            //        }
            //        else
            //        {
            //            IsDllLoaded = true;//
            //            System.Diagnostics.Debug.WriteLine("DLL başarıyla yüklendi.");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DllLoadError = $"DLL yükleme hatası: {ex.Message}";
            //    System.Diagnostics.Debug.WriteLine(DllLoadError);
            //}

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

            // DLL yükleme durumunu kontrol eden bir mesaj işleyici ekle
            config.MessageHandlers.Add(new DllCheckHandler());
            config.Filters.Add(new ApiLoggerAttribute());
            config.MessageHandlers.Add(new BufferPolicySelector());

            config.EnsureInitialized();

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

        // DLL yükleme durumunu kontrol eden mesaj işleyici
        public class DllCheckHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // Eğer DLL yüklenemedi ise ve PayGo API'sine bir istek yapılıyorsa hata döndür
                if (!IsDllLoaded && request.RequestUri.AbsolutePath.Contains("/api/paygo/"))
                {
                    var response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        Content = new StringContent($"{{\"message\": \"{DllLoadError}\"}}", Encoding.UTF8, "application/json")
                    };
                    return Task.FromResult(response);
                }

                return base.SendAsync(request, cancellationToken);
            }
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