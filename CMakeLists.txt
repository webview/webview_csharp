cmake_minimum_required(VERSION 3.11.0)
project(webview VERSION 0.10.0)

set(CMAKE_CXX_STANDARD 17)

if(${CMAKE_SYSTEM_NAME} MATCHES "Windows")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /MT")
    set(CMAKE_CXX_FLAGS_DEBUG "${CMAKE_CXX_FLAGS_DEBUG} /MTd")
elseif(${CMAKE_SYSTEM_NAME} MATCHES "Linux")
    find_package(PkgConfig REQUIRED)
    pkg_check_modules(
        gtk3 REQUIRED gtk+-3.0
        IMPORTED_TARGET
      )
    pkg_check_modules(
        webkit2 REQUIRED webkit2gtk-4.0
        IMPORTED_TARGET
      )
elseif(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
    set(CMAKE_CXX_FLAGS  "${CMAKE_CXX_FLAGS} -framework WebKit")
endif()

include(FetchContent)
FetchContent_Declare(
    webviewNative
    GIT_REPOSITORY https://github.com/geaz/webview
    GIT_TAG 313eae7e61519baa1799509ca98e187bf1f28dbe
)
FetchContent_Populate(webviewNative)

include_directories(
    "${webviewnative_SOURCE_DIR}"
    "${webviewnative_SOURCE_DIR}/script/microsoft.web.webview2.0.9.488/build/native/include"
)
link_directories("${webviewnative_SOURCE_DIR}/script/microsoft.web.webview2.0.9.488/build/native/x64")

add_library(webview SHARED "${webviewnative_SOURCE_DIR}/webview.cc")

if(${CMAKE_SYSTEM_NAME} MATCHES "Windows")
    target_compile_definitions(webview PRIVATE "WEBVIEW_API=__declspec(dllexport)")
    target_link_libraries(webview WebView2Loader.dll.lib)
    configure_file(${webviewnative_SOURCE_DIR}/script/microsoft.web.webview2.0.9.488/build/x64/WebView2Loader.dll WebView2Loader.dll COPYONLY)
elseif(${CMAKE_SYSTEM_NAME} MATCHES "Linux")
    target_link_libraries(webview PkgConfig::gtk3 PkgConfig::webkit2)
endif()