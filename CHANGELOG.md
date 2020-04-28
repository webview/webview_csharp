# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Add
- Intercept clicks to external links and open them in the system browser
- Patch for webview to open the webview on 'Run' instead of initialization (Windows)

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