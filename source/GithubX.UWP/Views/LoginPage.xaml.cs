using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GithubX.Shared.Services;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;

namespace GithubX.UWP.Views
{
	public sealed partial class LoginPage : Page
	{
		public LoginPage() => InitializeComponent();

		private async void Login_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var url = GithubService.Auth.GetOauthUrl(GithubService.Client, GithubService.ClientId, GithubService.FallBackUri);
				WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
						WebAuthenticationOptions.None, url, new Uri(GithubService.FallBackUri));
				if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
				{
					var response = WebAuthenticationResult.ResponseData;
					var cred = await GithubService.Auth.GetCredentialsAsync(GithubService.Client, response, GithubService.ClientId, GithubService.ClientSecret);
					GithubService.Auth.SaveCredential(cred);
					GithubService.SetClient(GithubService.Auth.ReadCredential().Password);
					Frame.Navigate(typeof(MotherPage));
				}
				else if (WebAuthenticationResult.ResponseStatus != WebAuthenticationStatus.UserCancel)
					throw new Exception(WebAuthenticationResult.ResponseStatus.ToString());
			}
			catch (Exception _)
			{
				//Logger.Init();
				//Logger.E(_);
				var dialog = new MessageDialog("Error! Try Again");
				dialog.Commands.Add(new UICommand("Close"));
				await dialog.ShowAsync();
			}
		}
	}
}