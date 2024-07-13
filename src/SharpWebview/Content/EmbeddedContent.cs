
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace SharpWebview.Content
{
    /// <summary>
    /// From user Lemonify
    /// https://github.com/webview/webview_csharp/issues/31#issuecomment-1848414030
    /// </summary>
    public class EmbeddedContent : HostedContent
    {
        /// <summary>
        /// Dont forget to setup the static files in the csproj:
        /// <ItemGroup>
        /// 	<EmbeddedResource Include="dist\**\*">
        /// 		<LogicalName>%(Filename)%(Extension)</LogicalName>
        /// 	</EmbeddedResource>
        /// </ItemGroup>
        /// 
        /// The folder should be flattened (all files in the root folder)
        /// 
        /// Usage example:
        /// 
        /// IWebviewContent content = new EmbeddedContent(typeof(Program).Assembly);
        /// webview.Navigate(content).Run();
        /// </summary>
        public EmbeddedContent(Assembly embeddedAssembly, int port = 0, bool activateLog = false, IDictionary<string, string>? additionalMimeTypes = null) :
        base(GetEmbeddedProvider(embeddedAssembly), port, activateLog, additionalMimeTypes)
        { }

        private static IFileProvider GetEmbeddedProvider(Assembly embeddedAssembly)
        {
            return new EmbeddedFileProvider(embeddedAssembly, "");
        }
    }
}