# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.7.0] - 2022-04-13
### Fixed
- Update to latest webview version

## [0.6.1] - 2020-07-11
### Fixed
- Dispatch delegates pinned until execution

## [0.6.0] - 2020-05-17
### Add
- Add binding for webview_dispatch
- New webview version

## [0.5.5] - 2020-05-05
### Add
- Link interception for image links
- Option activate/deactivate logging of the host
- New webview version

## [0.5.4] - 2020-04-29
### Add
- Intercept clicks to external links and open them in the system browser

### Fixed
- GC no longer disposes the callback functions of the webview binds

## [0.5.3] - 2020-04-27
### Changed
- Renamed *wwwroot* to *app* - HostedContent

### Fixed
- HostedContent now serves the *app* folder correctly even, if executed from another working directory

## [0.5.2] - 2020-04-26
### Add
- Bindings for zserge/webview
- HTML Content support
- URL support
- Hosted Content support