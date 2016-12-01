# #Frequently Asked Questions (FAQ)

* Q: Are multiple subscribers supported 
> A: _This has not yet been fully tested_

* Q: Why can't I disconnect devices selectively
>  A: _On android, you can, but exposing this functionality in xplat proves challenging since iOS does not support A LOT of things_
  
* Q: If Android is a better API, will you expose functionality from them even though it is not xplat?
>  A: _Eventually... I just have to pick and choose as I have done with all of my other libraries_

* Q: UWP support
> A: _There is currently no GATT API for UWP.  If there was, it could be as dev hostile as their client BLE library and may not be useful_
  
* Q: How can I cannot to a GATT server cross platform?
> A: _Use my other library on [github](https://github.com/aritchie/bluetoothle) or on [nuget](https://www.nuget.org/packages/Acr.Ble/)_