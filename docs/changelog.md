# CHANGE LOG

## 2.0.0
* [feature] UWP Support!
* [feature] Advertise manufacturer data on Android and Windows
* [breaking] Start is now async

## 1.2.0
* [breaking] Broadcast is now a void again, BroadcastObserve is the observable version
* [fix][ios] Write was not replying
* [fix][droid] characteristics would sometimes not be sent with the service
* [fix][droid] Reads, Writes, and Broadcasts can flood the BLE channel if all sent.  There is now a readwrite lock in place.

## 1.1.0
* [feature] characteristic broadcast now returns a hot observable for listening to successful broadcasts
* [feature] characteristic device now has a "Context" object on it that you can set to anything you want
* [fix][ios] WhenRunningChanged was not firing for stop

## 1.0.1
* [feature] New method for getting all subscribed devices from server instance
* [feature] Ability to observable all characteristics for a subscription
* [feature] Observe running state
* [fix][ios] characteristic observable misfiring

## 1.0.0
* Initial Release