using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr;
using Acr.Ble.Server;
using Acr.UserDialogs;
using Samples.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Samples.ViewModels.Server
{
	public class SetupViewModel : AbstractRootViewModel
	{
	    IGattServer server;
	    IGattService service;


		public SetupViewModel(ICoreServices services, AddCharacteristicViewModel addViewModel) : base(services)
		{
		    this.AddViewModel = addViewModel;
            this.server = new GattServer();
		    this.service = this.server.AddService(Guid.NewGuid(), true);

		    this.WhenAnyValue(x => x.IsAdvertising)
		        .Subscribe(x => this.StateText = x ? "Stop Server" : "Start Server");



		    this.ToggleServer = ReactiveCommand.CreateAsyncTask(_ =>
		    {
                if (this.server.IsRunning)
		        {
		            this.server.Stop();
                    this.IsAdvertising = false;
		        }
		        else
		        {
                    this.server.Start(new AdvertisementData
                    {
                        LocalName = this.LocalName,
                        IncludeDeviceName = this.IncludeDeviceName,
                        IncludeTxPower = this.IncludeTxPower,
                        IsConnectable = this.IsConnectable
                    });
                    this.IsAdvertising = true;
		        }
		        return Task.FromResult(false);
		    });


		    addViewModel.Create = ReactiveCommand.CreateAsyncTask(
                addViewModel.WhenAny(
                    x => x.CanWrite,
                    x => x.CanRead,
                    x => x.CanNotify,
                    x => x.Uuid,
                    (write, read, not, id) =>
                    {
                        var _ = Guid.Empty;
                        if (!Guid.TryParse(id.Value, out _))
                            return false;

                        return write.Value || read.Value || not.Value;
                    }
                ),
		        _ =>
		        {
		            var uuid = Guid.Parse(addViewModel.Uuid);
		            var props = CharacteristicProperties.Read;
                    if (addViewModel.CanNotify)
                        props |= CharacteristicProperties.Broadcast;

                    if (addViewModel.CanRead)
                        props |= CharacteristicProperties.Read;

                    if (addViewModel.CanWrite)
                        props |= CharacteristicProperties.Write;

		            var characteristic = this.service.AddCharacteristic(
		                uuid,
		                props,
		                CharacteristicPermissions.Readable | CharacteristicPermissions.Writeable
		            );
		            var vm = new CharacteristicViewModel(this.Dialogs, characteristic);
		            vm.Actions = new Command(() =>
		            {
		                var cfg = new ActionSheetConfig()
		                    .SetCancel()
		                    .SetDestructive("Remove", () =>
		                    {
                                this.service.RemoveCharacteristic(vm.Characteristic);
		                        this.Characteristics.Remove(vm);
		                    })
		                    .SetTitle("Actions");

		                if (vm.Characteristic.Properties.HasFlag(CharacteristicProperties.Broadcast))
		                {
		                    cfg.Add("Broadcast", async () =>
		                    {
		                        var result = await this.Dialogs.PromptAsync("Enter value to broadcast");
		                        if (!result.Ok)
		                            return;

		                        var bytes = Encoding.UTF8.GetBytes(result.Text); // TODO: > 20 bytes
                                vm.Characteristic.Broadcast(bytes);
		                    });
		                }
                        // TODO: set value for reads?
		                this.Dialogs.ActionSheet(cfg);
		            });
		            this.Characteristics.Add(vm);
		            addViewModel.Reset();

		            return Task.FromResult(false);
		        }
		    );
		}


	    public override void OnDeactivate()
	    {
	        base.OnDeactivate();
            this.server?.Dispose();
	    }


        public AddCharacteristicViewModel AddViewModel { get; }
        public ICommand ToggleServer { get; }
        [Reactive] public string LocalName { get; set; }
	    [Reactive] public string StateText { get; private set; }
		[Reactive] public bool IsAdvertising { get; private set; }
        [Reactive] public bool IncludeTxPower { get; set; }
        [Reactive] public bool IncludeDeviceName { get; set; }
        [Reactive] public bool IsConnectable { get; set; }

        public ObservableCollection<CharacteristicViewModel> Characteristics { get; } = new ObservableCollection<CharacteristicViewModel>();
	}
}

