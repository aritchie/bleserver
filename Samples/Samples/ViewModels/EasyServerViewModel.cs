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
        readonly IGattServerFactory factory;
        IGattServer server;
        IDisposable notifyBroadcast;


        public EasyServerViewModel(ICoreServices services, IGattServerFactory factory) : base(services)
        {
            this.factory = factory;

            this.ToggleServer = new Command(() =>
            {
                this.BuildServer();

                if (this.server.IsRunning)
                {
                    this.ServerText = "Start Server";
                    this.server.Stop();
                    this.OnEvent("Server Stopped");
                }
                else
                {

                    this.ServerText = "Stop Server";
                    this.server.Start(new AdvertisementData
                    {
                        LocalName = "Allan"
                    });
                    this.OnEvent("Server Started");
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


        void BuildServer()
        {
            if (this.server != null)
                return;

            try
            {
                this.server = this.factory.CreateInstance();
                var service = this.server.AddService(Guid.NewGuid(), true);
                this.OnEvent($"Service Added - {service.Uuid}");

                var characteristic = service.AddCharacteristic(
                    Guid.NewGuid(),
                    CharacteristicProperties.Read | CharacteristicProperties.Write,
                    CharacteristicPermissions.Writeable
                );
                this.OnEvent($"Characteristic Read/Write Added - {characteristic.Uuid}");

                var notifyCharacteristic = service.AddCharacteristic
                (
                    Guid.NewGuid(),
                    CharacteristicProperties.Notify,
                    CharacteristicPermissions.Readable
                );
                this.OnEvent($"Characteristic Notify Added - {notifyCharacteristic.Uuid}");

                //var descriptor = characteristic.AddDescriptor(Guid.NewGuid());

                notifyCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
                {
                    var @event = e.IsSubscribed ? "Subscribed" : "Unsubcribed";
                    this.OnEvent($"Device {e.Device.Uuid} {@event}");
                    this.OnEvent($"Characteristic Subscribers: {notifyCharacteristic.SubscribedDevices.Count}");

                    if (notifyCharacteristic.SubscribedDevices.Count == 0)
                    {
                        this.notifyBroadcast?.Dispose();
                        this.notifyBroadcast = null;
                        this.OnEvent("No subcribers to characteristic");
                    }
                    else if (this.notifyBroadcast == null)
                    {
                        this.OnEvent("Starting Subscriber Thread");
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
                    x.Value = Encoding.UTF8.GetBytes(this.CharacteristicValue);
                    this.OnEvent("Characteristic Read Received");
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
            }
            catch (Exception ex)
            {
                this.Dialogs.Alert("Error building gatt server - " + ex);
            }
        }


        void OnEvent(string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
                this.Output += msg + Environment.NewLine
            );
        }
    }
}
