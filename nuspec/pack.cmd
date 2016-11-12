@echo off
del *.nupkg
nuget pack Acr.Ble.Server.nuspec
nuget pack Plugin.BluetoothLE.Server.nuspec
pause