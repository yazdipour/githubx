using System;
using Windows.UI.Xaml.Controls;

namespace GithubX.UWP.Views
{
	public sealed partial class AboutPanel : ContentDialog
	{
		public AboutPanel()
		{
			this.InitializeComponent();
			if(App.Releasing) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Visit AboutPage");
		}
		

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private async void RatingControl_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri(Services.Api.Api.RateAppUri));
		}
	}
}
