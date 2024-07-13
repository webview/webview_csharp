using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SharpWebview.Content
{
    public class HostedContent : IWebviewContent
    {
        private readonly WebApplication _webApp;

        protected HostedContent(IFileProvider fileProvider, int port = 0, bool activateLog = false, IDictionary<string, string> additionalMimeTypes = null)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            builder.WebHost.UseKestrel(options => options.Listen(IPAddress.Loopback, port));

            if (!activateLog)
            {
                builder.Logging.ClearProviders();
            }

            _webApp = builder.Build();

            FileServerOptions fileServerOptions = new FileServerOptions
            {
                FileProvider = fileProvider,
                RequestPath = "",
                EnableDirectoryBrowsing = true,
            };

            if (additionalMimeTypes != null)
            {
                FileExtensionContentTypeProvider extensionProvider = new FileExtensionContentTypeProvider();

                foreach (var mimeType in additionalMimeTypes)
                {
                    extensionProvider.Mappings.Add(mimeType);
                }

                fileServerOptions.StaticFileOptions.ContentTypeProvider = extensionProvider;
            }

            _webApp.UseFileServer(fileServerOptions);
            _webApp.Start();
        }


        public HostedContent(int port = 0, bool activateLog = false, IDictionary<string, string> additionalMimeTypes = null) :
        this(GetDefaultFileProvider(), port, activateLog, additionalMimeTypes)
        { }

        private static IFileProvider GetDefaultFileProvider()
        {
            return new PhysicalFileProvider(Path.Combine(System.Environment.CurrentDirectory, "app"));
        }

        public string ToWebviewUrl()
        {
            return _webApp.Urls.First();
        }
    }
}
