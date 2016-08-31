using System;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Samples.Services;


namespace Samples.ViewModels
{
    public class BeaconAdvertisementViewModel : AbstractRootViewModel
    {
        public BeaconAdvertisementViewModel(ICoreServices services) : base(services)
        {
            this.Uuid = "";
            this.Major = ushort.MaxValue - 1;
            this.Minor = ushort.MaxValue - 1;

            this.StartAdvertising = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(
                    x => x.IsAdvertising,
                    x => x.Uuid,
                    x => x.Major,
                    x => x.Minor,
                    (status, uuid, major, minor) =>
                    {
                        if (status.Value)
                            return false;

                        if (minor.Value == 0)
                            return false;

                        if (major.Value == 0)
                            return false;

                        if (uuid.Value.Length != 32)
                            return false;

                        return true;
                    }
                ),
                async x =>
                {
                    this.IsAdvertising = true;
                }
            );
            this.StopAdvertising = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(
                    x => x.IsAdvertising,
                    x => !x.Value
                ),
                async x =>
                {
                    this.IsAdvertising = false;
                }
            );
        }



        public ICommand StartAdvertising { get; }
        public ICommand StopAdvertising { get; }

        [Reactive] public bool IsAdvertising { get; private set; }
        [Reactive] public string Uuid { get; set; }
        [Reactive] public ushort Major { get; set; }
        [Reactive] public ushort Minor { get; set; }
    }
}
