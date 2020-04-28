using System;
using SharpWebview;
using SharpWebview.Content;

namespace DesktopApp
{
    class Program
    {
        // Set the application to Single Threaded Apartment
        // Otherwise the webview won't work on windows at least
        [STAThread]
        static void Main(string[] args)
        {
            // For a better startup expierience 
            // I recommend to init the HostedContent in advance
            var hostedContent = new HostedContent();

            // Wrap the usage of the webview into a using block
            // Otherwise the native window will not get disposed correctly.
            // By setting the second parameter to true, sharpWebview intercepts
            // all external links and opens them in the system browser.
            using (var webview = new Webview(true, true))
            {
                webview
                    // Set the title of the application window
                    .SetTitle("The Lost Hitchhicker")
                    // Set the start size of the window                
                    .SetSize(1024, 768, WebviewHint.None)
                    // Set the minimum size of the window
                    .SetSize(800, 600, WebviewHint.Min)
                    // Bind a c# function to the webview - Accessible with the name "evalTest"
                    .Bind("evalTest", (id, req) =>
                    {
                        // Req contains the parameters of the javascript call
                        Console.WriteLine(req);
                        // And returns a successful promise result to the javascript function, which executed the 'evalTest'
                        webview.Return(id, RPCResult.Success, "{ result: 42 }");
                    })
                    // Navigate to this url on start
                    .Navigate(hostedContent)
                    // Run the webview loop
                    .Run();
            }                
        }
    }
}
