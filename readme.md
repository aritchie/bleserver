# ACR BluetoothLE Server Plugin for Xamarin
Easy to use, cross platform, REACTIVE BluetoothLE Server Plugin for Xamarin

[![NuGet](https://img.shields.io/nuget/v/Acr.Ble.Server.svg?maxAge=2592000)](https://www.nuget.org/packages/Acr.Ble.Server/)

## PLATFORMS

* Android 4.3+
* iOS 8+

## FEATURES

* Advertise Bluetooth services on iOS or Android device
* Charactertistics
    * Read
    * Write
    * Notify & Broadcast
    * Manage Subscribers
    * Status Replies

## BASIC USE

Most important things - you should setup all of your services and characteristics BEFORE you Start() the server!

```csharp
var server = BleAdapter.Current.CreateGattServer();
var service = server.AddService(Guid.NewGuid(), true);

var characteristic = service.AddCharacteristic(
    Guid.NewGuid(),
    CharacteristicProperties.Read | CharacteristicProperties.Write | CharacteristicProperties.WriteWithoutResponse,
    GattPermissions.Read | GattPermissions.Write
);

var notifyCharacteristic = service.AddCharacteristic
(
    Guid.NewGuid(),
    CharacteristicProperties.Indicate | CharacteristicProperties.Notify,
    GattPermissions.Read | GattPermissions.Write
);

notifyCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
{
    var @event = e.IsSubscribed ? "Subscribed" : "Unsubcribed";

    if (this.notifyBroadcast == null)
    {
        this.notifyBroadcast = Observable
            .Interval(TimeSpan.FromSeconds(1))
            .Where(x => notifyCharacteristic.SubscribedDevices.Count > 0)
            .Subscribe(_ =>
            {
                Debug.WriteLine("Sending Broadcast");
                var dt = DateTime.Now.ToString("g");
                var bytes = Encoding.UTF8.GetBytes(dt);
                notifyCharacteristic.Broadcast(bytes);
            });
    }
});

characteristic.WhenReadReceived().Subscribe(x =>
{
    var write = this.CharacteristicValue;
    if (write.IsEmpty())
        write = "(NOTHING)";

    // you must set a reply value
    x.Value = Encoding.UTF8.GetBytes(write);

    x.Status = GattStatus.Success; // you can optionally set a status, but it defaults to Success
});
characteristic.WhenWriteReceived().Subscribe(x =>
{
    var write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);
    // do something
});
```

## SETUP

Make sure to install the nuget package to all of your platform projects (iOS & Android) as well as your shared library

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