@echo off
del *.nupkg
nuget pack Acr.Ble.Server.nuspec
pause