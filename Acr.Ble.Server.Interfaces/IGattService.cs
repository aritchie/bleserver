using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public interface IGattService
    {
        IGattServer Server { get; }

        Guid Uuid { get; }
        bool IsPrimary { get; }

        IGattCharacteristic AddCharacteristic(Guid uuid, CharacteristicProperties properties, GattPermissions permissions);
        IReadOnlyList<IGattCharacteristic> Characteristics { get; }
    }
}
