using System;
using SharpWebview;
using SharpWebview.Content;

namespace Minimal
{
    class Program
    {
        // Set the application to Single Threaded Apartment
        // Otherwise the webview won't work on windows at least
        [STAThread]
        static void Main(string[] args)
        {
            // Wrap the usage of the webview into a using block
            // Otherwise the native window will not get disposed correctly
            using(var webview = new Webview(true))
            {
                webview
                    // Set the title of the application window
                    .SetTitle("The Hitchhicker")
                    // Set the start size of the window                
                    .SetSize(1024, 768, WebviewHint.None)
                    // Set the minimum size of the window
                    .SetSize(800, 600, WebviewHint.Min)
                    // This script gets executed after navigating to the url
                    .InitScript("window.x = 42;")
                    // Bind a c# function to the webview - Accessible with the name "evalTest"
                    .Bind("evalTest", (id, req) =>
                    {
                        // Executes the javascript on the webview
                        webview.Evaluate("console.log('The anwser is ' + window.x);");
                        // And returns a successful promise result to the javascript function, which executed the 'evalTest'
                        webview.Return(id, RPCResult.Success, "{ result: 'We always knew it!' }");
                    })
                    // Navigate to this url on start
                    .Navigate(new UrlContent("https://en.wikipedia.org/wiki/The_Hitchhiker%27s_Guide_to_the_Galaxy_(novel)"))
                    // Run the webview loop
                    .Run();
            }                
        }
    }
}
