# ACR BluetoothLE Server Plugin for Xamarin
Easy to use, cross platform, REACTIVE BluetoothLE Server Plugin for Xamarin

[![NuGet](https://img.shields.io/nuget/v/Acr.Ble.Server.svg?maxAge=2592000)](https://www.nuget.org/packages/Acr.Ble.Server/)

## PLATFORMS

* Android 4.3+
* iOS 8+

## SETUP

### Android

_Add the following to your AndroidManifest.xml_
 
```xml
<uses-permission android:name="android.permission.BLUETOOTH"/>
<uses-permission android:name="android.permission.BLUETOOTH_ADMIN"/>
```

### iOS
_Add the following to your Info.plist_

```xml
<array>
<string>bluetooth-peripheral</string>
</array>
```


## HOW TO USE
More documentation coming soon.  Here is an easy sample in the meantime

[Sample](https://github.com/aritchie/bleserver/blob/master/Samples/Samples/ViewModels/EasyServerViewModel.cs)