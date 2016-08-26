using System;
using Acr;
using Xamarin.Forms;


namespace Samples.Pages.Server
{
	public partial class SetupPage : TabbedPage
	{
		public SetupPage ()
		{
			this.InitializeComponent();
		}


	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            (this.BindingContext as IViewModelLifecycle)?.OnActivate();
	    }


	    protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
            (this.BindingContext as IViewModelLifecycle)?.OnDeactivate();
	    }
	}
}

