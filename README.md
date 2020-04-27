[![ReleaseBuild](https://github.com/geaz/sharpWebview/workflows/ReleaseBuild/badge.svg)](https://github.com/geaz/sharpWebview/actions?query=workflow%3AReleaseBuild)
[![WebviewNative](https://github.com/geaz/sharpWebview/workflows/WebviewNative/badge.svg)](https://github.com/geaz/sharpWebview/actions?query=workflow%3AWebviewNative)
[![Nuget](https://img.shields.io/nuget/v/SharpWebview?color=green)](https://www.nuget.org/packages/SharpWebview/)

# sharpWebview

This repository contains battery included C# bindings for [zserge/webview](https://github.com/zserge/webview). **It only supports x64 systems.**

# Webview

[zserge/webview](https://github.com/zserge/webview) is a small C/C++ header only library for a cross platform access of a webview control.
It uses Edge Chromium, with fallback to the 'old' Edge, on Windows, GTK Webkit on Linux and Cocoa Webkit on maxOS.
*sharpWebview* ships precompiled libraries for each system, ready to be used in your next C# project. This repository contains a cmake file to compile webview via *Github Actions* [![WebviewNative](https://github.com/geaz/sharpWebview/workflows/WebviewNative/badge.svg)](https://github.com/geaz/sharpWebview/actions?query=workflow%3AWebviewNative).

You are always able to see which webview version *sharpWebview* uses by looking into the [CMakeLists.txt](https://github.com/geaz/sharpWebview/blob/master/CMakeLists.txt) (GIT_TAG option in the *FetchContent_Declar command). You can find all compiled libraries and used patches in the [libs](https://github.com/geaz/sharpWebview/tree/master/libs) folder of this repository.

All patches are also contributed back to [zserge/webview](https://github.com/zserge/webview).

# Get started

## A basic example

Create a new .net core console application and add the **SharpWebview** nuget. Use the dotnet command line or the package management in Visual Studio, if you use it.

```
dotnet add package SharpWebview
```

Always add the *[STAThread]* attribute to the main method. This is necessary to work on windows at least.

```
[STAThread]
static void Main(string[] args)
{
```

To create a webview use a *using* block. This way you make sure that the native webview gets always disposed correctly!

```
using SharpWebview;

[...]

using(var webview = new Webview())
{
}
```

To open a basic webview which is pointing to a wikipedia article use the following code:

```
using(var webview = new Webview())
{
    webview
        .SetTitle("The Hitchhicker")             
        .SetSize(1024, 768, WebviewHint.None)
        .SetSize(800, 600, WebviewHint.Min)
        .Navigate(new UrlContent("https://en.wikipedia.org/wiki/The_Hitchhiker%27s_Guide_to_the_Galaxy_(novel)"))
        .Run();
}
```

The [examples](https://github.com/geaz/sharpWebview/tree/master/examples) folder contains two basic projects. The [*Minimal*](https://github.com/geaz/sharpWebview/tree/master/examples/Minimal) projects shows you a basic example on how to create a cross platform webview and how to open a URL in it.
Please have a look into the documented [*Program.cs*](https://github.com/geaz/sharpWebview/blob/master/examples/Minimal/Program.cs). You are also able to use the *HtmlContent* to provide some HTML which the webview will render.

## Run a webserver to serve a javascript application

Besides the *UrlContent* and *HtmlContent*, **sharpWebview** also provides a *HostedContent*. This content type creates a webserver on start of the application to which the webview will automatically navigate.

To use this content it is necessary to create a *wwwroot* folder in your project. Every file you put into this folder will be served by the *HostedContent*. The [*DesktopApp*](https://github.com/geaz/sharpWebview/tree/master/examples/DesktopApp) project is an example of the usage of this content type.
Don't forget to set the files in the *wwwroot* folder to *copy always* (see project file for an example).

### HostedContent on Windows systems

The Edge webview uses a UWP application context on windows. This context disallowes loopbacks, if the application is not installed. For development purpose it is necessary to run the following command in an administrative command prompt:

```
CheckNetIsolation.exe LoopbackExempt -a -n="Microsoft.Win32WebViewHost_cw5n1h2txyewy"
```

This adds the Edge Webview Host to the exception list of this limitation. If you want to ship your application, you have to provide a MSI installer. Only application installed on a windows system are not affected by this behaviour.