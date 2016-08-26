using System;
using System.Windows.Input;
using Acr;
using ReactiveUI.Fody.Helpers;


namespace Samples.ViewModels.Server
{
    public class AddCharacteristicViewModel : AbstractViewModel
    {
        public AddCharacteristicViewModel()
        {
            this.GenerateRandom = new Command(() => this.Uuid = Guid.NewGuid().ToString());
        }


        public void Reset()
        {
            this.Uuid = String.Empty;
            this.CanNotify = false;
            this.CanWrite = false;
            this.CanRead = false;
        }


        public ICommand GenerateRandom { get; }
        public ICommand Create { get; set; }
        [Reactive] public string Uuid { get; set; }
        [Reactive] public bool CanWrite { get; set; }
        [Reactive] public bool CanRead { get; set; }
        [Reactive] public bool CanNotify { get; set; }
    }
}
