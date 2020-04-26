using System.Linq;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace SharpWebview.Content
{
    public sealed class HostedContent : IWebviewContent
    {
        private readonly IWebHost _webHost;

        public HostedContent(int port = 0)
        {
            _webHost = WebHost.CreateDefaultBuilder()
                   .UseStartup<Startup>()
                   .UseKestrel(options => options.Listen(IPAddress.Loopback, port))
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
            app.UseFileServer();
        }
    }
}