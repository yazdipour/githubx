using System;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class AboutPanel : ContentDialog
	{
		public AboutPanel()
		{
			this.InitializeComponent();
			if(Services.Api.ApiKeys.AppCenter!=null) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Visit AboutPage");
			Title = "Github✘ v" + App.Version;
		}

		private async void RatingControl_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri(Services.Api.ApiHandler.RateAppUri));
		}
	}
}
