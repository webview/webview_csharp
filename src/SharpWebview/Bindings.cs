using System;
using System.Runtime.InteropServices;

namespace SharpWebview
{
    public enum WebviewHint
    {
        /// <summary>
        /// Width and height are default size
        /// </summary>
        None = 0,
        /// <summary>
        /// Width and height are minimum bounds
        /// </summary>
        Min = 1,
        /// <summary>
        ///  Width and height are maximum bounds
        /// </summary>
        Max = 2,
        /// <summary>
        /// Window size can not be changed by a user
        /// </summary>
        Fixed = 3,
    }

    public enum RPCResult
    {
        Success = 0,
        Error = 1,
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CallBackFunction(
        [MarshalAs(UnmanagedType.LPStr)] string id,
        [MarshalAs(UnmanagedType.LPStr)] string req,
        IntPtr arg);

    internal static class Bindings
    {
        private const string DllFile = "webview";

        /// <summary>
        /// Creates a new webview instance.
        /// 
        /// Binding for:
        /// WEBVIEW_API webview_t webview_create(int debug, void *window);
        /// </summary>
        /// <param name="debug">
        /// If debug is non-zero - developer tools will
        /// be enabled (if the platform supports them).
        /// </param>
        /// <param name="window">
        /// Window parameter can be a
        /// pointer to the native window handle. If it's non-null - then child WebView
        /// is embedded into the given parent window. Otherwise a new window is created.
        /// Depending on the platform, a GtkWindow, NSWindow or HWND pointer can be
        /// passed here.
        /// </param>
        /// <returns></returns>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr webview_create(int debug, IntPtr window);

        /// <summary>
        /// Destroys a webview and closes the native window.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_destroy(webview_t w);
        /// </summary>
        /// <param name="webview">The webview pointer to destroy.</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_destroy(IntPtr webview);

        /// <summary>
        /// Runs the main loop until it's terminated. After this function exits - you
        /// must destroy the webview.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_run(webview_t w);
        /// </summary>
        /// <param name="webview">The webview pointer to run.</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_run(IntPtr webview);

        /// <summary>
        /// Stops the main loop. It is safe to call this function from another other
        /// background thread.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_terminate(webview_t w);
        /// </summary>
        /// <param name="webview">The webview pointer to terminate.</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_terminate(IntPtr webview);
               
        /// <summary>
        /// Updates the title of the native window. Must be called from the UI thread.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_set_title(webview_t w, const char* title);
        /// </summary>
        /// <param name="webview">The webview to update.</param>
        /// <param name="title">New webview title.</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_set_title(IntPtr webview, [MarshalAs(UnmanagedType.LPStr)] string title);

        /// <summary>
        /// Updates native window size.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_set_size(webview_t w, int width, int height, int hints);
        /// </summary>
        /// <param name="webview">The webview to update</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="hint">Size behaviour</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_set_size(IntPtr webview, int width, int height, WebviewHint hint);

        /// <summary>
        /// Navigates webview to the given URL. URL may be a data URI, i.e.
        /// "data:text/text,<html>...</html>". It is often ok not to url-encode it
        /// properly, webview will re-encode it for you.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_navigate(webview_t w, const char* url);
        /// </summary>
        /// <param name="webview">The webview to update</param>
        /// <param name="url">The url to navigate to</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_navigate(IntPtr webview, [MarshalAs(UnmanagedType.LPStr)] string url);

        /// <summary>
        /// Injects JavaScript code at the initialization of the new page. Every time
        /// the webview will open a the new page - this initialization code will be
        /// executed. It is guaranteed that code is executed before window.onload.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_init(webview_t w, const char* js);
        /// </summary>
        /// <param name="webview">The webview to execute the javascript.</param>
        /// <param name="js">The javascript to execute.</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_init(IntPtr webview, [MarshalAs(UnmanagedType.LPStr)] string js);

        /// <summary>
        /// Evaluates arbitrary JavaScript code. Evaluation happens asynchronously, also
        /// the result of the expression is ignored. Use RPC bindings if you want to
        /// receive notifications about the results of the evaluation.
        /// 
        /// Binding for:
        /// WEBVIEW_API void webview_eval(webview_t w, const char *js);
        /// </summary>
        /// <param name="webview">The webview to execute the javascript in.</param>
        /// <param name="js">The javascript to evaluate.</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_eval(IntPtr webview, [MarshalAs(UnmanagedType.LPStr)] string js);
        
        /// <summary>
        /// Binds a native C callback so that it will appear under the given name as a
        /// global JavaScript function. Internally it uses webview_init(). Callback
        /// receives a request string and a user-provided argument pointer. Request
        /// string is a JSON array of all the arguments passed to the JavaScript
        /// function.
        ///
        /// Binding for:
        /// WEBVIEW_API void webview_bind(webview_t w, const char* name, 
        ///     void (* fn) (const char* seq, const char* req, void* arg), void* arg);
        /// </summary>
        /// <param name="webview"></param>
        /// <param name="js"></param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_bind(
            IntPtr webview,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            CallBackFunction callback,
            IntPtr arg);

        /// <summary>
        /// Allows to return a value from the native binding. Original request pointer
        /// must be provided to help internal RPC engine match requests with responses.
        /// If status is zero - result is expected to be a valid JSON result value.
        /// If status is not zero - result is an error JSON object.
        ///
        /// Binding for:
        /// WEBVIEW_API void webview_return(webview_t w, const char* seq, int status, const char* result);
        /// </summary>
        /// <param name="webview">The webview to return the result to.</param>
        /// <param name="id">The id of the call.</param>
        /// <param name="result">The result of the call.</param>
        /// <param name="resultJson">The json data to return to the webview.</param>
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webview_return(IntPtr webview,
            [MarshalAs(UnmanagedType.LPStr)] string id,
            RPCResult result,
            [MarshalAs(UnmanagedType.LPStr)] string resultJson);

        /*
        Not mapped:
            // Posts a function to be executed on the main thread. You normally do not need
            // to call this function, unless you want to tweak the native window.
            WEBVIEW_API void
            webview_dispatch(webview_t w, void (*fn)(webview_t w, void *arg), void *arg);

            // Returns a native window handle pointer. When using GTK backend the pointer
            // is GtkWindow pointer, when using Cocoa backend the pointer is NSWindow
            // pointer, when using Win32 backend the pointer is HWND pointer.
            WEBVIEW_API void *webview_get_window(webview_t w);
        */
    }
}
