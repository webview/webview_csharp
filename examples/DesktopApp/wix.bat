@echo off

echo Build Project
dotnet build --configuration Release

echo Harvest ApplicationFiles
heat dir bin/Release/netcoreapp3.1 -o Files.wxs -dr INSTALLDIR -cg ApplicationFiles -gg -sreg -srd -scom

echo Executing Candle
candle DesktopApp.wxs Files.wxs 

echo Executing Light
light DesktopApp.wixobj Files.wixobj -ext WixUIExtension -b bin/Release/netcoreapp3.1 -o DesktopApp