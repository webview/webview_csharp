using System;
using System.Text;

namespace SharpWebview.Content
{
    public sealed class HtmlContent : IWebviewContent
    {
        private readonly string _html;

        public HtmlContent(string html)
        {
            _html = html;
        }

        public string ToWebviewUrl()
        {
            var webviewUrl = new StringBuilder("data:text/html,");
            webviewUrl.Append(Uri.EscapeDataString(_html));

            return webviewUrl.ToString();
        }
    }
}