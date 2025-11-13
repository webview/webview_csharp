using SharpWebview;
using SharpWebview.Content;
using System.Reflection.Metadata;
using System.Text.Json;

namespace ConsoleDemo
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using var webview = new Webview(true);
            // Bind a native method as javascript
            // This method opens the url parameter in the system browser
            webview.Bind("openExternalLink", (id, req) =>
            {
                var array = JsonSerializer.Deserialize<string[]>(req)!;
                Console.WriteLine(array[0]);
                webview.Return(id, RPCResult.Success, "{}");
            });

            // On Init of the webview we inject some javascript
            // This javascript intercepts all click events and checks,
            // if the intercepted click is an external link.
            // In case of an external link the registered native method is called.
            webview.InitScript(@"
                function interceptClickEvent(e) {
                    var href = '';
                    var target = e.target || e.srcElement;
                    if (target.tagName === 'A') {
                        href = target.getAttribute('href');
                    }
                    else if(target.tagName === 'IMG') {
                        href = target.parentElement.getAttribute('href');
                    }
                    if(href.startsWith('http') 
                        && !href.startsWith('http://localhost')
                        && !href.startsWith('http://127.0.0.1')
                    ) {
                        openExternalLink(href);
                        e.preventDefault();
                    }
                }

                if (document.addEventListener) {
                    document.addEventListener('click', interceptClickEvent);
                } else if (document.attachEvent) {
                    document.attachEvent('onclick', interceptClickEvent);
                }

                window.addEventListener('popstate', function(event) {
                    openExternalLink(document.location.href);
                });
            ");

            webview.SetTitle("User Demo")
                   .SetSize(1024, 768, WebviewHint.None)
                   .SetSize(800, 600, WebviewHint.Min)
                   .Navigate(new UrlContent("https://www.baidu.com"))
                   .Run();
        }
    }
}
