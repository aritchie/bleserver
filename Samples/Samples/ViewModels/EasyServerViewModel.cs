using System;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Acr.Ble.Server;
using ReactiveUI.Fody.Helpers;
using Samples.Services;
using Xamarin.Forms;
using Command = Acr.Command;


namespace Samples.ViewModels
{
    public class EasyServerViewModel : AbstractRootViewModel
    {
        readonly IGattServer server;
        IDisposable notifyBroadcast;


        public EasyServerViewModel(ICoreServices services, IGattServerFactory factory) : base(services)
        {
            this.server = factory.CreateInstance();
            var service = this.server.AddService(Guid.NewGuid(), true);
            var characteristic = service.AddCharacteristic(
                Guid.NewGuid(),
                CharacteristicProperties.Read | CharacteristicProperties.Write,
                CharacteristicPermissions.Writeable
            );
            var notifyCharacteristic = service.AddCharacteristic
            (
                Guid.NewGuid(),
                CharacteristicProperties.Notify,
                CharacteristicPermissions.Readable
            );

            //var descriptor = characteristic.AddDescriptor(Guid.NewGuid());

            notifyCharacteristic.WhenSubscriptionStateChanged().Subscribe(subscribed =>
            {
                this.OnEvent($"Characteristic Subscription State: {subscribed}");

                if (!subscribed)
                {
                    this.notifyBroadcast?.Dispose();
                    this.notifyBroadcast = null;
                }
                else if (this.notifyBroadcast == null)
                {
                    this.OnEvent("Characteristic Broadcast Started");
                    this.notifyBroadcast = Observable
                        .Interval(TimeSpan.FromSeconds(1))
                        .Subscribe(_ =>
                        {
                            var dt = DateTime.Now.ToString("g");
                            var bytes = Encoding.UTF8.GetBytes(dt);
                            notifyCharacteristic.Broadcast(bytes);
                        });
                }
            });

            characteristic.WhenReadReceived().Subscribe(x =>
            {
                this.OnEvent("Characteristic Read Received");
                x.Value = Encoding.UTF8.GetBytes(this.CharacteristicValue);
            });
            characteristic.WhenWriteReceived().Subscribe(x =>
            {
                var write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);
                this.OnEvent($"Characteristic Write Received - {write}");
            });

            //descriptor.WhenReadReceived().Subscribe(x =>
            //    this.OnEvent("Descriptor Read Received")
            //);
            //descriptor.WhenWriteReceived().Subscribe(x =>
            //{
            //    var write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);
            //    this.OnEvent($"Descriptor Write Received - {write}");
            //});

            this.ToggleServer = new Command(() =>
            {
                if (this.server.IsRunning)
                {
                    this.ServerText = "Start Server";
                    this.server.Stop();
                }
                else
                {
                    this.ServerText = "Stop Server";
                    this.server.Start(new AdvertisementData());
                }
            });
            this.Clear = new Command(() => this.Output = String.Empty);
        }


        [Reactive] public string ServerText { get; private set; } = "Start Server";
        [Reactive] public string CharacteristicValue { get; set; }
        [Reactive] public string DescriptorValue { get; set; }
        [Reactive] public string Output { get; private set; }
        public ICommand ToggleServer { get; }
        public ICommand Clear { get; }


        void OnEvent(string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
                this.Output += msg + Environment.NewLine
            );
        }
    }
}
