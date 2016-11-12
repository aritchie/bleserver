using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr;
using Acr.Ble.Server;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Samples.Services;
using Xamarin.Forms;
using Command = Acr.Command;


namespace Samples.ViewModels
{
    public class EasyServerViewModel : AbstractRootViewModel
    {
        readonly IBleAdapter adapter;
        IGattServer server;
        IDisposable notifyBroadcast;


        public EasyServerViewModel(ICoreServices services, IBleAdapter adapter) : base(services)
        {
            this.adapter = adapter;
            adapter
                .WhenAdapterStatusChanged()
                .Subscribe(x => this.Status = x);
            
            this.ToggleServer = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(
                    x => x.Status,
                    x => x.Value == AdapterStatus.PoweredOn
                ),
                _ =>
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
                            LocalName = "Allan",
                            ManufacturerId = 127,
                            ManufacturerData = new byte[] { 0x1, 0x1, 0x1, 0x1 }
                        });
                        this.OnEvent("Server Started");
                    }
                    return Task.FromResult(new object());
                }
            );
            this.Clear = new Command(() => this.Output = String.Empty);
        }


        [Reactive] public string ServerText { get; private set; } = "Start Server";
        [Reactive] public string CharacteristicValue { get; set; }
        [Reactive] public string DescriptorValue { get; set; }
        [Reactive] public string Output { get; private set; }
        [Reactive] public AdapterStatus Status { get; private set; }
        public ICommand ToggleServer { get; }
        public ICommand Clear { get; }


        void BuildServer()
        {
            if (this.server != null)
                return;

            try
            {
                this.server = this.adapter.CreateGattServer();
                var service = this.server.AddService(Guid.NewGuid(), true);
                this.OnEvent($"Service Added - {service.Uuid}");

                var characteristic = service.AddCharacteristic(
                    Guid.NewGuid(),
                    CharacteristicProperties.Read | CharacteristicProperties.Write | CharacteristicProperties.WriteWithoutResponse,
                    GattPermissions.Read | GattPermissions.Write
                );
                this.OnEvent($"Characteristic Read/Write Added - {characteristic.Uuid}");

                var notifyCharacteristic = service.AddCharacteristic
                (
                    Guid.NewGuid(),
                    CharacteristicProperties.Indicate | CharacteristicProperties.Notify,
                    GattPermissions.Read | GattPermissions.Write
                );
                this.OnEvent($"Characteristic Notify Added - {notifyCharacteristic.Uuid}");

                //var descriptor = characteristic.AddDescriptor(Guid.NewGuid(), Encoding.UTF8.GetBytes("Test Descriptor"));

                notifyCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
                {
                    var @event = e.IsSubscribed ? "Subscribed" : "Unsubcribed";
                    this.OnEvent($"Device {e.Device.Uuid} {@event}");
                    this.OnEvent($"Charcteristic Subcribers: {notifyCharacteristic.SubscribedDevices.Count}");

                    if (this.notifyBroadcast == null)
                    {
                        this.OnEvent("Starting Subscriber Thread");
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

                    x.Value = Encoding.UTF8.GetBytes(write);
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
