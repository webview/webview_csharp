[![ReleaseBuild](https://github.com/webview/sharpWebview/workflows/ReleaseBuild/badge.svg)](https://github.com/webview/sharpWebview/actions?query=workflow%3AReleaseBuild)
[![WebviewNative](https://github.com/webview/sharpWebview/workflows/WebviewNative/badge.svg)](https://github.com/webview/sharpWebview/actions?query=workflow%3AWebviewNative)
[![Nuget](https://img.shields.io/nuget/v/SharpWebview?color=green)](https://www.nuget.org/packages/SharpWebview/)
[![Webview Org Chat](https://img.shields.io/badge/chat-on%20discord-7289da.svg)](https://discord.gg/grzBQBP)

# sharpWebview

This repository contains battery included C# bindings for [webview](https://github.com/webview/webview). **It only supports x64 systems.**

# Webview

[webview](https://github.com/webview/webview) is a small C/C++ header only library for a cross platform access of a webview control.
It uses Edge Chromium, with fallback to the 'old' Edge, on Windows, GTK Webkit on Linux and Cocoa Webkit on macOS.
*sharpWebview* ships precompiled libraries for each system, ready to be used in your next C# project. This repository contains a cmake file to compile webview via *Github Actions* [![WebviewNative](https://github.com/webview/sharpWebview/workflows/WebviewNative/badge.svg)](https://github.com/webview/sharpWebview/actions?query=workflow%3AWebviewNative).

You are always able to see which webview version *sharpWebview* uses by looking into the [CMakeLists.txt](https://github.com/webview/sharpWebview/blob/master/CMakeLists.txt) (GIT_TAG option in the *FetchContent_Declare* command). You can find all compiled libraries and used patches in the [libs](https://github.com/webview/sharpWebview/tree/master/libs) folder of this repository.

All patches are also contributed back to [webview](https://github.com/webview/webview).

# Get started

## Linux Prerequisites

Please install the developer packages of webkit2gtk and libgtk on your machine. 

With a distribution using apt run:  
```
sudo apt install -y libwebkit2gtk-4.0-dev libgtk-3-dev
```

## A basic example

Create a new .net core console application and add the **SharpWebview** nuget. Use the dotnet command line or the package management in Visual Studio, if you use it.

```
dotnet add package SharpWebview
```

Always add the *[STAThread]* attribute to the main method. This is necessary to work on windows at least.

```csharp
[STAThread]
static void Main(string[] args)
```

To create a webview use a *using* block. This way you make sure that the native webview gets disposed correctly!

```csharp
using SharpWebview;

[...]

using(var webview = new Webview())
{
}
```

To open a basic webview which is pointing to a wikipedia article use the following code:

```csharp
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

The [examples](https://github.com/webview/sharpWebview/tree/master/examples) folder contains two basic projects. The [*Minimal*](https://github.com/webview/sharpWebview/tree/master/examples/Minimal) projects shows you a basic example on how to create a cross platform webview and how to open a URL in it.
Please have a look into the documented [*Program.cs*](https://github.com/webview/sharpWebview/blob/master/examples/Minimal/Program.cs). You are also able to use the *HtmlContent* to provide some HTML which the webview will render.

## Run a webserver to serve a javascript application

Besides the *UrlContent* and *HtmlContent*, *sharpWebview* also provides a *HostedContent*. This content type creates a webserver to which the webview will automatically navigate.

To use this content it is necessary to create a *app* folder in your project. Every file you put into this folder will be served by the *HostedContent*. The [*DesktopApp*](https://github.com/webview/sharpWebview/tree/master/examples/DesktopApp) project is an example of the usage of this content type.
Don't forget to set the files in the *app* folder to *copy always* (see project file for an example).

### HostedContent on Windows systems

The Edge webview uses a UWP application context on windows. UWP applications disallow loopbacks. For development purpose it is necessary to run the following command in an administrative command prompt:

```
CheckNetIsolation.exe LoopbackExempt -a -n="Microsoft.Win32WebViewHost_cw5n1h2txyewy"
```

This adds the Edge Webview Host to the exception list of this limitation. Your best bet for application distribution is to create an installer which executes this command on installation.

# Application Distribution
## Windows
 
The [*DesktopApp*](https://github.com/webview/sharpWebview/tree/master/examples/DesktopApp) example contains a simple script to create a MSI installer. You are able to take the *wix.bat* and *DesktopApp.wix* files as a starting point for your application. To use the *wix.bat* you need to install the WIX Toolset.

I highly recommend to use [scoop](https://scoop.sh/) to install it. Scoop is a command line installer for easy installation of many different applications. Just run 

```
scoop install wixtoolset
```

to install WIX. After this you should be able to execute the *wix.bat* to create a basic installer for the example [*DesktopApp*](https://github.com/webview/sharpWebview/tree/master/examples/DesktopApp).
