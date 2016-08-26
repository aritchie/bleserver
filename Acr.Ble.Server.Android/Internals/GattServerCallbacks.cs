using System;
using Android.Bluetooth;


namespace Acr.Ble.Server.Internals
{
    public class GattServerCallbacks : BluetoothGattServerCallback
    {

        public override void OnCharacteristicReadRequest(BluetoothDevice device,
                                                         int requestId,
                                                         int offset,
                                                         BluetoothGattCharacteristic characteristic)
        {
            base.OnCharacteristicReadRequest(device, requestId, offset, characteristic);
        }


        public override void OnCharacteristicWriteRequest(BluetoothDevice device,
                                                          int requestId,
                                                          BluetoothGattCharacteristic characteristic,
                                                          bool preparedWrite,
                                                          bool responseNeeded,
                                                          int offset,
                                                          byte[] value)
        {
            base.OnCharacteristicWriteRequest(device, requestId, characteristic, preparedWrite, responseNeeded, offset, value);
        }


        public override void OnDescriptorReadRequest(BluetoothDevice device, 
                                                     int requestId, int offset, BluetoothGattDescriptor descriptor)
        {
            base.OnDescriptorReadRequest(device, requestId, offset, descriptor);
        }


        public override void OnDescriptorWriteRequest(BluetoothDevice device, int requestId, BluetoothGattDescriptor descriptor,
                                                      bool preparedWrite, bool responseNeeded, int offset, byte[] value)
        {
            base.OnDescriptorWriteRequest(device, requestId, descriptor, preparedWrite, responseNeeded, offset, value);
        }


        public override void OnConnectionStateChange(BluetoothDevice device, ProfileState status, ProfileState newState)
        {
            base.OnConnectionStateChange(device, status, newState);
        }


        //public override void OnExecuteWrite(BluetoothDevice device, int requestId, bool execute)
        //{
        //    base.OnExecuteWrite(device, requestId, execute);
        //}


        //public override void OnMtuChanged(BluetoothDevice device, int mtu)
        //{
        //    base.OnMtuChanged(device, mtu);
        //}


        //public override void OnNotificationSent(BluetoothDevice device, GattStatus status)
        //{
        //    base.OnNotificationSent(device, status);
        //}


        //public override void OnServiceAdded(ProfileState status, BluetoothGattService service)
        //{
        //    base.OnServiceAdded(status, service);
        //}
    }
}
