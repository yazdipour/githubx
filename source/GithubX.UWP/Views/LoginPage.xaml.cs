using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GithubX.Shared.Services;
using Windows.UI.Popups;

namespace GithubX.UWP.Views
{
	public sealed partial class LoginPage : Page
	{
		public LoginPage() => InitializeComponent();

		private async void Page_Loading(FrameworkElement sender, object args)
		{
			Logger.Init(Shared.Keys.AppCenteerToken);
			loadingControl.IsEnabled = true;
			var cred = await GithubService.Auth.ReadCredential();
			loadingControl.IsEnabled = false;
			if (cred == null) return;
			GithubService.SetClient(cred.Password);
			if (GithubService.Auth.IsLoggedIn())
				Frame.Navigate(typeof(MotherPage));
		}

		private async void Login_Click(object sender, RoutedEventArgs e)
		{
			loadingControl.IsEnabled = true;
			loadingControl.Tag = "Waiting for Authentication...";
			try
			{
				var url = GithubService.Auth.GetOauthUrl();
				await Helpers.Utils.OpenUri(url);
				Octokit.Credentials cred = null;
				while ((cred = await GithubService.Auth.ReadCredential()) == null)
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3));
				GithubService.SetClient(cred?.Password);
				Frame.Navigate(typeof(MotherPage));
			}
			catch (Exception _)
			{
				Logger.E(_);
				var dialog = new MessageDialog("Error! Try Again");
				dialog.Commands.Add(new UICommand("Close"));
				await dialog.ShowAsync();
			}
			finally
			{
				loadingControl.IsEnabled = false;
			}
		}
	}
}