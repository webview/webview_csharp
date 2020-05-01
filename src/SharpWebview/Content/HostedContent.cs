using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace SharpWebview.Content
{
    public sealed class HostedContent : IWebviewContent
    {
        private readonly IWebHost _webHost;

        public HostedContent(int port = 0, bool activateLog = false)
        {
            _webHost = WebHost.CreateDefaultBuilder()
                   .UseStartup<Startup>()
                   .UseKestrel(options => options.Listen(IPAddress.Loopback, port))
                   .ConfigureLogging((logger) => { if(!activateLog) logger.ClearProviders(); })
                   .Build();
            _webHost.Start();
        }

        public string ToWebviewUrl()
        {
            return _webHost.ServerFeatures
                .Get<IServerAddressesFeature>()
                .Addresses
                .First();
        }
    }

    internal sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services) { }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(workingDirectory, "app")),
                RequestPath = "",
                EnableDirectoryBrowsing = true
            });
        }
    }
}