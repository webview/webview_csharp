cmake_minimum_required(VERSION 3.11.0)
project(webview VERSION 0.10.0)

set(CMAKE_CXX_STANDARD 17)

if(${CMAKE_SYSTEM_NAME} MATCHES "Windows")
    set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /MT -DWEBVIEW_STATIC")
    set(CMAKE_CXX_FLAGS_DEBUG "${CMAKE_CXX_FLAGS_DEBUG} /MTd -DWEBVIEW_STATIC")
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
    set(CMAKE_CXX_FLAGS "-DWEBVIEW_STATIC")
elseif(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
    set(CMAKE_CXX_FLAGS  "${CMAKE_CXX_FLAGS} -framework WebKit -DWEBVIEW_STATIC")
endif()

include(FetchContent)
FetchContent_Declare(
    webviewNative
    GIT_REPOSITORY https://github.com/webview/webview
    GIT_TAG 53ea174ce79ca2f52e28dd51d49052aebce3f4c5
)
FetchContent_Populate(webviewNative)

include_directories(
    "${webviewnative_SOURCE_DIR}"
    "${CMAKE_CURRENT_SOURCE_DIR}/libs/include"
)
link_directories("${CMAKE_CURRENT_SOURCE_DIR}/libs")

add_library(webview SHARED "${webviewnative_SOURCE_DIR}/webview.cc")

if(${CMAKE_SYSTEM_NAME} MATCHES "Windows")
    target_compile_definitions(webview PRIVATE "WEBVIEW_API=__declspec(dllexport)")
    target_link_libraries(webview WebView2Loader.dll.lib)
    configure_file(${CMAKE_CURRENT_SOURCE_DIR}/libs/WebView2Loader.dll WebView2Loader.dll COPYONLY)
elseif(${CMAKE_SYSTEM_NAME} MATCHES "Linux")
    target_link_libraries(webview PkgConfig::gtk3 PkgConfig::webkit2)
endif()