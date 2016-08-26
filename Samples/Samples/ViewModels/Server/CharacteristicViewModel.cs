using System;
using System.Windows.Input;
using Acr.Ble.Server;
using Acr.UserDialogs;
using ReactiveUI.Fody.Helpers;


namespace Samples.ViewModels.Server
{
    public class CharacteristicViewModel : AbstractViewModel
    {
        readonly IGattCharacteristic characteristic;


        public CharacteristicViewModel(IUserDialogs dialogs, IGattCharacteristic characteristic)
        {
            this.Characteristic = characteristic;

            characteristic
                .WhenReadReceived()
                .Subscribe(async x =>
                {
                    var result = await dialogs.PromptAsync($"Read Request {this.Characteristic.Uuid}");
                    //if (result.Ok)
                });

            characteristic
                .WhenWriteReceived()
                .Subscribe(x =>
                {
                    //var result = await dialogs.PromptAsync($"Write Received");
                });

            characteristic
                .WhenSubscriptionStateChanged()
                .Subscribe(x => this.IsSubscribed = x);
        }


        public IGattCharacteristic Characteristic { get; }
        public ICommand Actions { get; set; }
        [Reactive] public bool IsSubscribed { get; set; }
    }
}
