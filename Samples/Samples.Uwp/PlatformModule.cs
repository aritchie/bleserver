﻿using System;
using Autofac;


namespace Samples.Uwp
{
    public class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new CoreModule());
        }
    }
}
