namespace SharpWebview.Content
{
    public class UrlContent : IWebviewContent
    {
        private readonly string _url;

        public UrlContent(string url)
        {
            _url = url;
        }

        public string ToWebviewUrl()
        {
            return _url;
        }
    }
}