@echo off
del *.nupkg
nuget pack Acr.Ble.Server.nuspec
rem nuget pack Plugin.BluetoothLE.Server.nuspec
pause