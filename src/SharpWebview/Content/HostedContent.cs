using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace SharpWebview.Content
{
    public sealed class HostedContent : IWebviewContent
    {
        private readonly IWebHost _webHost;

        public HostedContent(int port = 0, bool activateLog = false, IDictionary<string, string> additionalMimeTypes = null)
        {
            _webHost = WebHost.CreateDefaultBuilder()
                   .ConfigureServices(s => s.AddSingleton(x => new StartupParameters() { AdditionalMimeTypes = additionalMimeTypes }))
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
        private StartupParameters startupParameters;
        public Startup(StartupParameters startupParameters) 
        {
            this.startupParameters = startupParameters;
        }
        public void ConfigureServices(IServiceCollection services) { }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var fileServerOptions = new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(System.Environment.CurrentDirectory, "app")),
                RequestPath = "",
                EnableDirectoryBrowsing = true,
            };
            if(startupParameters.AdditionalMimeTypes != null)
            {
                var extensionProvider = new FileExtensionContentTypeProvider();
                foreach (var mimeType in startupParameters.AdditionalMimeTypes)
                    extensionProvider.Mappings.Add(mimeType);
                fileServerOptions.StaticFileOptions.ContentTypeProvider = extensionProvider;
            }
            app.UseFileServer(fileServerOptions);
        }
    }
}
