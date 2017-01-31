using System;


namespace Acr.Ble.Server
{
    [Flags]
    public enum CharacteristicProperties
    {
        Broadcast = 1, //0x1
        Read = 2, //0x2
        WriteWithoutResponse = 4, //0x4
        Write = 8, //0x8
        Notify = 16, //0x10
        Indicate = 32, //0x20
        AuthenticatedSignedWrites = 64, //0x40
        ExtendedProperties = 128, //0x80
        NotifyEncryptionRequired = 256, //0x100
        IndicateEncryptionRequired = 512
    }
}

/*
 * iOS
        Broadcast = 1uL,
        Read = 2uL,
        WriteWithoutResponse = 4uL, // writenoresponse
        Write = 8uL,
        Notify = 16uL,
        Indicate = 32uL,
        AuthenticatedSignedWrites = 64uL,
        ExtendedProperties = 128uL,
        NotifyEncryptionRequired = 256uL,
        IndicateEncryptionRequired = 512uL

    Android
        Broadcast
        ExtendedProps
        Indicate
        Notify
        Read
        SignedWrite
        Write
        WriteNoResponse

        //NotifyEncryptionRequired = 256, //0x100 //ReliableWrites = 256 UWP
        //IndicateEncryptionRequired = 512 //WritableAuxiliaries = 512
*/