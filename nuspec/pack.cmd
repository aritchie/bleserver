@echo off
del *.nupkg
rem nuget pack Acr.Ble.Server.nuspec
nuget pack Plugin.BleGattServer.nuspec
pause