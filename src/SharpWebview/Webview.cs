using System;
using SharpWebview.Content;

namespace SharpWebview
{
    /// <summary>
    /// A cross platform webview.
    /// This is a binding class for the native implementation of https://github.com/zserge/webview.
    /// </summary>
    public class Webview : IDisposable
    {
        private bool _disposed = false;
        private readonly IntPtr _nativeWebview;

        /// <summary>
        /// Creates a new webview object.
        /// </summary>
        /// <param name="debug">
        /// Set to true, to activate a debug view, 
        /// if the current webview implementation supports it.
        /// </param>
        public Webview(bool debug = false)
        {
            _nativeWebview = Bindings.webview_create(debug ? 1 : 0, IntPtr.Zero);
        }

        /// <summary>
        /// Set the title of the webview application window.
        /// </summary>
        /// <param name="title">The new title.</param>
        /// <returns>The webview object for a fluent api.</returns>
        public Webview SetTitle(string title)
        {
            Bindings.webview_set_title(_nativeWebview, title);
            return this;
        }

        /// <summary>
        /// Set the size information of the webview application window.
        /// </summary>
        /// <param name="width">The width of the webview application window.</param>
        /// <param name="height">The height of the webview application window.</param>
        /// <param name="hint">The type of the size information.</param>
        /// <returns>The webview object for a fluent api.</returns>
        public Webview SetSize(int width, int height, WebviewHint hint)
        {
            Bindings.webview_set_size(_nativeWebview, width, height, hint);
            return this;            
        }

        /// <summary>
        /// Injects JavaScript code at the initialization of the new page. Every time
        /// the webview will open a new page. this initialization code will be
        /// executed. It is guaranteed that code is executed before window.onload.
        /// </summary>
        /// <remarks>
        /// Execute this method before <see cref="Navigate(IWebviewContent)"/>
        /// </remarks>
        /// <param name="javascript">The javascript code to execute.</param>
        /// <returns>The webview object for a fluent api.</returns>
        public Webview InitScript(string javascript)
        {
            Bindings.webview_init(_nativeWebview, javascript);
            return this;
        }

        /// <summary>
        /// Navigates webview to the given content.
        /// </summary>
        /// <param name="webviewContent">The content to navigate to.</param>
        /// <remarks>Content can be a UrlContent, HtmlContent or WebhostContent</remarks>
        /// <returns>The webview object for a fluent api.</returns>
        public Webview Navigate(IWebviewContent webviewContent)
        {
            Bindings.webview_navigate(_nativeWebview, webviewContent.ToWebviewUrl());
            return this;
        }

        /// <summary>
        /// Binds a callback so that it will appear under the given name as a global JavaScript function. 
        /// </summary>
        /// <param name="name">Global name of the javascript function.</param>
        /// <param name="callback">Callback with two parameters. id -> The id of the call, req -> The parameters of the call as json</param>
        /// <returns>The webview object for a fluent api.</returns>
        public Webview Bind(string name, Action<string, string> callback)
        {
            Bindings.webview_bind(_nativeWebview, name, (id, req, arg) => { callback(id, req); }, IntPtr.Zero);
            return this;
        }

        /// <summary>
        /// Runs the main loop of the webview. Should be used as the last statement.
        /// </summary>
        /// <returns>The webview object.</returns>
        public Webview Run()
        {
            Bindings.webview_run(_nativeWebview);
            return this;
        }

        /// <summary>
        /// Allows to return a value to the caller of a bound callback <see cref="Bind(string, Action{string, string})"/>.
        /// </summary>
        /// <param name="id">The id of the call.</param>
        /// <param name="result">The result of the call.</param>
        /// <param name="resultJson">The result data as json.</param>
        public void Return(string id, RPCResult result, string resultJson)
        {
            Bindings.webview_return(_nativeWebview, id, result, resultJson);
        }

        /// <summary>
        /// Evaluates arbitrary JavaScript code. Evaluation happens asynchronously, also
        /// the result of the expression is ignored. Use bindings if you want to
        /// receive notifications about the results of the evaluation.
        /// </summary>
        /// <param name="javascript">The javascript to execute.</param>
        public void Evaluate(string javascript)
        {
            Bindings.webview_eval(_nativeWebview, javascript);
        }

        /// <summary>
        /// Disposes the current webview.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Bindings.webview_destroy(_nativeWebview);
                _disposed = true;
            }
        }

        ~Webview() => Dispose(false);
    }
}
