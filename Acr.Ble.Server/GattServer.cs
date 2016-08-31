﻿using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public class GattServer : IGattServer
    {
        public GattServer()
        {
            throw new ArgumentException("This is the PCL library of Acr.Ble.Server.  Did you include the nuget package in your platform project?");
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool IsRunning { get; }
        public void Start(AdvertisementData adData)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public IGattService AddService(Guid uuid, bool primary)
        {
            throw new NotImplementedException();
        }

        public void RemoveService(IGattService service)
        {
            throw new NotImplementedException();
        }

        public void ClearServices()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IGattService> Services { get; }
    }
}